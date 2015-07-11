using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.UI;
using Xynapse.UI.Controls;
using Xynapse.Input;

using Xybrid.Graphics;
using Xybrid.Util;

namespace Xybrid.UI {
    public class XybridWindowFrame : WindowFrame {
        public class TitleBar : UIContainerFlat {
            public XybridWindowFrame frame;

            public TextDrawable label = new TextDrawableFreetype();
            public int labelOffset = 4;

            public Button buttonClose;
            public Button buttonMaximize;
            public Button buttonMinimize;

            public TitleBar(XybridWindowFrame frame) {
                this.frame = frame;

                buttonClose = AddChild(new Button()).OnButtonClick(0, () => frame.form.Close());
                buttonClose.Text = "x";
                buttonMaximize = AddChild(new Button()).OnButtonClick(0, () => {
                    if (frame.form.WindowState == System.Windows.Forms.FormWindowState.Maximized) frame.form.WindowState = System.Windows.Forms.FormWindowState.Normal;
                    else frame.form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                });
                buttonMaximize.Text = "[ ]";
                buttonMinimize = AddChild(new Button()).OnButtonClick(0, () => frame.form.WindowState = System.Windows.Forms.FormWindowState.Minimized);
                buttonMinimize.Text = " _ ";

                OnResize();
            }

            public override bool InterceptMouse(PxVector mousePos) { return true; }
            public override bool IsDraggable(int button) { return button == 0 && frame.form.WindowState != System.Windows.Forms.FormWindowState.Maximized; }
            public override void OnDrag(InputState input, int button, PxVector deltaFrame, PxVector deltaStart) {
                frame.form.Location = new System.Drawing.Point(frame.form.Location.X + deltaFrame.X, frame.form.Location.Y + deltaFrame.Y);
            }

            public override void OnResize() {
                int border = 2;
                int bh = H - (border * 2);
                int bw = (int)(bh * 1.5);
                buttonClose.Size = buttonMaximize.Size = buttonMinimize.Size = new PxVector(bw, bh);
                buttonClose.Position = new PxVector(W - (border + bw) * 1, border);
                buttonMaximize.Position = new PxVector(W - (border + bw) * 2, border);
                buttonMinimize.Position = new PxVector(W - (border + bw) * 3, border);

            }

            public override void Draw() {
                //if (!needsRedraw) return;
                //needsRedraw = false;
                ThemeManager.FetchDrawable("window.title.background").Draw(this, ViewRect);
                label.Text = frame.window.Title;
                label.Draw(this, new PxRect(labelOffset, 0, W - labelOffset, H));
                DrawChildren();
            }
        }

        public class ResizeHandle : UIControl {
            public XybridWindowFrame frame;
            public ResizeHandle(XybridWindowFrame frame) {
                this.frame = frame;
            }
            

            PxVector origSize;
            bool shouldDrag = false;
            public override bool InterceptMouse(PxVector mousePos) { shouldDrag = (mousePos.X + mousePos.Y >= W); return shouldDrag; }
            public override bool IsDraggable(int button) { return shouldDrag && button == 0; }
            public override void OnMouseDown(InputState input, int button) {
                if (button == 0) origSize = ((System.Drawing.Point)frame.form.Size).PxVector();
            }
            public override void OnDrag(InputState input, int button, PxVector deltaFrame, PxVector deltaStart) {
                frame.form.Size = new System.Drawing.Size((origSize + deltaStart).DPoint());
            }

            public override void Draw() {
                ThemeManager.FetchDrawable("window.resizeHandle").Draw(Parent, ViewRect);
            }
        }

        //
        public WindowBase window;

        public TitleBar titleBar;
        public ResizeHandle resizeHandle;

        public XybridWindowFrame(WindowBase window, UIForm form) {
            this.form = form;
            this.window = window;

            titleBar = new TitleBar(this);
            titleBar.Size = new PxVector(W, titleHeight);

            resizeHandle = new ResizeHandle(this);
            resizeHandle.Size = new PxVector(16, 16);

            /*var b = AddChild(new Button());
            b.Text = "x";
            b.Rect = new PxRect(4, 4, 16, 16);
            b.OnButtonClick(0, () => form.Close());*/

            RebuildList();
        }

        public override WindowBase Window { get { return window; } }

        const int frameWidth = 2;
        const int titleHeight = 24;

        public override PxVector ClientToWindowPos(PxVector inp) {
            if (!hasTitleBar) return inp;
            return inp + new PxVector(frameWidth, titleHeight);
        }
        public override PxVector WindowToClientPos(PxVector inp) {
            if (!hasTitleBar) return inp;
            return inp + new PxVector(frameWidth, titleHeight);
        }
        public override PxVector ClientToWindowSize(PxVector inp) {
            if (!hasTitleBar) return inp;
            return inp + new PxVector(frameWidth * 2, titleHeight + frameWidth);
        }
        public override PxVector WindowToClientSize(PxVector inp) {
            if (!hasTitleBar) return inp;
            return inp - new PxVector(frameWidth * 2, titleHeight + frameWidth);
        }

        public UIForm form;

        #region window control props
        bool hasTitleBar = true;
        public override bool HasTitleBar {
            get {
                return hasTitleBar;
            }
            set {
                if (value != hasTitleBar) {
                    hasTitleBar = value;
                    SetWindowDims();
                    if (hasTitleBar) form.Location = (form.Location.PxVector() - new PxVector(frameWidth, titleHeight)).DPoint();
                    else form.Location = (form.Location.PxVector() + new PxVector(frameWidth, titleHeight)).DPoint();

                    RebuildList();
                }
            }
        }
        bool manuallyResizable = true;
        public override bool ManuallyResizable {
            get {
                return manuallyResizable;
            }
            set {
                throw new NotImplementedException();
            }
        }
        #endregion

        public override DrawableCanvas canvas { get; set; }

        public override void OnResize() {
            titleBar.Size = new PxVector(W, titleHeight);
            resizeHandle.Position = Size - resizeHandle.Size;
            SetWindowDims();
            RebuildList();
        }

        internal void SetWindowDims() {
            if (hasTitleBar) {
                Window.Position = new PxVector(frameWidth, titleHeight);
                Window.Size = Size - new PxVector(frameWidth * 2, titleHeight + frameWidth);
            }
            else {
                Window.Position = new PxVector(0, 0);
                Window.Size = Size;
            }
        }

        internal void RebuildList() {
            if (!hasTitleBar) { Resnap(titleBar, null); }
            else {
                Resnap(titleBar, this);
                Resnap(Window, this);
                if (manuallyResizable && form.WindowState != System.Windows.Forms.FormWindowState.Maximized) {
                    Resnap(resizeHandle, this);
                }
                else Resnap(resizeHandle, null);
            }
        }
        private void Resnap(UIControl control, UIContainer parent) {
            if (parent == control.Parent) control.Parent = null;
            control.Parent = parent;
        }

        public override void Draw() {
            if (!needsRedraw) return;
            needsRedraw = false;
            ThemeManager.FetchDrawable("window.frame.background").Draw(this, this.ViewportRect);
            //ThemeManager.FetchDrawable("window.title.background").Draw(this, new PxRect(0, 0, this.W, titleHeight));
            DrawChildren();
            ThemeManager.FetchDrawable("window.frame.border").Draw(this, this.ViewportRect);
        }

        public override bool InterceptMouse(PxVector mousePos) { return true; }
    }
}

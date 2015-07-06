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
            public TitleBar(XybridWindowFrame frame) {
                this.frame = frame;
            }

            public override bool InterceptMouse(PxVector mousePos) { return true; }
            public override bool IsDraggable(int button) { return button == 0; }
            public override void OnDrag(InputState input, int button, PxVector deltaFrame, PxVector deltaStart) {
                frame.form.Location = new System.Drawing.Point(frame.form.Location.X + deltaFrame.X, frame.form.Location.Y + deltaFrame.Y);
            }

            public override void Draw() {
                ThemeManager.FetchDrawable("window.title.background").Draw(this, ViewRect);
                label.Text = frame.window.Title;
                label.Draw(this, new PxRect(labelOffset, 0, W - labelOffset, H));
                DrawChildren();
            }
        }

        public class ResizeHandle : UIControl {

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
            SetWindowDims();
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
            }
        }
        private void Resnap(UIControl control, UIContainer parent) {
            if (parent == control.Parent) control.Parent = null;
            control.Parent = parent;
        }

        public override void Draw() {
            ThemeManager.FetchDrawable("window.frame.background").Draw(this, this.ViewportRect);
            //ThemeManager.FetchDrawable("window.title.background").Draw(this, new PxRect(0, 0, this.W, titleHeight));
            DrawChildren();
            ThemeManager.FetchDrawable("window.frame.border").Draw(this, this.ViewportRect);
        }


        // hmm.
        bool resizing = false;
        System.Drawing.Size origSize;
        public override bool IsDraggable(int button) {
            if (button != 573) return false;
            if (Window.ScreenRect.Contains(UIHandler.instance.inputState.MousePosition)) return false;
            resizing = (UIHandler.instance.inputState.MousePosition - ScreenRect.Position).Y > titleHeight;
            if (resizing) origSize = form.Size;
            return true;
        }
        public override bool InterceptMouse(PxVector mousePos) { return true; }
        public override void OnDrag(InputState input, int button, PxVector deltaFrame, PxVector deltaStart) {
            if (resizing) form.Size = new System.Drawing.Size(new System.Drawing.Point(origSize.Width + deltaStart.X, origSize.Height + deltaStart.Y));
            else form.Location = new System.Drawing.Point(form.Location.X + deltaFrame.X, form.Location.Y + deltaFrame.Y);
        }
    }
}

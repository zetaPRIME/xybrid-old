using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework;

using Xynapse.UI;
using Xynapse.Input;

using Xybrid.UI;
using Xybrid.Graphics;
using Xybrid.Util;

using Rectangle = Microsoft.Xna.Framework.Rectangle;
using XPoint = Microsoft.Xna.Framework.Point;
using DPoint = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Xybrid {
    public partial class UIForm : Form {
        public UIForm(WindowBase window) {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.window = window;
            frame = new XybridWindowFrame(window, this);
            InitializeComponent();
        }

        public WindowBase window;
        public XybridWindowFrame frame;
        public SwapChainRenderTarget target;

        public override Size MinimumSize { get { return SizeFromClientSize(new Size(frame.ClientToWindowSize(window.MinimumSize).DPoint())); } }
        public override Size MaximumSize { get { return SizeFromClientSize(new Size(frame.ClientToWindowSize(window.MaximumSize).DPoint())); } }

        const int WS_MINIMIZEBOX = 0x20000;
        const int CS_DBLCLKS = 0x8;
        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                cp.ClassStyle |= CS_DBLCLKS;
                return cp;
            }
        }

        //internal string prevName = "";

        protected override void OnLoad(EventArgs e) {
            window.Size = window.DefaultSize;
            ClientSize = new System.Drawing.Size(window.Size.DPoint());

            frame.Position = this.PointToScreen(new DPoint(0, 0)).PxVector();
            frame.Size = new PxVector(ClientSize.Width, ClientSize.Height);
            frame.SetWindowDims();

            RemakeTarget();
        }

        protected override void OnMove(EventArgs e) {
            frame.Position = this.PointToScreen(new DPoint(0, 0)).PxVector();
        }

        protected override void OnResize(EventArgs e) {
            RemakeTarget();
            frame.Size = new PxVector(ClientSize.Width, ClientSize.Height);
        }

        void RemakeTarget() {
            //const int step = 64;
            if (target == null || target.Width != ClientSize.Width || target.Height != ClientSize.Height) {
                if (target != null) target.Dispose();
                int width = ClientSize.Width; //(int)Math.Ceiling((float)ClientSize.Width / step) * step;
                int height = ClientSize.Height; //(int)Math.Ceiling((float)ClientSize.Height / step) * step;
                target = new SwapChainRenderTarget(GraphicsManager.device, Handle, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents, PresentInterval.Default);
                frame.canvas = new Canvas(target);
            }
        }

        protected override void OnClosing(CancelEventArgs e) {
            if (window.OnClose()) {
                UIManager.windows.Remove(this);
                if (UIManager.dragForm == this) UIManager.dragForm = null;
            }
            else e.Cancel = true;
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            UIHandler.scrollValue += e.Delta / Math.Abs(e.Delta);
        }

        public List<UIControl> mouseOverLastFrame = new List<UIControl>();
        public List<UIControl> mouseOverThisFrame = new List<UIControl>();

        UIControl dragControl;
        PxVector dragStart, dragLast;
        int dragButton;

        public void ProcessInputEvents(InputState input, bool mouseOver) {
            if (UIManager.dragForm == null) {
                {
                    var swap = mouseOverLastFrame;
                    mouseOverLastFrame = mouseOverThisFrame;
                    mouseOverThisFrame = swap;
                    mouseOverThisFrame.Clear();
                }
                if (mouseOver) frame.Dive((ctl) => ctl.ScreenRect.Contains(input.MousePosition), (ctl) => ctl.InterceptMouse(input.MousePosition - ctl.ScreenRect.Position), (ctl) => mouseOverThisFrame.Add(ctl));

                foreach (UIControl ctl in mouseOverLastFrame) if (!mouseOverThisFrame.Contains(ctl)) ctl.OnMouseLeave(input);
                foreach (UIControl ctl in mouseOverThisFrame) if (!mouseOverLastFrame.Contains(ctl)) ctl.OnMouseEnter(input);

                for (int i = 0; i < 3; i++) {
                    foreach (UIControl ctl in mouseOverThisFrame) {
                        if (input.MouseReleased(i)) ctl.OnMouseUp(input, i);
                        if (input.MousePressed(i)) {
                            ctl.OnMouseDown(input, i);
                            if (ctl.IsDraggable(i)) {
                                UIManager.dragForm = this;
                                dragControl = ctl;
                                dragStart = dragLast = input.MousePosition;
                                dragButton = i;
                                // todo: maybe abort rest of non-drag phase?
                            }
                        }
                    }
                }

                if (input.scrollWheel != 0) foreach (UIControl ctl in mouseOverThisFrame) {
                    if (ctl.OnScroll(input.scrollWheel)) break;
                    else if (ctl == mouseOverThisFrame.Last()) {
                        UIControl ctc = ctl.Parent;
                        while (ctc != null) {
                            if (ctc.OnScroll(input.scrollWheel)) break;
                            ctc = ctc.Parent;
                        }
                    }
                }
                
            }
            else if (UIManager.dragForm == this) {
                if (input.MouseReleased(dragButton)) {
                    dragControl.OnMouseUp(input, dragButton);
                    dragControl = null; // don't hold an old ref
                    UIManager.dragForm = null; // and of course
                }
                else if (input.MousePosition != dragLast) {
                    dragControl.OnDrag(input, dragButton, input.MousePosition - dragLast, input.MousePosition - dragStart);
                    dragLast = input.MousePosition;
                }
            }

        }
        //
        //
        //
    }
}

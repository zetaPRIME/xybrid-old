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
            //InitializeComponent();
            //Load += OnLoad;
            //num = n;

            windowDef = window;
        }

        public WindowBase windowDef;
        //public GameWindow window;
        public SwapChainRenderTarget target;

        //public XPoint? mpoint = null;

        public override Size MinimumSize { get { return SizeFromClientSize(new Size(windowDef.MinimumSize.DPoint())); } }
        public override Size MaximumSize { get { return SizeFromClientSize(new Size(windowDef.MaximumSize.DPoint())); } }

        protected override void OnLoad(EventArgs e) {
            windowDef.Size = windowDef.DefaultSize;
            ClientSize = new System.Drawing.Size(windowDef.Size.DPoint());

            /*target = new SwapChainRenderTarget(GraphicsManager.device, this.Handle, ClientSize.Width, ClientSize.Height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents, PresentInterval.Default);
            windowDef.canvas = new Canvas(target);*/
            RemakeTarget();
        }

        protected override void OnResize(EventArgs e) {
            RemakeTarget();
            windowDef.Size = new PxVector(ClientSize.Width, ClientSize.Height);
        }

        void RemakeTarget() {
            //const int step = 64;
            if (target == null || target.Width != ClientSize.Width || target.Height != ClientSize.Height) {
                if (target != null) target.Dispose();
                int width = ClientSize.Width; //(int)Math.Ceiling((float)ClientSize.Width / step) * step;
                int height = ClientSize.Height; //(int)Math.Ceiling((float)ClientSize.Height / step) * step;
                target = new SwapChainRenderTarget(GraphicsManager.device, Handle, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents, PresentInterval.Default);
                windowDef.canvas = new Canvas(target);
            }
        }

        protected override void OnClosing(CancelEventArgs e) {
            if (windowDef.OnClose()) UIManager.windows.Remove(this);
            else e.Cancel = true;
        }
    }
}

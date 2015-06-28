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

using Rectange = Microsoft.Xna.Framework.Rectangle;

namespace Xybrid {
    public partial class UIForm : Form {
        public UIForm(int n) {
            //InitializeComponent();
            Load += OnLoad;
            //num = n;

            Resize += cOnResize;

            FormClosed += (Object s, FormClosedEventArgs e) => {
                UIManager.windows.Remove(this);
            };
        }

        public WindowBase windowDef;
        public GameWindow window;
        public SwapChainRenderTarget target;

        void OnLoad(Object sender, EventArgs e) {
            //Debug.WriteLine("onload");
            //InitializeComponent();
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            if (this.DesignMode == false) {
                WindowsDeviceConfig.UseForm = false;
                WindowsDeviceConfig.ControlToUse = this;

                //window = GameWindow.Create(UIHandler.instance, ClientSize.Width, ClientSize.Height);
                target = new SwapChainRenderTarget(UIHandler.graphicsDevice, this.Handle, ClientSize.Width, ClientSize.Height);

                UIManager.windows.Add(this);

                /*game = new UIHandler(this);
                game.num = num;
                game.IsFixedTimeStep = false;
                UIManager.windows.Add(game);*/
            }
        }
        void cOnResize(Object sender, EventArgs e) {
            /*game.graphics.PreferredBackBufferWidth = this.ClientSize.Width;
            game.graphics.PreferredBackBufferHeight = this.ClientSize.Height;
            game.graphics.ApplyChanges();*/
            //target.

            target = new SwapChainRenderTarget(UIHandler.graphicsDevice, Handle, ClientSize.Width, ClientSize.Height);
        }
    }
}

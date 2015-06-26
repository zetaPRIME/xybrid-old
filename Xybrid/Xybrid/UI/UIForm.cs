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
using MonoGame.Framework;

namespace Xybrid {
    public partial class UIForm : Form {
        public UIForm(int n) {
            //InitializeComponent();
            Load += OnLoad;
            num = n;

            FormClosed += (Object s, FormClosedEventArgs e) => {
                closed = true;
            };
        }

        int num;
        Thread t = null;
        bool closed = false;

        void OnLoad(Object sender, EventArgs e) {
            Debug.WriteLine("onload");
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            if (this.DesignMode == false) {
                WindowsDeviceConfig.UseForm = false;
                WindowsDeviceConfig.ControlToUse = this;

                MyGame game = new MyGame(this);
                game.num = num;
                game.IsFixedTimeStep = false;

                t = new Thread(new ThreadStart(() => {
                    while (true) {
                        if (this.closed) return;
                        this.Invoke(new MethodInvoker(() => {
                            game.RunOneFrame();
                        }));

                        Thread.Sleep(16);
                    }
                }));

                t.IsBackground = true;
                //t.Name = "Monogame Thread";
                t.Start();
            }
        }
    }
}

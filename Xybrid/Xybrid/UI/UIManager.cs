using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Xna.Framework.Graphics;

using Xynapse.UI;

using Xybrid.Graphics;

namespace Xybrid.UI {
    public static class UIManager {
        public static List<UIForm> windows = new List<UIForm>();

        static Thread mainThread = null;
        static Thread uiUpdateThread = null;
        static Control mainThreadCtrl = null;
        public static void SpawnUIUpdateThread() {
            mainThread = Thread.CurrentThread;
            mainThreadCtrl = new Panel();
            mainThreadCtrl.CreateControl();
            uiUpdateThread = new Thread(UIUpdateThread);
            uiUpdateThread.IsBackground = true;
            uiUpdateThread.Start();
        }
        static void UIUpdateThread() {
            MethodInvoker run = new MethodInvoker(RunUIUpdate);
            try {
                while (true) {
                    // even-ish 16fps
                    mainThreadCtrl.Invoke(run);
                    Thread.Sleep(17);
                    mainThreadCtrl.Invoke(run);
                    Thread.Sleep(17);
                    mainThreadCtrl.Invoke(run);
                    Thread.Sleep(16);
                }
            }
            catch { }
        }
        static void RunUIUpdate() {
            UIHandler.instance.PreAllUpdate();
            List<UIForm> tmpwin = new List<UIForm>(windows); // copy
            foreach (UIForm win in tmpwin) UIHandler.RunFrame(win); //win.form.Invoke(new MethodInvoker(win.RunOneFrame));
        }

        internal static void OpenWindow(WindowBase window) {
            UIForm form = new UIForm(window);
            form.CreateControl();
            //form.target = new SwapChainRenderTarget(GraphicsManager.device, form.Handle, form.ClientSize.Width, form.ClientSize.Height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents, PresentInterval.Default);
            //window.canvas = new Canvas(form.target);

            windows.Add(form);
            form.window.QueueFullRedraw();
            form.Show();
        }

        public static UIForm GetForm(WindowBase window) { foreach (UIForm wind in windows) if (wind.window == window) return wind; return null; }

        internal static UIForm dragForm = null;
    }
}

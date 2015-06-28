using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            List<UIForm> tmpwin = new List<UIForm>(windows); // copy
            foreach (UIForm win in tmpwin) UIHandler.RunFrame(win); //win.form.Invoke(new MethodInvoker(win.RunOneFrame));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Xynapse.UI;

namespace Xynapse.Interop.Host {
    public class HostInterop {
        public static void LockIn() { locked = true; } // call when host is done placing hooks
        static bool locked = false;

        private static Func<string, Drawable> _loadThemeImage;
        public static Func<string, Drawable> loadThemeImage {
            get { return _loadThemeImage; }
            set { if (!locked) _loadThemeImage = value; }
        }
        private static Func<string, Assembly, Drawable> _loadPluginImage;
        public static Func<string, Assembly, Drawable> loadPluginImage {
            get { return _loadPluginImage; }
            set { if (!locked) _loadPluginImage = value; }
        }

        private static Func<int, int, DrawableCanvas> _getCanvas;
        public static Func<int, int, DrawableCanvas> getCanvas {
            get { return _getCanvas; }
            set { if (!locked) _getCanvas = value; }
        }

        private static Action<WindowBase> _openWindow;
        public static Action<WindowBase> openWindow {
            get { return _openWindow; }
            set { if (!locked) _openWindow = value; }
        }
    }
}

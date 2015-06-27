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

        private static Func<Drawable, string> _loadThemeImage;
        public static Func<Drawable, string> loadThemeImage {
            get { return _loadThemeImage; }
            set { if (!locked) _loadThemeImage = value; }
        }
        private static Func<Drawable, Assembly, string> _loadPluginImage;
        public static Func<Drawable, Assembly, string> loadPluginImage {
            get { return _loadPluginImage; }
            set { if (!locked) _loadPluginImage = value; }
        }
    }
}

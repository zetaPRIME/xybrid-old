using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Xynapse.UI;

namespace Xynapse.Interop.Host {
    public class HostInterop {
        private static HostInterop host = null;
        public static HostInterop Host { get { return host; } set { if (host == null) host = value; } }

        // // //

        public virtual Drawable LoadThemeImage(string name) { return Drawable.None; }
        public virtual Drawable LoadPluginImage(string name, Assembly plugin) { return Drawable.None; }
        public virtual TextDrawable GetTextDrawable(string font = default, TextAlign align = TextAlign.Left) { return null; }

        public virtual DrawableCanvas GetCanvas(int width, int height) { return null; }

        public virtual void OpenWindow(WindowBase window) { }
        public virtual void SetDefaults(UIControl control, string subtype = "default") { }
    }
}

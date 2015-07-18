using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.Interop.Host;
using Xynapse.UI;
using Xynapse.Util;

using Xybrid.UI;
using Xybrid.Graphics;

namespace Xybrid {
    public static class InteropManager {
        private class XybridHost : HostInterop {
            public override Drawable LoadThemeImage(string name) { return ThemeManager.FetchDrawable(name); }
            public override TextDrawable GetTextDrawable(string font = default, TextAlign align = TextAlign.Left) { return new TextDrawableFreetype(font, "", align); }

            public override DrawableCanvas GetCanvas(int width, int height) { return new Canvas(width, height); }

            public override void OpenWindow(WindowBase window) { UIManager.OpenWindow(window); }
            public override void SetDefaults(UIControl control, string subtype) { ThemeManager.SetDefaults(control, subtype); }
        }

        public static void Init() {
            HostInterop.Host = new XybridHost();
        }
    }
}

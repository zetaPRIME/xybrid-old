using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.Interop.Host;

using Xybrid.Graphics;

namespace Xybrid {
    public static class InteropManager {
        public static void Init() {
            HostInterop.loadThemeImage = ThemeManager.FetchDrawable;

            HostInterop.getCanvas = (x, y) => { return new Canvas(x, y); };

            // done, keep other things from interfering
            HostInterop.LockIn();
        }
    }
}

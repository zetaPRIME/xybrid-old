using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.Interop.Host;

namespace Xynapse.UI {
    public abstract class DrawableCanvas : Drawable, DrawContext {
        public static DrawableCanvas Get(int x, int y) {
            return HostInterop.getCanvas(x, y);
        }

        public abstract PxVector Size { get; set; }
        public abstract void SetSize(int x, int y);
        public abstract void Clear(int x, int y);

        // DrawContext
        public abstract void Set();
    }
}

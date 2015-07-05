using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.Interop.Host;

namespace Xynapse.UI {
    public abstract class DrawableCanvas : Drawable, DrawContext {
        public static DrawableCanvas Get(int x, int y) {
            return HostInterop.Host.GetCanvas(x, y);
        }

        public abstract PxVector Size { get; set; }
        public abstract void SetSize(int x, int y);
        public abstract void Clear(DrawColor color);
        public void Clear() { Clear(new DrawColor(0f, 0f, 0f, 0f)); }

        // DrawContext
        public abstract void Set();
    }
}

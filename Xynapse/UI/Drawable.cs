using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public abstract class Drawable {
        public abstract void Draw(DrawContext context, PxRect rect, PxRect? sampleRect = null, DrawColor? color = null);
        public abstract void Draw(DrawContext context, FxVector position, FxVector? align = null, PxRect? sampleRect = null, DrawColor? color = null, float rotation = 0, FxVector? scale = null);
        public void Draw(DrawContext context, FxVector position, FxVector? align, PxRect? sampleRect, DrawColor? color, float rotation, float scale) {
            Draw(context, position, align, sampleRect, color, rotation, new FxVector(scale, scale));
        }
        //public abstract void SetAlign(float x, float y); // really? default align should just be a json attribute
    }
}

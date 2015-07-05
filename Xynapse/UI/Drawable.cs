using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public abstract class Drawable {
        private class NullDrawable : Drawable {
            public override void Draw(DrawContext context, PxRect rect, PxRect? sampleRect = null, DrawColor? color = null) { }
            public override void Draw(DrawContext context, FxVector position, FxVector? align = null, PxRect? sampleRect = null, DrawColor? color = null, float rotation = 0, FxVector? scale = null) { }
        }

        public static Drawable None = new NullDrawable();

        public abstract void Draw(DrawContext context, PxRect rect, PxRect? sampleRect = null, DrawColor? color = null);
        public abstract void Draw(DrawContext context, FxVector position, FxVector? align = null, PxRect? sampleRect = null, DrawColor? color = null, float rotation = 0, FxVector? scale = null);
        public void Draw(DrawContext context, FxVector position, FxVector? align, PxRect? sampleRect, DrawColor? color, float rotation, float scale) {
            Draw(context, position, align, sampleRect, color, rotation, new FxVector(scale, scale));
        }
        //public abstract void SetAlign(float x, float y); // really? default align should just be a json attribute

        public static DrawableStack operator +(Drawable left, Drawable right) { return new DrawableStack(left, right); }
        public static DrawableStack operator +(DrawableStack left, Drawable right) { return left.Copy.Add(right); }
        public static DrawableStack operator +(Drawable left, DrawableStack right) { return right.Copy.Insert(right, 0); }
    }
}

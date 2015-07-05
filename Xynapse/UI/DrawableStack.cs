using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public class DrawableStack : Drawable {
        List<Drawable> list;// = new List<Drawable>();

        public DrawableStack(params Drawable[] of) {
            list = new List<Drawable>(of);
        }

        public Drawable this[int i] {
            get { return list[i]; }
            set { if (i >= list.Count) list.Add(value); list[i] = value; }
        }

        public static DrawableStack operator +(DrawableStack left, DrawableStack right) {
            var d = left.Copy;
            d.list.AddRange(right.list);
            return d;
        }

        public DrawableStack Copy { get { var c = new DrawableStack(); c.list = new List<Drawable>(list); return c; } }
        public DrawableStack Add(Drawable d) { list.Add(d); return this; }
        public DrawableStack Insert(Drawable d, int index) { list.Insert(index, d); return this; }
        
        public override void Draw(DrawContext context, PxRect rect, PxRect? sampleRect = null, DrawColor? color = null) {
            foreach (Drawable d in list) d.Draw(context, rect, sampleRect, color);
        }

        public override void Draw(DrawContext context, FxVector position, FxVector? align = null, PxRect? sampleRect = null, DrawColor? color = null, float rotation = 0, FxVector? scale = null) {
            foreach (Drawable d in list) d.Draw(context, position, align, sampleRect, color, rotation, scale);
        }
    }
}

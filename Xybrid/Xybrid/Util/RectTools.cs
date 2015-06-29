using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xybrid.Util {
    public static class RectTools {
        // rect margins
        public static Rectangle MarginLeft(this Rectangle inp, int margin) {
            return new Rectangle(inp.X, inp.Y, margin, inp.Height);
        }
        public static Rectangle MarginRight(this Rectangle inp, int margin) {
            return new Rectangle((inp.X + inp.Width) - margin, inp.Y, margin, inp.Height);
        }
        public static Rectangle MarginTop(this Rectangle inp, int margin) {
            return new Rectangle(inp.X, inp.Y, inp.Width, margin);
        }
        public static Rectangle MarginBottom(this Rectangle inp, int margin) {
            return new Rectangle(inp.X, (inp.Y + inp.Height) - margin, inp.Width, margin);
        }

        // inflate replacement
        public static Rectangle Inflated(this Rectangle inp, int h, int v) {
            Rectangle outp = inp;
            outp.Inflate(h, v);
            return outp;
        }
    }
}

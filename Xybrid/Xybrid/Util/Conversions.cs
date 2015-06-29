using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Xynapse.UI;

using XPoint = Microsoft.Xna.Framework.Point;
using DPoint = System.Drawing.Point;

namespace Xybrid.Util {
    public static class Conversions {
        #region Point/PxVector
        public static PxVector PxVector(this XPoint pt) { return new PxVector(pt.X, pt.Y); }
        public static PxVector PxVector(this DPoint pt) { return new PxVector(pt.X, pt.Y); }
        public static XPoint XPoint(this PxVector pt) { return new XPoint(pt.X, pt.Y); }
        public static XPoint XPoint(this DPoint pt) { return new XPoint(pt.X, pt.Y); }
        public static DPoint DPoint(this PxVector pt) { return new DPoint(pt.X, pt.Y); }
        public static DPoint DPoint(this XPoint pt) { return new DPoint(pt.X, pt.Y); }
        #endregion

        #region Vector2/FxVector
        public static FxVector FxVector(this Vector2 vec) { return new FxVector(vec.X, vec.Y); }
        public static Vector2 Vector2(this FxVector vec) { return new Vector2(vec.X, vec.Y); }
        #endregion

        #region Rectangle/PxRect
        public static PxRect PxRect(this Rectangle rect) { return new PxRect(rect.X, rect.Y, rect.Width, rect.Height); }
        public static Rectangle Rectangle(this PxRect rect) { return new Rectangle(rect.X, rect.Y, rect.W, rect.H); }
        #endregion

        #region Color/DrawColor
        public static DrawColor DrawColor(this Color c) { return new DrawColor(c.R, c.G, c.B, c.A); }
        public static Color Color(this DrawColor c) { return new Color(c.R, c.G, c.B, c.A); }
        #endregion
    }
}

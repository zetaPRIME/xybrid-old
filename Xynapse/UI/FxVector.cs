using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public struct FxVector {
        public readonly float X;
        public readonly float Y;
        public FxVector(float x, float y) { X = x; Y = y; }

        public static FxVector operator +(FxVector left, PxVector right) { return new FxVector(left.X + (float)right.X, left.Y + (float)right.Y); }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public struct PxVector {
        public readonly int X, Y;
        public PxVector(int x, int y) { X = x; Y = y; }

        public static PxVector operator +(PxVector left, PxVector right) { return new PxVector(left.X + right.X, left.Y + right.Y); }
        public static PxVector operator -(PxVector left, PxVector right) { return new PxVector(left.X - right.X, left.Y - right.Y); }

        public PxVector ClampTo(PxRect rect) { return new PxVector(Math.Max(rect.X, Math.Min(X, rect.X + rect.W)), Math.Max(rect.Y, Math.Min(Y, rect.Y + rect.H))); }

        public override string ToString() { return "{ " + X + ", " + Y + " }"; }
    }
}

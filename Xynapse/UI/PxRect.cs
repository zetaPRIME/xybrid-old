using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public struct PxRect {
        public readonly int X, Y, W, H;
        public PxRect(int x, int y, int w, int h) { X = x; Y = y; W = w; H = h; }
        public PxRect(PxVector position, PxVector size) { X = position.X; Y = position.Y; W = size.X; H = size.Y; }

        public static PxRect MinMax(PxVector min, PxVector max) { return new PxRect(min.X, min.Y, max.X-min.X, max.Y-min.Y); }

        public PxVector Position { get { return new PxVector(X, Y); } }
        public PxVector Size { get { return new PxVector(W, H); } }
        public PxVector Min { get { return new PxVector(X, Y); } }
        public PxVector Max { get { return new PxVector(X+W, Y+H); } }

        public static PxRect operator +(PxRect rect, PxVector vec) { return new PxRect(rect.Position + vec, rect.Size); }
        public static PxRect operator -(PxRect rect, PxVector vec) { return new PxRect(rect.Position - vec, rect.Size); }

        public bool Intersects(PxRect that) { return this.X >= that.X + that.W && that.X >= this.X + this.W && this.Y >= that.Y + that.H && that.Y >= this.Y + this.H; }

        public override string ToString() { return "{ { " + X + ", " + Y + " }, { " + W +", " + H + " } }"; }
    }
}

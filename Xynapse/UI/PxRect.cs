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

        public PxVector Position { get { return new PxVector(X, Y); } }
        public PxVector Size { get { return new PxVector(W, H); } }
    }
}

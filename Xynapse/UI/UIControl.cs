using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.Util;

namespace Xynapse.UI {
    public abstract class UIControl {
        //public readonly DrawableCanvas canvas; // why is this here, seriously

        // parent, size, position (and property taking parent offset into account) getrect? implement rects
        internal UIContainer parent = null;
        public UIContainer Parent {
            get {
                return parent;
            }
            set {
                if (parent != null) parent.children.Remove(this);
                parent = value;
                if (parent != null) parent.children.Add(this);
            }
        }

        public int X, Y, W, H;
        public PxVector Position { get { return new PxVector(X, Y); } set { X = value.X; Y = value.Y; } }
        public PxVector Size { get { return new PxVector(W, H); } set { if (value.X == W && value.Y == H) return; W = value.X; H = value.Y; OnResize(); } } // might as well put onresize there
        //public PxRect Rect { get { return new PxRect(Position, Size); } set { Position = value.Position; Size = value.Size; } }
        public PxRect Rect { get { return new PxRect(X, Y, W, H); } set { X = value.X; Y = value.Y; W = value.W; H = value.H; } }

        public virtual PxRect ViewRect {
            get { if (parent == null) return Rect; return Rect + parent.ScrollOffset; }
            set { }
        }


        //EVENTS
        public virtual bool InterceptMouse(int x, int y) { return false; }
        public virtual bool OnScroll(int scroll) { return false; }

        public virtual void OnResize() { }

        public virtual void MouseEnter() { }
        public virtual void MouseLeave() { }
        public virtual void OnClick() { }
        public virtual void OnDoubleClick() { }
        public virtual void OnRightClick() { }
        public virtual void OnMiddleClick() { }

        public virtual void Update() { }
        public virtual void Draw() { }

    }
}

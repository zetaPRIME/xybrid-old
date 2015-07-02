﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.Input;
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
        public PxVector Size { get { return new PxVector(W, H); } set { if (value.X == W && value.Y == H) return; W = value.X; H = value.Y; OnResize(); if (parent != null) parent.OnChildResize(this); } } // might as well put onresize there
        //public PxRect Rect { get { return new PxRect(Position, Size); } set { Position = value.Position; Size = value.Size; } }
        public PxRect Rect { get { return new PxRect(X, Y, W, H); } set { X = value.X; Y = value.Y; W = value.W; H = value.H; } }

        public virtual PxRect ViewRect {
            get { if (parent == null) return Rect; return Rect + parent.ScrollOffset; }
            set { }
        }

        public virtual PxRect ScreenRect {
            get {
                if (parent == null) return Rect;
                return ViewRect + parent.ScreenRect.Position;
            }
            set { }
        }

        //EVENTS
        public virtual bool InterceptMouse(PxVector mousePos) { return false; }
        public virtual bool IsDraggable(int button) { return false; }
        public virtual bool OnScroll(int scroll) { return false; }

        public virtual void OnResize() { }

        public virtual void OnMouseEnter(InputState input) { }
        public virtual void OnMouseLeave(InputState input) { }
        public virtual void OnMouseDown(InputState input, int button) { }
        public virtual void OnMouseUp(InputState input, int button) { }
        public virtual void OnDrag(InputState input, int button, PxVector deltaFrame, PxVector deltaStart) { }
        /*public virtual void OnClick(InputState input) { } // pending rethinking
        public virtual void OnDoubleClick(InputState input) { }
        public virtual void OnRightClick(InputState input) { }
        public virtual void OnMiddleClick(InputState input) { }*/

        public virtual void Update() { }
        public virtual void Draw() { }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.Util;

namespace Xynapse.UI {
    public abstract class UIControl {
        public readonly DrawableCanvas canvas;

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

        


        //EVENTS
        public virtual bool InterceptMouse(int x, int y) { return false; }
        public virtual bool OnScroll(int scroll) { return false; }

        public virtual void MouseEnter() { }
        public virtual void MouseLeave() { }
        public virtual void OnClick() { }
        public virtual void OnDoubleClick() { }
        public virtual void OnRightClick() { }
        public virtual void OnMiddleClick() { }

    }
}

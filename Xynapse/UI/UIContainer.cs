using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.Util;

namespace Xynapse.UI {
    public abstract class UIContainer : UIControl, DrawContext {
        internal List<UIControl> children = new List<UIControl>();
        internal ListAccessor<UIControl> a_children = null;
        public ListAccessor<UIControl> Children {
            get {
                if (a_children == null) a_children = new ListAccessor<UIControl>(children);
                return a_children;
            }
        }
        public T AddChild<T>(T child) where T : UIControl {
            child.Parent = this;
            return child;
        }

        public abstract DrawableCanvas canvas { get; set; }

        private PxVector _ScrollOffset__BackingField;
        public virtual PxVector ScrollOffset {
            get { return _ScrollOffset__BackingField; }
            set { PxVector old = _ScrollOffset__BackingField; _ScrollOffset__BackingField = value.ClampTo(ScrollBounds); if (_ScrollOffset__BackingField != old) QueueRedraw(); }
        }
        public virtual PxRect ScrollBounds { get; set; }
        public PxRect ViewportRect { get { return new PxRect(ScrollOffset, Size); } set { } }

        #region advanced?
        public void Dive(Func<UIControl, bool> check, Func<UIControl, bool> intercept, Action<UIControl> perform) {
            bool done = false;
            _Dive(check, intercept, perform, ref done);
        }
        internal void _Dive(Func<UIControl, bool> check, Func<UIControl, bool> intercept, Action<UIControl> perform, ref bool done) {
            for (int i = children.Count - 1; i >= 0; i--) {
                if (check(children[i])) {
                    if (children[i] is UIContainer) {
                        (children[i] as UIContainer)._Dive(check, intercept, perform, ref done);
                        if (done) return;
                    }
                    else {
                        perform(children[i]);
                        if (intercept(children[i])) { done = true; return; }
                    }
                }
            }
            if (check(this)) {
                perform(this);
                if (intercept(this)) done = true;
            }
        }
        #endregion

        public override void Update() {
            UpdateChildren();
        }

        protected void UpdateChildren() {
            List<UIControl> chtemp = new List<UIControl>(children);
            foreach (UIControl child in chtemp) child.Update();
        }

        public override void Draw() {
            if (needsRedraw) {
                needsRedraw = false;
                canvas.Clear();
                DrawChildren();
            }
            if (parent != null) canvas.Draw(parent, ViewRect);
        }

        protected void DrawChildren() {
            List<UIControl> chtemp = new List<UIControl>(children);
            foreach (UIControl child in chtemp) if (child.Rect.Intersects(ViewportRect)) child.Draw();
        }

        public virtual void Set() { canvas.Set(); }
        public virtual void AddOffset(PxVector offset) { canvas.AddOffset(offset); }

        public virtual void OnChildResize(UIControl child) { }
    }
}

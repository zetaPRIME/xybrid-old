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
        //public void AddChild

        // hmm.
        public void Dive(Func<UIControl, bool> check, Action<UIControl> perform) {
            bool done = false;
            _Dive(check, perform, ref done);
        }
        internal void _Dive(Func<UIControl, bool> check, Action<UIControl> perform, ref bool done) {
            for (int i = children.Count - 1; i >= 0; i--) {
                if (children[i] is UIContainer) {
                    (children[i] as UIContainer)._Dive(check, perform, ref done);
                    if (done) return;
                }
                else {
                    perform(children[i]);
                    if (check(children[i])) { done = true; return; }
                }
            }
            perform(this);
            if (check(this)) done = true;
        }

        public override void Update() {
            List<UIControl> chtemp = new List<UIControl>(children);
            foreach (UIControl child in chtemp) child.Update();
        }

        public override void Draw() {
            List<UIControl> chtemp = new List<UIControl>(children);
            foreach (UIControl child in chtemp) child.Draw();
        }

        public abstract void Set(); // TBI
    }
}

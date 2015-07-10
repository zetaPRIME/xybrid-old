using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public class DrawableStates {
        public Dictionary<string, Drawable> dict;
        public DrawableStates() { dict = new Dictionary<string, Drawable>(); }

        public virtual Drawable this[string name] {
            get { Drawable output; if (!dict.TryGetValue(name, out output)) return Drawable.None; return output; }
            set {
                if (value == null) { if (dict.ContainsKey(name)) dict.Remove(name); }
                else if (dict.ContainsKey(name)) dict[name] = value;
                else dict.Add(name, value);
            }
        }

        public virtual DrawableStates Clear() { dict.Clear(); return this; }
        public virtual DrawableStates Add(string name, Drawable drawable) { dict.Add(name, drawable); return this; }
    }

    public class DrawableStatesProxy : DrawableStates {
        DrawableStates inner;
        public DrawableStatesProxy(DrawableStates link) { inner = link; }

        public void Set(DrawableStates link) { inner = link; }
        public DrawableStates Get() { return inner; }

        public override Drawable this[string name] { get { return inner[name]; } set { inner[name] = value; } }
        public override DrawableStates Clear() { inner.Clear(); return this; }
        public override DrawableStates Add(string name, Drawable drawable) { inner.Add(name, drawable); return this; }
    }
}

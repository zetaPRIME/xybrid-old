using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public class DrawableStateCollection {
        public Dictionary<string, Drawable> dict = new Dictionary<string, Drawable>();

        public Drawable this[string name] {
            get { if (!dict.ContainsKey(name)) return Drawable.None; return dict[name]; }
            set {
                if (value == null) { if (dict.ContainsKey(name)) dict.Remove(name); }
                else if (dict.ContainsKey(name)) dict[name] = value;
                else dict.Add(name, value);
            }
        }

        public void Clear() { dict.Clear(); }
    }
}

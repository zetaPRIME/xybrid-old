using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public enum TextAlign {
        Left, Right, Center
    }
    public enum TextVAlign {
        Top, Bottom, Middle
    }

    public abstract class TextDrawable : Drawable {
        public abstract string Text { get; set; }
        public virtual string Font { get { return "default"; } set { } }
        public abstract TextAlign Alignment { get; set; }
    }
}

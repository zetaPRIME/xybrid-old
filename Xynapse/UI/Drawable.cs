using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public abstract class Drawable {
        public abstract void Draw(DrawContext context);
        public abstract void SetAlign(float x, float y);
    }
}

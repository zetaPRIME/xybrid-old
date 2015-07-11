using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public class UIContainerFlat : UIContainer {
        public override DrawableCanvas canvas { get { return null; } set { } }
        public override void Set() {
            parent.Set();
            parent.AddOffset(ViewRect.Position);
        }
        public override void AddOffset(PxVector offset) { parent.AddOffset(offset); }

        public override void Draw() {
            //if (!needsRedraw) return;
            //needsRedraw = false;
            DrawChildren();
        }
    }
}

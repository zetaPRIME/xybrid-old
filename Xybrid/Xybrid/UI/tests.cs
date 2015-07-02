using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Xynapse.DataTypes;
using Xynapse.UI;
using Xynapse.Input;

using Xybrid.Graphics;
using Xybrid.Util;

namespace Xybrid.UI {
    public class testBtn : UIControl {
        public override bool InterceptMouse(PxVector mousePos) { return true; }
        public override void OnMouseEnter(InputState input) { hover = true; }
        public override void OnMouseLeave(InputState input) { hover = false; }
        bool hover = false;

        public override void Draw() {
            //PxVector mv = Mouse.GetState().Position.PxVector();
            //PxVector mv = System.Windows.Forms.Control.MousePosition.PxVector();
            //Debug.WriteLine(mv);

            Drawable d = null;
            if (hover) {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) d = ThemeManager.FetchDrawable("controls.button.default.press");
                else d = ThemeManager.FetchDrawable("controls.button.default.hover");
            }
            else d = ThemeManager.FetchDrawable("controls.button.default.idle");
            d.Draw(Parent, ViewRect);
        }

        public override bool IsDraggable(int button) { return button == 0; }
        public override void OnDrag(InputState input, int button, PxVector deltaFrame, PxVector deltaStart) {
            Position += deltaFrame;
        }
        public override bool OnScroll(int scroll) {
            Size += new PxVector(0, scroll * 4);
            return true;
        }
    }
}

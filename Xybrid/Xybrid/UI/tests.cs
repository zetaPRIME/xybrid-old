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
        public override void OnMouseEnter(InputState input) { hover = true; QueueRedraw(); }
        public override void OnMouseLeave(InputState input) { hover = false; QueueRedraw(); }
        bool hover = false;

        TextDrawable td = new TextDrawableFreetype();

        static SharpFont.Library lib = new SharpFont.Library();
        static FontDef fd = new FontDef(new SharpFont.Face(lib, "C:\\Windows\\Fonts\\segoeui.ttf"), 14);

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

            //td.Alignment = TextAlign.Center;
            //td.Draw(Parent, ViewRect);

            Parent.Set();
            DrawBatch.Draw(fd.Atlas, new FxVector(ViewRect.X + ViewRect.W / 2f, ViewRect.Y + ViewRect.H / 2f), null, fd.GetGlyph('W').PxRect(), null, 0, null);
            //DrawBatch.Draw(fd.atlas, new FxVector(0, 0), new FxVector(0, 0), null, null, 0, null);
        }

        public override bool IsDraggable(int button) { return button == 0; }
        public override void OnDrag(InputState input, int button, PxVector deltaFrame, PxVector deltaStart) {
            Position += deltaFrame;
        }
        public override bool OnScroll(int scroll) {
            Size += new PxVector(0, scroll * 4);
            td.Text = "Size: " + Size.Y;
            return true;
        }

        public override void OnMouseDown(InputState input, int button) {
            QueueRedraw();
        }
        public override void OnMouseUp(InputState input, int button) {
            Debug.WriteLine("blah blah up " + button);
            td.Text = "I'm le banana :D";
            QueueRedraw();
        }
    }
}

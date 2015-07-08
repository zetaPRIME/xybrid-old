using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

using Xynapse;
using Xynapse.UI;
using Xynapse.UI.Controls;
using Xynapse.DataTypes;

using Xybrid.Graphics;
using Xybrid.UI;

namespace Xybrid {
    public class WindowMain : WindowBase {
        public WindowMain() {
            testBtn blah = AddChild(new testBtn());
            blah.Rect = new PxRect(48, 48, 128, 32);

            AddChild(new testBtn()).Rect = new PxRect(48, 48, 128, 32);
            //AddChild(new testBtn()).Rect = new PxRect(48, 48, 1024, 1024);

            var b = AddChild(new Button());
            b.Rect = new PxRect(128, 128, 128, 32);
            b.Text = "Hello!";
            b.OnButtonClick(0, () => b.Position += new PxVector(4, 4))
                .OnButtonClick(1, () => b.Position += new PxVector(-4, 4))
                .OnButtonClick(2, () => {
                    DrawableStates st = (b.Background as DrawableStatesProxy).Get();
                    Drawable cc = st["idle"];
                    st["idle"] = st["hover"];
                    st["hover"] = st["press"];
                    st["press"] = cc;
                });
        }

        public override PxVector DefaultSize { get { return new PxVector(854, 480); } }
        
        public override string Title { get { return "Xybrid"; } }

        public override void Draw() {
            //ThemeManager.FetchDrawable("controls.button.default.idle").Draw(this, this.ViewportRect);

            DrawChildren();
        }

        public override bool OnClose() {
            System.Windows.Forms.Application.Exit();
            return true;
        }
    }
}

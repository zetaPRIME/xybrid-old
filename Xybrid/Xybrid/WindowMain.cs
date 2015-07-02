using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Xynapse;
using Xynapse.UI;
using Xynapse.DataTypes;

using Xybrid.Graphics;
using Xybrid.UI;

namespace Xybrid {
    public class WindowMain : WindowBase {
        public WindowMain() {
            testBtn blah = AddChild(new testBtn());
            blah.Rect = new PxRect(48, 48, 128, 32);

            AddChild(new testBtn()).Rect = new PxRect(48, 48, 128, 32);
        }

        public override string Title { get { return "Xybrid"; } }

        public override void Draw() {
            ThemeManager.FetchDrawable("controls.button.default.idle").Draw(this, this.ViewportRect);

            DrawChildren();
        }

        public override bool OnClose() {
            Application.Exit();
            return true;
        }
    }
}

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

namespace Xybrid {
    public class WindowMain : WindowBase {

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

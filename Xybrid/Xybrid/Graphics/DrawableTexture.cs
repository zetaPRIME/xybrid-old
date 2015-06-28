using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Xynapse.UI;

namespace Xybrid.Graphics {
    public class DrawableTexture : Drawable {
        Texture2D texture;
        public DrawableTexture(Texture2D texture) { this.texture = texture; }

        public override void Draw(DrawContext context, PxRect rect, PxRect? sampleRect = null, DrawColor? color = null) {
            throw new NotImplementedException();
        }

        public override void Draw(DrawContext context, FxVector position, FxVector? align = null, PxRect? sampleRect = null, DrawColor? color = null, float rotation = 0, FxVector? scale = null) {
            throw new NotImplementedException();
        }
    }
}

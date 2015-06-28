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

        public override void Draw(DrawContext context) {
            SpriteBatch sb = new SpriteBatch(UIHandler.graphicsDevice);
            //sb.Draw(texture, )
            
            throw new NotImplementedException();
        }

        public override void SetAlign(float x, float y) {
            throw new NotImplementedException();
        }
    }
}

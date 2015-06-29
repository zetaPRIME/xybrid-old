using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Xynapse.UI;

using Xybrid.Util;

namespace Xybrid.Graphics {
    public class Drawable9Patch : Drawable {
        Texture2D texture;
        int fw, fh;
        public Drawable9Patch(Texture2D texture, int fw, int fh) { this.texture = texture; this.fw = fw; this.fh = fh; }

        public override void Draw(DrawContext context, PxRect pxrect, PxRect? pxsampleRect = null, DrawColor? color = null) {
            context.Set();
            Rectangle rect = pxrect.Rectangle();
            Rectangle sampleRect;
            if (pxsampleRect == null) sampleRect = texture.Bounds;
            else sampleRect = pxsampleRect.Value.Rectangle();

            // top left
            DrawBatch.Draw(texture, rect.MarginTop(fh).MarginLeft(fw).PxRect(), sampleRect.MarginTop(fh).MarginLeft(fw).PxRect(), color);
            // top
            DrawBatch.Draw(texture, rect.MarginTop(fh).Inflated(-fw, 0).PxRect(), sampleRect.MarginTop(fh).Inflated(-fw, 0).PxRect(), color);
            // top right
            DrawBatch.Draw(texture, rect.MarginTop(fh).MarginRight(fw).PxRect(), sampleRect.MarginTop(fh).MarginRight(fw).PxRect(), color);
            // left
            DrawBatch.Draw(texture, rect.Inflated(0, -fh).MarginLeft(fw).PxRect(), sampleRect.Inflated(0, -fh).MarginLeft(fw).PxRect(), color);
            // center
            DrawBatch.Draw(texture, rect.Inflated(-fw, -fh).PxRect(), sampleRect.Inflated(-fw, -fh).PxRect(), color);
            // right
            DrawBatch.Draw(texture, rect.Inflated(0, -fh).MarginRight(fw).PxRect(), sampleRect.Inflated(0, -fh).MarginRight(fw).PxRect(), color);
            // bottom left
            DrawBatch.Draw(texture, rect.MarginBottom(fh).MarginLeft(fw).PxRect(), sampleRect.MarginBottom(fh).MarginLeft(fw).PxRect(), color);
            // bottom
            DrawBatch.Draw(texture, rect.MarginBottom(fh).Inflated(-fw, 0).PxRect(), sampleRect.MarginBottom(fh).Inflated(-fw, 0).PxRect(), color);
            // bottom right
            DrawBatch.Draw(texture, rect.MarginBottom(fh).MarginRight(fw).PxRect(), sampleRect.MarginBottom(fh).MarginRight(fw).PxRect(), color);
            
        }

        public override void Draw(DrawContext context, FxVector position, FxVector? align = null, PxRect? sampleRect = null, DrawColor? color = null, float rotation = 0, FxVector? scale = null) {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Xynapse.UI;

namespace Xybrid.Graphics {
    public class Canvas : DrawableCanvas {
        internal RenderTarget2D target;

        public override PxVector Size {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        internal Canvas(RenderTarget2D tgt) { target = tgt; }
        public Canvas(int x, int y) {
            target = new RenderTarget2D(GraphicsManager.device, x, y);
        }

        public override void SetSize(int x, int y) {
            target = new RenderTarget2D(GraphicsManager.device, x, y);
        }

        public override void Clear() {
            DrawBatch.Clear(target, Color.Transparent);
        }

        public override void Set() {
            DrawBatch.Target = target;
        }

        public override void Draw(DrawContext context, PxRect rect, PxRect? sampleRect = null, DrawColor? color = null) {
            throw new NotImplementedException();
        }

        public override void Draw(DrawContext context, FxVector position, FxVector? align = null, PxRect? sampleRect = null, DrawColor? color = null, float rotation = 0, FxVector? scale = null) {
            throw new NotImplementedException();
        }
    }
}

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
    public class Canvas : DrawableCanvas {
        internal RenderTarget2D target;

        public override PxVector Size {
            get { return new PxVector(target.Width, target.Height); }
            set { SetSize(value.X, value.Y); }
        }

        internal Canvas(RenderTarget2D tgt) { target = tgt; }
        public Canvas(int x, int y) {
            target = new RenderTarget2D(GraphicsManager.device, x, y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

        public override void SetSize(int x, int y) {
            if (x == target.Width && y == target.Height) return; // don't thrash gpu
            target.Dispose(); // this made things faster in UIForm, probably works much the same
            target = new RenderTarget2D(GraphicsManager.device, x, y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

        public override void Clear(DrawColor color) {
            DrawBatch.Clear(target, color.Color());
        }

        public override void Set() {
            DrawBatch.Target = target;
            DrawBatch.drawOffset = new PxVector(0, 0);
        }
        public override void AddOffset(PxVector offset) { DrawBatch.drawOffset += offset; }

        public override void Draw(DrawContext context, PxRect rect, PxRect? sampleRect = null, DrawColor? color = null) {
            throw new NotImplementedException();
        }

        public override void Draw(DrawContext context, FxVector position, FxVector? align = null, PxRect? sampleRect = null, DrawColor? color = null, float rotation = 0, FxVector? scale = null) {
            throw new NotImplementedException();
        }
    }
}

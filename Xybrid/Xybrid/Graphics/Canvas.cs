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

        public Canvas(int x, int y) {
            target = new RenderTarget2D(UIHandler.graphicsDevice, x, y);
        }

        public override void SetSize(int x, int y) {
            throw new NotImplementedException();
        }

        public override void Clear(int x, int y) {
            throw new NotImplementedException();
        }

        public override void Set() {
            throw new NotImplementedException();
        }

        public override void Draw(DrawContext context) {
            throw new NotImplementedException();
        }

        public override void SetAlign(float x, float y) {
            throw new NotImplementedException();
        }
    }
}

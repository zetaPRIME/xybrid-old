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
    public static class DrawBatch {
        private static RenderTarget2D target = null;
        public static RenderTarget2D Target { get { return target; } set { if (value == target) return; GraphicsManager.device.SetRenderTarget(target); sb.End(); sb.Begin(); target = value; } }
        internal static SpriteBatch sb = null;
        public static PxVector drawOffset = new PxVector(0, 0);

        public static void Init() {
            sb = new SpriteBatch(GraphicsManager.device);
            sb.Begin();
        }

        public static void Clear(RenderTarget2D tgt, Color color) {
            GraphicsManager.device.SetRenderTarget(tgt);
            GraphicsManager.device.Clear(color);
        }
        public static void Clear(RenderTarget2D tgt, DrawColor color) { Clear(tgt, color.Color()); } // take Xynapse color

        public static void Draw(Texture2D texture, PxRect rect, PxRect? sampleRect, DrawColor? color) {
            sb.Draw(texture, (rect + drawOffset).Rectangle(), (sampleRect == null) ? null : (Rectangle?)sampleRect.Value.Rectangle(), (color == null) ? Color.White : color.Value.Color());
        }

        public static void Draw(Texture2D texture, FxVector position, FxVector? align, PxRect? sampleRect, DrawColor? color, float rotation, FxVector? scale) {
            Vector2 origin = new Vector2(texture.Width, texture.Height) * align.GetValueOrDefault(new FxVector(0.5f, 0.5f)).Vector2();
            sb.Draw(texture, (position + drawOffset).Vector2(), (sampleRect == null) ? null : (Rectangle?)sampleRect.Value.Rectangle(), (color == null) ? Color.White : color.Value.Color(), rotation, origin, scale.GetValueOrDefault(new FxVector(1, 1)).Vector2(), SpriteEffects.None, 0);
        }

        static List<RenderTarget2D> stackTgt = new List<RenderTarget2D>();
        static List<PxVector> stackOffset = new List<PxVector>();

        public static void Push() { stackTgt.Add(target); stackOffset.Add(drawOffset); }
        public static void PushSwap(RenderTarget2D target, PxVector offset) { Push(); Target = target; drawOffset = offset; }
        public static void PushSwap(RenderTarget2D target) { PushSwap(target, new PxVector(0, 0)); }
        public static void Pop() {
            if (stackTgt.Count == 0) return;
            Target = stackTgt.Last();
            drawOffset = stackOffset.Last();
            stackTgt.RemoveAt(stackTgt.Count - 1);
            stackOffset.RemoveAt(stackOffset.Count - 1);
        }
    }
}

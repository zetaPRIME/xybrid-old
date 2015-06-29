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
    public static class GraphicsManager {
        public static GraphicsDevice device;

        static BlendState blendColor, blendAlpha;

        public static void Init() {
            // set up blendstates for premultiplying
            blendColor = new BlendState();
            blendColor.ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue;

            blendColor.AlphaDestinationBlend = Blend.Zero;
            blendColor.ColorDestinationBlend = Blend.Zero;

            blendColor.AlphaSourceBlend = Blend.SourceAlpha;
            blendColor.ColorSourceBlend = Blend.SourceAlpha;

            blendAlpha = new BlendState();
            blendAlpha.ColorWriteChannels = ColorWriteChannels.Alpha;

            blendAlpha.AlphaDestinationBlend = Blend.Zero;
            blendAlpha.ColorDestinationBlend = Blend.Zero;

            blendAlpha.AlphaSourceBlend = Blend.One;
            blendAlpha.ColorSourceBlend = Blend.One;

            DrawBatch.Init();
        }

        public static Texture2D ConvertToPreMultipliedAlphaGPU(Texture2D texture) {
            // code borrowed from http://jakepoz.com/jake_poznanski__speeding_up_xna.html

            // Set up a render target to hold our final texture which will have premulitplied alpha values
            RenderTarget2D result = new RenderTarget2D(device, texture.Width, texture.Height);

            device.SetRenderTarget(result);
            device.Clear(Color.Black);

            // Multiply each color by the source alpha, and write in just the color values into the final texture
            SpriteBatch spriteBatch = new SpriteBatch(device);
            spriteBatch.Begin(SpriteSortMode.Immediate, blendColor);
            spriteBatch.Draw(texture, texture.Bounds, Color.White);
            spriteBatch.End();

            // Now copy over the alpha values from the PNG source texture to the final one, without multiplying them
            spriteBatch.Begin(SpriteSortMode.Immediate, blendAlpha);
            spriteBatch.Draw(texture, texture.Bounds, Color.White);
            spriteBatch.End();

            // Release the GPU back to drawing to the screen
            device.SetRenderTarget(null);

            return result as Texture2D;
        }
    }
}

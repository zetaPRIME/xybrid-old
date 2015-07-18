using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SharpFont;

using Xynapse.UI;

using Xybrid.Util;

namespace Xybrid.Graphics {
    public class TextDrawableFreetype : TextDrawable {
        public static Library ftLib = new Library();

        string text = "";
        string font = "default";
        TextAlign align = TextAlign.Left;
        bool dirty = true;

        Texture2D texture = null;

        public TextDrawableFreetype(string font = "default", string text = "", TextAlign align = TextAlign.Left) {
            this.text = text; this.font = font; this.align = align;
        }

        void BuildTexture() {
            FontDef font = ThemeManager.FetchFont(this.font);

            PxVector tsize = font.MeasureLine(text);
            if (texture != null) texture.Dispose();

            if (tsize.X == 0 || tsize.Y == 0) {
                texture = new Texture2D(GraphicsManager.device, 1, 1);
                return;
            }
            RenderTarget2D rt = new RenderTarget2D(GraphicsManager.device, tsize.X, tsize.Y, false, SurfaceFormat.Color, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);
            
            RenderTarget2D pop = DrawBatch.Target;
            PxVector popOff = DrawBatch.drawOffset;

            DrawBatch.Target = rt;
            DrawBatch.drawOffset = new PxVector(0, 0);
            DrawBatch.Clear(rt, new DrawColor(0f, 0f, 0f, 0f));
            
            float cursor = 0;
            for (int i = 0; i < text.Length; i++) {
                Vector2 cpos = new Vector2((float)Math.Round(cursor), 0); // pixel align on draw, but retain float accumulation
                DrawBatch.Draw(font.Atlas, cpos.FxVector(), new FxVector(0, 0), font.GetGlyph(text[i]).PxRect(), Microsoft.Xna.Framework.Color.White.DrawColor(), 0f, new FxVector(1, 1));
                cursor += font.MeasureGlyph(text, i);
            }

            DrawBatch.Target = null;
            DrawBatch.Target = pop;
            DrawBatch.drawOffset = popOff;

            texture = rt;
        }

        void BuildTextureOld() {
            #region attribution
            // Significant parts of this method were based off the following example:
            // https://github.com/Robmaister/SharpFont/blob/master/Source/Examples/Program.cs
            // The following license and attribution applies to those parts:

            /*Copyright (c) 2012 Robert Rouhani <robert.rouhani@gmail.com>
            Permission is hereby granted, free of charge, to any person obtaining a copy of
            this software and associated documentation files (the "Software"), to deal in
            the Software without restriction, including without limitation the rights to
            use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
            of the Software, and to permit persons to whom the Software is furnished to do
            so, subject to the following conditions:
            The above copyright notice and this permission notice shall be included in all
            copies or substantial portions of the Software.
            THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
            IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
            FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
            AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
            LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
            OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
            SOFTWARE.*/
            #endregion

            Face face = new Face(ftLib, "C:\\Windows\\Fonts\\segoeui.ttf");
            face.SetCharSize(0, 14, 72, 72);
            //face.SetPixelSizes(0, 14);

            const LoadFlags loadFlags = LoadFlags.Default;
            const LoadTarget loadTarget = LoadTarget.Light;
            const RenderMode renderMode = RenderMode.Light;

            float cx = 0, cy = 0, tw = 0, th = 0;

            // base size
            face.LoadGlyph(face.GetCharIndex('A'), loadFlags, loadTarget);
            th = (float)face.Glyph.Metrics.Height;

            for (int i = 0; i < text.Length; i++) {
                char c = text[i];
                uint cnum = face.GetCharIndex(c);
                face.LoadGlyph(cnum, loadFlags, loadTarget);
                tw += (float)face.Glyph.Advance.X;
                if (face.HasKerning && i < text.Length - 1)  tw += (float)face.GetKerning(cnum, face.GetCharIndex(text[i + 1]), KerningMode.Default).X;
                th = Math.Max(th, (float)face.Glyph.Metrics.Height);
                
            }
            if (tw == 0) tw = th;
            
            Bitmap bmp = new Bitmap((int)Math.Ceiling(tw), (int)Math.Ceiling(th*2)); // assumption that any downstem is at most half the height of a capital letter
            cy = th * 0.5f; // adjustment for such
            //Bitmap bmp = new Bitmap(128, 32);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            g.Clear(System.Drawing.Color.Transparent);
            //g.Clear(System.Drawing.Color.DimGray);

            for (int i = 0; i < text.Length; i++) {
                char c = text[i];
                uint cnum = face.GetCharIndex(c);
                face.LoadGlyph(cnum, loadFlags, loadTarget);
                face.Glyph.RenderGlyph(renderMode);
                if (c == ' ') {
                    cx += (float)face.Glyph.Advance.X;
                    // omitted an if statement that didn't seem to do anything relevant??
                    cy += (float)face.Glyph.Advance.Y;
                    continue;
                }

                FTBitmap ftb = face.Glyph.Bitmap; // lol ftb
                Bitmap cb = ftb.ToGdipBitmap(System.Drawing.Color.White);
                g.DrawImageUnscaled(cb, (int)Math.Round(cx + face.Glyph.BitmapLeft), (int)Math.Round(cy + (Math.Ceiling(th) - face.Glyph.BitmapTop))); // th and not bitmap.height
                cx += (float)face.Glyph.Metrics.HorizontalAdvance;
                cy += (float)face.Glyph.Advance.Y;
                if (face.HasKerning && i < text.Length - 1) cx += (float)face.GetKerning(cnum, face.GetCharIndex(text[i + 1]), KerningMode.Default).X;
            }
            g.Dispose();
            if (texture != null) texture.Dispose();
            //texture = GraphicsManager.ConvertToPreMultipliedAlphaGPU(GetTexture(GraphicsManager.device, bmp));
            texture = GetTexture(GraphicsManager.device, bmp);
        }

        private Texture2D GetTexture(GraphicsDevice dev, System.Drawing.Bitmap bmp) { // borrowed from http://stackoverflow.com/a/7394185
            int[] imgData = new int[bmp.Width * bmp.Height];
            Texture2D texture = new Texture2D(dev, bmp.Width, bmp.Height);

            unsafe {
                // lock bitmap
                System.Drawing.Imaging.BitmapData origdata =
                    bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

                uint* byteData = (uint*)origdata.Scan0;

                // Switch bgra -> rgba
                for (int i = 0; i < imgData.Length; i++) {
                    byteData[i] = (byteData[i] & 0x000000ff) << 16 | (byteData[i] & 0x0000FF00) | (byteData[i] & 0x00FF0000) >> 16 | (byteData[i] & 0xFF000000);
                }

                // copy data
                System.Runtime.InteropServices.Marshal.Copy(origdata.Scan0, imgData, 0, bmp.Width * bmp.Height);

                byteData = null;

                // unlock bitmap
                bmp.UnlockBits(origdata);
            }

            texture.SetData(imgData);

            return texture;
        }

        public override string Text { get { return text; } set { if (value == text) return; dirty = true; text = value; } }
        public override string Font { get { return font; } set { if (value == font) return; dirty = true; font = value; } }
        //public override TextAlign Alignment { get { return align; } set { if (value == align) return; dirty = true; align = value; } }
        public override TextAlign Alignment { get { return align; } set { align = value; } }
        
        public override void Draw(DrawContext context, PxRect rect, PxRect? sampleRect = null, DrawColor? color = null) {
            if (dirty) { BuildTexture(); dirty = false; }
            context.Set();

            int hoff = 0;
            int voff = (rect.H / 2) - (texture.Bounds.Height / 2);
            if (align == TextAlign.Right) hoff = rect.W - texture.Bounds.Width;
            else if (align == TextAlign.Center) hoff = (rect.W / 2) - (texture.Bounds.Width / 2);

            DrawBatch.Draw(texture, new PxRect(rect.Position + new PxVector(hoff, voff), texture.Bounds.Size.PxVector()), sampleRect, color);
            //DrawBatch.Draw(texture, rect, sampleRect, color);
        }

        public override void Draw(DrawContext context, FxVector position, FxVector? align = null, PxRect? sampleRect = null, DrawColor? color = null, float rotation = 0, FxVector? scale = null) {
            throw new NotImplementedException();
        }
    }
}

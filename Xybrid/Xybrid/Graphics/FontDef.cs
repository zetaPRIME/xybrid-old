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

using Rectangle = Microsoft.Xna.Framework.Rectangle;
using DRect = System.Drawing.Rectangle;
using Point = Microsoft.Xna.Framework.Point;
using DPoint = System.Drawing.Point;

namespace Xybrid.Graphics {
    public class FontDef {
        Face fontFace;
        int pxSize;

        int lineSize;
        
        RenderTarget2D atlas;

        Dictionary<char, Rectangle> charRects = new Dictionary<char, Rectangle>();

        int lineOffset = 0;
        int lineNum = 0;

        const LoadFlags loadFlags = LoadFlags.Default;
        const LoadTarget loadTarget = LoadTarget.Light;
        const RenderMode renderMode = RenderMode.Light;

        public FontDef(Face face, int size) {
            const int atlasSize = 1024;
            atlas = new RenderTarget2D(GraphicsManager.device, atlasSize, atlasSize, false, SurfaceFormat.Color, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);
            fontFace = face;
            pxSize = size;

            face.SetPixelSizes((uint)size, (uint)size);
            face.LoadGlyph(face.GetCharIndex('A'), loadFlags, loadTarget);
            lineSize = (int)Math.Ceiling((float)face.Glyph.Metrics.Height * 2f);

            const string defaultCharset = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPRSTUVWXYZ!@#$%^&*()`-=~_+[]{}\\|;',./:\"<>? ";
            foreach (char c in defaultCharset) AddGlyph(c);
        }

        void AddGlyph(char c) {
            const int buffer = 2;

            fontFace.LoadGlyph(fontFace.GetCharIndex(c), loadFlags, loadTarget);
            fontFace.Glyph.RenderGlyph(renderMode);
            int w = (int)Math.Ceiling((float)fontFace.Glyph.Metrics.HorizontalAdvance);
            int h = (int)Math.Ceiling((float)fontFace.Glyph.Metrics.Height);

            if (lineOffset + w + buffer > atlas.Width) {
                lineOffset = 0;
                lineNum++;
            }
            if ((lineNum + 1) * lineSize > atlas.Height) throw new Exception("Font atlas overflow");

            Rectangle rect = new Rectangle(lineOffset, lineNum * lineSize, w, lineSize);
            charRects.Add(c, rect);
            lineOffset += w + buffer;

            if (w <= 0 || h <= 0) return;

            // and draw
            Bitmap bmp = new Bitmap(w, lineSize);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            g.Clear(System.Drawing.Color.Transparent);

            
            FTBitmap ftb = fontFace.Glyph.Bitmap; // lol ftb
            Bitmap cb = ftb.ToGdipBitmap(System.Drawing.Color.White);
            //g.DrawImageUnscaled(cb, (int)Math.Round((float)fontFace.Glyph.BitmapLeft), (int)Math.Round((lineSize * 0.25 + h) - (float)fontFace.Glyph.BitmapTop)); // th and not bitmap.height
            g.DrawImageUnscaled(cb, (int)Math.Round((float)fontFace.Glyph.BitmapLeft), (int)Math.Round(lineSize * 0.75 - (float)fontFace.Glyph.BitmapTop));

            g.Dispose();
            Texture2D ch = GetTexture(GraphicsManager.device, bmp);
            bmp.Dispose();

            RenderTarget2D pop = DrawBatch.Target;
            PxVector popOff = DrawBatch.drawOffset;

            DrawBatch.Target = atlas;
            DrawBatch.Draw(ch, rect.PxRect(), null, null);
            DrawBatch.Target = null;
            DrawBatch.Target = pop;
            DrawBatch.drawOffset = popOff;

            ch.Dispose();
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

        public Texture2D Atlas { get { return atlas; } }
        public Rectangle GetGlyph(char c) {
            Rectangle res;
            if (charRects.TryGetValue(c, out res)) return res;
            AddGlyph(c);
            return charRects[c];
        }

        

        public PxVector MeasureLine(string line) {
            float length = 0;

            for (int i = 0; i < line.Length; i++) {
                length += MeasureGlyph(line, i);
            }

            return new PxVector((int)Math.Ceiling(length), lineSize);
        }

        public float MeasureGlyph(string line, int index) {
            if (index < line.Length - 1) return MeasureGlyph(line[index], line[index + 1]);
            return MeasureGlyph(line[index]);
        }
        public float MeasureGlyph(char c, char next = ' ') {
            uint cnum = fontFace.GetCharIndex(c);
            fontFace.LoadGlyph(cnum, loadFlags, loadTarget);
            float width = (float)fontFace.Glyph.Advance.X;
            
            if (!fontFace.HasKerning) return width;
            
            uint cnumn = fontFace.GetCharIndex(next);
            width += (float)fontFace.GetKerning(cnum, cnumn, KerningMode.Default).X;

            return width;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ionic.Zip;

using Xynapse.UI;

namespace Xybrid.Graphics {
    public static class ThemeManager {
        static Dictionary<string, Drawable> assetsDrawableDefault = new Dictionary<string, Drawable>();
        internal static void LoadDefault() {
            assetsDrawableDefault.Clear();

            using (Stream zs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Xybrid.Resources.DefaultTheme.zip")) {
                ZipFile zip = ZipFile.Read(zs);
                foreach (ZipEntry entry in zip) {
                    if (!entry.FileName.ToLower().EndsWith(".png")) continue;
                    string name = entry.FileName.Substring(0, entry.FileName.Length - 4);
                    Texture2D tex = null;
                    using (var ms = new MemoryStream()) {
                        entry.Extract(ms);
                        ms.Position = 0;
                        tex = Texture2D.FromStream(UIHandler.graphicsDevice, ms);
                    }
                    tex = GraphicsManager.ConvertToPreMultipliedAlphaGPU(tex);

                    assetsDrawableDefault.Add(name.Replace('/', '.'), new DrawableTexture(tex));

                }
            }
        }

        public static Drawable FetchDrawable(string name) {
            if (assetsDrawableDefault.ContainsKey(name)) return assetsDrawableDefault[name];
            return null;
        }
    }
}

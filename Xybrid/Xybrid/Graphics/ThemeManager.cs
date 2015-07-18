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

using LitJson;

using Xynapse.UI;
using Xynapse.UI.Controls;

namespace Xybrid.Graphics {
    public static class ThemeManager {
        static SharpFont.Library fontLibrary = new SharpFont.Library();

        static Dictionary<string, Drawable> assetsDrawableDefault = new Dictionary<string, Drawable>();
        static Dictionary<string, FontDef> assetsFontsDefault = new Dictionary<string, FontDef>();
        internal static void LoadDefault() {
            assetsDrawableDefault.Clear();
            assetsFontsDefault.Clear();

            using (Stream zs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Xybrid.Resources.DefaultTheme.zip")) {
                ZipFile zip = ZipFile.Read(zs);
                foreach (ZipEntry entry in zip) {
                    string fname = entry.FileName.ToLower();
                    if (fname.EndsWith(".png")) {
                        Texture2D tex = null;
                        using (var ms = new MemoryStream()) {
                            entry.Extract(ms);
                            ms.Position = 0;
                            tex = Texture2D.FromStream(GraphicsManager.device, ms);
                        }
                        tex = GraphicsManager.ConvertToPreMultipliedAlphaGPU(tex);

                        string name;// = entry.FileName.Substring(0, entry.FileName.Length - 4);
                        Drawable d = ToDrawable(entry.FileName, tex, out name);
                        assetsDrawableDefault.Add(name, d);

                        //assetsDrawableDefault.Add(name.Replace('/', '.'), new DrawableTexture(tex));
                    }
                    else if (fname.EndsWith(".json")) {
                        if (fname.StartsWith("fonts/")) {
                            string name;
                            FontDef font = ToFontDef(entry.FileName, zip, out name);
                            assetsFontsDefault.Add(name, font);
                        }
                    }
                }
            }
        }

        static Drawable ToDrawable(string fileName, Texture2D tex, out string name) {
            name = fileName.Substring(0, fileName.LastIndexOf("."));
            string tag = "";
            while (true) { // lol hacky control block
                int ts = name.LastIndexOf('['); if (ts < 0) break;
                int te = name.LastIndexOf(']'); if (te < 0) break;
                tag = name.Substring(ts+1, (te - ts) - 1);
                name = name.Substring(0, ts).TrimEnd();
                break;
            }

            name = name.Replace('/', '.');

            if (tag != "") {
                string[] token = tag.Split(' ');
                if (token[0] == "9patch") {
                    try {
                        int fw = int.Parse(token[1]);
                        int fh = int.Parse(token[2]);
                        return new Drawable9Patch(tex, fw, fh);
                    }
                    catch { }
                }
            }
            return new DrawableTexture(tex);
        }

        static FontDef ToFontDef(string fileName, ZipFile archive, out string name) {
            JsonData json;
            using (MemoryStream ms = new MemoryStream()) {
                archive[fileName].Extract(ms);
                json = JsonMapper.ToObject(Encoding.UTF8.GetString(ms.ToArray()));
            }

            string fontFile = (string)json["file"];
            int size = (int)json["size"];

            FontDef font;
            // find font file
            using (MemoryStream ms = new MemoryStream()) {
                archive["fonts/" + fontFile].Extract(ms);
                SharpFont.Face face = new SharpFont.Face(fontLibrary, ms.ToArray(), 0);
                font = new FontDef(face, size);
            }

            name = fileName.Substring("fonts/".Length);
            name = name.Substring(0, name.Length - 5);
            name = name.Replace('/', '.');
            return font;
        }

        public static Drawable FetchDrawable(string name) {
            Drawable output = null;
            //if (assetsDrawableTheme.TryGetValue(name, out output)) return output;
            if (assetsDrawableDefault.TryGetValue(name, out output)) return output;
            return null;
        }

        public static FontDef FetchFont(string name) {
            FontDef output = null;
            //if (assetsFontsTheme.TryGetValue(name, out output)) return output;
            if (assetsFontsDefault.TryGetValue(name, out output)) return output;
            if (name == "default") throw new Exception("Default font missing");
            return FetchFont("default");
        }

        #region Caching etc.
        static Dictionary<string, DrawableStates> controlStates = new Dictionary<string, DrawableStates>();
        static void BuildControlStates(DrawableStatesProxy proxy, string control, string subtype, params string[] assets) {
            DrawableStates states = new DrawableStates();
            foreach (string state in assets) {
                Drawable d = FetchDrawable("controls." + control + "." + subtype + "." + state);
                if (d == null) d = FetchDrawable("controls." + control + ".default." + state);
                if (d == null) d = Drawable.None;
                states[state] = d;
            }
            proxy.Set(states);
        }
        public static DrawableStates FetchControlStates(string control, string subtype, params string[] assets) {
            string fullName = control + "." + subtype;
            if (controlStates.ContainsKey(fullName)) return controlStates[fullName];
            DrawableStatesProxy proxy = new DrawableStatesProxy(null);
            BuildControlStates(proxy, control, subtype, assets);
            controlStates.Add(fullName, proxy);
            return proxy;
        }
        #endregion

        #region Control theming and related
        public static void SetDefaults(UIControl control, string subtype) {
            { var c = control as Button; if (c != null) { SetDefaults(c, subtype); return; } }
        }
        public static void SetDefaults(Button btn, string subtype) {
            btn.Label = new TextDrawableFreetype();
            btn.Label.Alignment = TextAlign.Center;
            btn.Background = FetchControlStates("button", subtype, "idle", "hover", "press");
        }
        #endregion
    }
}

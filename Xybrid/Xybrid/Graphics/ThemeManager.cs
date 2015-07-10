﻿using System;
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
using Xynapse.UI.Controls;

namespace Xybrid.Graphics {
    public static class ThemeManager {
        static Dictionary<string, Drawable> assetsDrawableDefault = new Dictionary<string, Drawable>();
        internal static void LoadDefault() {
            assetsDrawableDefault.Clear();

            using (Stream zs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Xybrid.Resources.DefaultTheme.zip")) {
                ZipFile zip = ZipFile.Read(zs);
                foreach (ZipEntry entry in zip) {
                    if (!entry.FileName.ToLower().EndsWith(".png")) continue;
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

        public static Drawable FetchDrawable(string name) {
            Drawable output = null;
            //if (assetsDrawableTheme.TryGetValue(name, out output)) return output;
            if (assetsDrawableDefault.TryGetValue(name, out output)) return output;
            return null;
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

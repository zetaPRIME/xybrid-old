using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Keys = Microsoft.Xna.Framework.Input.Keys;
using MonoGame.Framework;

namespace Xybrid {
    public class UIHandler : Game {

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point lpPoint);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        public static IntPtr GetWindowUnderCursor() {
            Point ptCursor = new Point();

            if (!(/*PInvoke.*/GetCursorPos(out ptCursor)))
                return IntPtr.Zero;

            return WindowFromPoint(ptCursor);
        }

        public SpriteBatch spriteBatch;
        public GraphicsDeviceManager graphics;

        UIForm form;
        public int num = 0;

        public MyGame(UIForm f) {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content/Native";
            form = f;
        }

        protected override void LoadContent() {
            Debug.WriteLine("loadcontent " + num);
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            IsMouseVisible = true;

            Panel pan = new Panel();
            pan.SetBounds(32, 32, 32, 32);
            form.Controls.Add(pan);
        }

        protected override void Draw(GameTime gameTime) {
            Debug.WriteLine("draw " + num + " window " + form.Handle);
            WindowsDeviceConfig.ControlToUse = form;

            MouseState ms = Mouse.GetState(Window);
            spriteBatch.GraphicsDevice.Clear(new Color((float)ms.Position.X / Window.ClientBounds.Width, 0f, 0f));
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A)) spriteBatch.GraphicsDevice.Clear(new Color(0f, 1f, 0f));
            if (GetWindowUnderCursor() == form.Handle) {
                spriteBatch.GraphicsDevice.Clear(Color.Blue);
                Debug.WriteLine("cleared " + num + ", " );
            }


            base.Draw(gameTime);
        }
    }
}

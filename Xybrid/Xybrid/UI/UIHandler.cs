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

using Xynapse.UI;

using Xybrid.Graphics;
using Xybrid.Util;

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

        public static UIHandler instance;
        public static ContainerControl control;

        public static SpriteBatch spriteBatch;
        public GraphicsDeviceManager graphics;

        public static UIForm currentForm;
        public int num = 0;

        private UIHandler() {
            instance = this;

            graphics = new GraphicsDeviceManager(this);
        }

        public static void Init() {
            if (instance != null) return;
            control = new ContainerControl();
            control.CreateControl();

            WindowsDeviceConfig.UseForm = false;
            WindowsDeviceConfig.ControlToUse = control;
            new UIHandler();

            instance.IsFixedTimeStep = false;
            instance.RunOneFrame();
        }

        public static void RunFrame(UIForm form) {
            currentForm = form;
            instance.RunOneFrame();
        }

        protected override void LoadContent() {
            GraphicsManager.device = GraphicsDevice;
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            IsMouseVisible = true;
        }

        internal void PreAllUpdate() {

        }
        
        protected override void Update(GameTime gameTime) {
            if (currentForm == null) return;

            currentForm.windowDef.Draw();

            /*MouseState ms = Mouse.GetState();
            bool lm = ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            if (lm && (currentForm.mpoint != null || GetWindowUnderCursor() == currentForm.Handle)) {
                if (currentForm.mpoint != null) {
                    Point old = currentForm.Location.XPoint();
                    currentForm.Location = new System.Drawing.Point(old.X + (ms.Position.X - currentForm.mpoint.Value.X), old.Y + (ms.Position.Y - currentForm.mpoint.Value.Y));
                }
                currentForm.mpoint = ms.Position;
            }
            else currentForm.mpoint = null;*/
        }

        protected override bool BeginDraw() {
            if (currentForm == null) return false;
            GraphicsDevice.SetRenderTarget(currentForm.target);
            return true;
        }

        bool ainit = false;
        static RenderTarget2D blah;

        protected override void Draw(GameTime gameTime) {
            currentForm.windowDef.Draw();
            /*if (!ainit) {
                ainit = true;

                blah = new RenderTarget2D(GraphicsDevice, 1, 1);
                GraphicsDevice.SetRenderTarget(blah);
                GraphicsDevice.Clear(Color.White);

                
                
                return;
            }

            MouseState ms = Mouse.GetState(Window);
            System.Drawing.Point mp = currentForm.PointToClient(new System.Drawing.Point(ms.X, ms.Y));
            spriteBatch.GraphicsDevice.Clear(new Color((float)mp.X / currentForm.ClientRectangle.Width, 0f, 0f));
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A)) spriteBatch.GraphicsDevice.Clear(new Color(0f, 1f, 0f));
            if (GetWindowUnderCursor() == currentForm.Handle) spriteBatch.GraphicsDevice.Clear(Color.Blue);
            
            spriteBatch.Begin();
            spriteBatch.Draw(blah, new Rectangle(88, 88, 88, 88), new Color(127, 0, 255));
            spriteBatch.End();

            Canvas c = new Canvas(currentForm.target);
            Drawable d = ThemeManager.FetchDrawable("controls.button.default.press");
            d.Draw(c, new Xynapse.UI.PxRect(4, 4, 128, 32));
            d.Draw(c, new Xynapse.UI.PxRect(4, 48, 12, 12));
            
            
            DrawBatch.Target = null;
            //base.Draw(gameTime);*/
        }

        protected override void EndDraw() {
            DrawBatch.Target = null;
            if (currentForm != null) currentForm.target.Present();
        }
    }
}

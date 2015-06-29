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
            //Debug.WriteLine("loadcontent " + num);
            GraphicsManager.device = GraphicsDevice;
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            IsMouseVisible = true;

            //Panel pan = new Panel();
            //pan.SetBounds(32, 32, 32, 32);
            //form.Controls.Add(pan);
        }

        protected override bool BeginDraw() {
            if (currentForm == null) return true;
            GraphicsDevice.SetRenderTarget(currentForm.target);
            System.Drawing.Rectangle r = currentForm.ClientRectangle;
            control.Location = new System.Drawing.Point(r.X, r.Y);
            control.Size = new System.Drawing.Size(r.Width, r.Height);
            return true;
        }

        //Point? mpoint = null;
        protected override void Update(GameTime gameTime) {
            if (!ainit) return;

            MouseState ms = Mouse.GetState();
            bool lm = ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            if (lm && (currentForm.mpoint != null || GetWindowUnderCursor() == currentForm.Handle)) {
                if (currentForm.mpoint != null) {
                    Point old = currentForm.Location.XPoint();
                    currentForm.Location = new System.Drawing.Point(old.X + (ms.Position.X - currentForm.mpoint.Value.X), old.Y + (ms.Position.Y - currentForm.mpoint.Value.Y));
                }
                currentForm.mpoint = ms.Position;
            }
            else currentForm.mpoint = null;
        }

        bool ainit = false;
        static RenderTarget2D blah;

        protected override void Draw(GameTime gameTime) {
            if (!ainit) {
                ainit = true;

                blah = new RenderTarget2D(GraphicsDevice, 1, 1);
                GraphicsDevice.SetRenderTarget(blah);
                GraphicsDevice.Clear(Color.White);

                
                
                return;
            }

            //Debug.WriteLine("drawing " + currentForm.Handle);
            //GraphicsDevice.SetRenderTarget(currentForm.target);

            //Debug.WriteLine("draw " + num + " window " + form.Handle);
            //WindowsDeviceConfig.ControlToUse = form;

            MouseState ms = Mouse.GetState(Window);
            System.Drawing.Point mp = currentForm.PointToClient(new System.Drawing.Point(ms.X, ms.Y));
            spriteBatch.GraphicsDevice.Clear(new Color((float)mp.X / currentForm.ClientRectangle.Width, 0f, 0f));
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A)) spriteBatch.GraphicsDevice.Clear(new Color(0f, 1f, 0f));
            if (GetWindowUnderCursor() == currentForm.Handle) {
                spriteBatch.GraphicsDevice.Clear(Color.Blue);
                //Debug.WriteLine("cleared " + num + ", " );
            }
            spriteBatch.Begin();
            spriteBatch.Draw(blah, new Rectangle(88, 88, 88, 88), new Color(127, 0, 255));
            spriteBatch.End();

            //ThemeManager.FetchDrawable("pickle.TestImage").Draw(new Canvas(currentForm.target), new Xynapse.UI.FxVector(32, 32));
            ThemeManager.FetchDrawable("pickle.TestImage2").Draw(new Canvas(currentForm.target), new Xynapse.UI.PxRect(4, 4, 128, 128));
            
            DrawBatch.Target = null;
            //base.Draw(gameTime);
        }

        protected override void EndDraw() {
            if (currentForm != null) currentForm.target.Present();
            //GraphicsDevice.Present();
        }
    }
}

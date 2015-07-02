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
using Xynapse.Input;

using Xybrid.Graphics;
using Xybrid.Util;

using XKeys = Xynapse.Input.Keys;

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

        public InputState inputState = new InputState();
        IntPtr mouseOverWindow;
        internal static int scrollValue = 0;
        bool[] mouseButtonsLastFrame = new bool[3];
        bool[] mouseButtonsThisFrame = new bool[3];
        Keys[] keysLastFrame = new Keys[] { };
        Keys[] keysThisFrame = new Keys[] { };
        

        internal void PreAllUpdate() {
            inputState.MousePosition = Control.MousePosition.PxVector();
            mouseOverWindow = GetWindowUnderCursor();

            MouseState ms = Mouse.GetState();
            
            inputState.scrollWheel = scrollValue;
            scrollValue = 0;

            for (int i = 0; i < 3; i++) mouseButtonsLastFrame[i] = mouseButtonsThisFrame[i];
            mouseButtonsThisFrame[0] = ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            mouseButtonsThisFrame[1] = ms.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            mouseButtonsThisFrame[2] = ms.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;

            for (int i = 0; i < 3; i++) {
                if (inputState.mouseLastPressed[i] < InputState.MaxTrack) inputState.mouseLastPressed[i]++;
                if (inputState.mouseLastReleased[i] < InputState.MaxTrack) inputState.mouseLastReleased[i]++;

                if (mouseButtonsThisFrame[i] && !mouseButtonsLastFrame[i]) inputState.mouseLastPressed[i] = 0;
                if (mouseButtonsLastFrame[i] && !mouseButtonsThisFrame[i]) inputState.mouseLastReleased[i] = 0;
            }

            foreach (KeyValuePair<XKeys, int> kvp in inputState.lastPressed.ToList()) if (kvp.Value < InputState.MaxTrack) inputState.lastPressed[kvp.Key]++;
            foreach (KeyValuePair<XKeys, int> kvp in inputState.lastReleased.ToList()) if (kvp.Value < InputState.MaxTrack) inputState.lastReleased[kvp.Key]++;

            keysLastFrame = keysThisFrame;
            keysThisFrame = Keyboard.GetState().GetPressedKeys();

            foreach (Keys k in keysThisFrame) if (!keysLastFrame.Contains(k)) inputState.lastPressed[(XKeys)k] = 0;
            foreach (Keys k in keysLastFrame) if (!keysThisFrame.Contains(k)) inputState.lastReleased[(XKeys)k] = 0;
        }
        
        protected override void Update(GameTime gameTime) {
            if (currentForm == null) return;

            currentForm.ProcessInputEvents(inputState, currentForm.Handle == mouseOverWindow);

            currentForm.windowDef.Update();

            string title = currentForm.windowDef.Title;
            if (currentForm.Text != title) currentForm.Text = title;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.UI;

namespace Xynapse.Input {
    public enum InputModifier {
        None = 0,
        Ctrl = 1,
        Alt = 2,
        Shift = 4,
        
        CtrlShift = 5,
        CtrlAlt = 3,
        AltShift = 6,
        CtrlAltShift = 7
    }

    public class InputState {
        public const int MaxTrack = 60 * 60;

        public PxVector MousePosition = new PxVector();
        public int scrollWheel = 0;

        public int[] mouseLastPressed = new int[] { MaxTrack, MaxTrack, MaxTrack };
        public int[] mouseLastReleased = new int[] { MaxTrack, MaxTrack, MaxTrack };

        public bool MousePressed(int button) { return mouseLastPressed[button] == 0; }
        public bool MouseDown(int button) { return mouseLastPressed[button] < mouseLastReleased[button]; }
        public bool MouseReleased(int button) { return mouseLastReleased[button] == 0; }

        public Dictionary<Keys, int> lastPressed = new Dictionary<Keys, int>();
        public Dictionary<Keys, int> lastReleased = new Dictionary<Keys, int>();

        public bool KeyPressed(Keys key) { return lastPressed.ContainsKey(key) && lastPressed[key] == 0; }
        public bool KeyDown(Keys key) {
            if (!lastPressed.ContainsKey(key)) return false;
            int lr = MaxTrack;
            if (lastReleased.ContainsKey(key)) lr = lastReleased[key];
            return lastPressed[key] < lr;
        }
        public bool KeyReleased(Keys key) { return lastReleased.ContainsKey(key) && lastReleased[key] == 0; }

        public bool Ctrl { get { return KeyDown(Keys.LeftControl) || KeyDown(Keys.RightControl); } }
        public bool Alt { get { return KeyDown(Keys.LeftAlt) || KeyDown(Keys.RightAlt); } }
        public bool Shift { get { return KeyDown(Keys.LeftShift) || KeyDown(Keys.RightShift); } }

        public InputModifier Modifier {
            get {
                int res = 0;
                if (Ctrl) res += 1;
                if (Alt) res += 2;
                if (Shift) res += 4;
                return (InputModifier)res;
            }
        }
    }
}

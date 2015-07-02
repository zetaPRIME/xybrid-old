using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.UI;

namespace Xynapse.Input {
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
    }
}

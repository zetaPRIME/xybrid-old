using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public struct DrawColor {
        public readonly float R, G, B, A;

        public DrawColor(float r, float g, float b, float a) { R = r; G = g; B = b; A = a; }
        public DrawColor(float r, float g, float b) { R = r; G = g; B = b; A = 1; }
        public DrawColor(byte r, byte g, byte b, byte a) { R = (float)r / 255; G = (float)g / 255; B = (float)b / 255; A = (float)a / 255; }
        public DrawColor(byte r, byte g, byte b) { R = (float)r / 255; G = (float)g / 255; B = (float)b / 255; A = 1; }
    }
}

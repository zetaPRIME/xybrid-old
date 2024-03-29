﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.Input;

namespace Xynapse.UI.Controls {
    public class Button : UIControl {
        public DrawableStates Background { get; set; }
        public TextDrawable Label { get; set; }
        public bool[] showPressOnButton = { true, false, false };

        string subtype;

        public Button(string subtype = "default") {
            Interop.Host.HostInterop.Host.SetDefaults(this, subtype);
            this.subtype = subtype;
        }

        public string Text { get { return Label.Text; } set { if (value == Label.Text) return; Label.Text = value; QueueRedraw(); } }

        public bool isHovered = false;
        public bool isPressed = false;
        int dragKey = -1;

        public override void Draw() {
            string state = isHovered ? isPressed ? "press" : "hover" : "idle";
            Background[state].Draw(parent, ViewRect);
            Label.Draw(parent, ViewRect);
        }

        public override bool InterceptMouse(PxVector mousePos) { return true; }
        public override void OnMouseEnter(InputState input) { isHovered = true; QueueRedraw(); }
        public override void OnMouseLeave(InputState input) { isHovered = false; QueueRedraw(); }

        public override bool IsDraggable(int button) { return clickAction[button] != null; } // I guess there's no point in registering it if it doesn't lead anywhere
        public override void OnMouseDown(InputState input, int button) {
            if (IsDraggable(button)) {
                dragKey = button;
                isPressed = showPressOnButton[button];
                QueueRedraw();
            }
        }
        public override void OnMouseUp(InputState input, int button) {
            if (button == dragKey) {
                dragKey = -1;
                isPressed = false;

                if (isHovered && clickAction[button] != null) clickAction[button]();
                QueueRedraw();
            }
        }
        public override void OnDrag(InputState input, int button, PxVector deltaFrame, PxVector deltaStart) {
            isHovered = ScreenRect.Contains(input.MousePosition);
            QueueRedraw();
        }

        // action
        public Action[] clickAction = new Action[3];
        public Button OnButtonClick(int button, Action action) { clickAction[button] = action; return this; }
        public Button OnButtonClick(Action action) { return OnButtonClick(0, action); }
    }
}

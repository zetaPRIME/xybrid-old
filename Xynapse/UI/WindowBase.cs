using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.Interop.Host;

namespace Xynapse.UI {
    public class WindowBase : UIContainerFlat {
        //protected DrawableCanvas _canvas;
        //public override DrawableCanvas canvas { get { return _canvas; } set { _canvas = value; } }

        public WindowFrame Frame { get { return parent as WindowFrame; } }

        public virtual PxVector MinimumSize { get; set; }
        public virtual PxVector MaximumSize { get; set; }

        public virtual PxVector DefaultSize { get { return new PxVector(160, 160); } }

        public virtual string Title { get; set; }

        public WindowBase() {
            MinimumSize = new PxVector(160, 160);
            MaximumSize = new PxVector(99999, 99999);
        }

        public void Open() { HostInterop.Host.OpenWindow(this); }

        #region Events
        public virtual bool OnClose() { return false; }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xynapse.Interop.Host;

namespace Xynapse.UI {
    public class WindowBase : UIContainer {
        protected DrawableCanvas _canvas;
        public override DrawableCanvas canvas { get { return _canvas; } set { _canvas = value; } }

        public PxVector MinimumSize { get; set; }
        public PxVector MaximumSize { get; set; }

        public PxVector DefaultSize { get { return new PxVector(160, 160); } }

        public virtual string Title { get; set; }

        public WindowBase() {
            MinimumSize = new PxVector(160, 160);
            MaximumSize = new PxVector(99999, 99999);
        }

        public void Open() { Xynapse.Interop.Host.HostInterop.Host.OpenWindow(this); }

        #region Events
        public virtual bool OnClose() { return false; }
        #endregion
    }
}

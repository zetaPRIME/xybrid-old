using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public abstract class WindowFrame : UIContainer {

        public virtual WindowBase Window { get { return children[0] as WindowBase; } }
        //
        public virtual PxVector ClientToWindowPos(PxVector inp) { return inp; }
        public virtual PxVector WindowToClientPos(PxVector inp) { return inp; }
        public virtual PxVector ClientToWindowSize(PxVector inp) { return inp; }
        public virtual PxVector WindowToClientSize(PxVector inp) { return inp; }

        public abstract bool HasTitleBar { get; set; }
        public abstract bool ManuallyResizable { get; set; }
    }
}

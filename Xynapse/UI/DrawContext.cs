using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.UI {
    public interface DrawContext {
        void Set();
        void AddOffset(PxVector offset);
    }
}

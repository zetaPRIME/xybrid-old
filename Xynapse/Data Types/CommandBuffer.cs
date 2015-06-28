using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.DataTypes {
    public class CommandBuffer {
        public readonly List<ICommand> commands = new List<ICommand>();
        public ICommand Pop() {
            if (commands.Count == 0) return null;
            ICommand cmd = commands[0];
            commands.RemoveAt(0);
            return cmd;
        }
        public void Add(ICommand cmd) { commands.Add(cmd); }
        public void Add(CommandBuffer buffer) { commands.AddRange(buffer.commands); }
        public void Add(IEnumerable<ICommand> list) { commands.AddRange(list); }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.DataTypes {
    public enum CommandType : byte {
        Note,
        NoteOff,

    }
    public interface ICommand {
        CommandType type{get;}
    }
    public struct CommandNote : ICommand {
        public CommandType type { get { return CommandType.Note; } }
        public readonly int noteId;
        public readonly bool startNew;
        public readonly float? note;
        public readonly float? volume;
        public readonly float? pan;
    }
    public struct CommandNoteOff : ICommand {
        public CommandType type { get { return CommandType.Note; } }
        public readonly int noteId;
        public readonly bool cut;
    }
}

using System.Collections.Generic;
using CodeEditor.DataStructures;

namespace CodeEditor.Views.TextView {
    internal class LinesRemovalInfo {
        public IEnumerable<int> LinesToRemove { get; set; }
        public IEnumerable<KeyValuePair<TextPosition, string>> LinesAffected { get; set; }
    }
}

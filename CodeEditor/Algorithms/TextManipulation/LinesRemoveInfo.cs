using System.Collections.Generic;
using CodeEditor.DataStructures;

namespace CodeEditor.Algorithms.TextManipulation {
    internal class LinesRemovalInfo {
        public IEnumerable<int> LinesToRemove { get; set; }
        public IEnumerable<KeyValuePair<TextPosition, string>> LinesToChange { get; set; }
    }
}

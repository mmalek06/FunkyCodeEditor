using System.Collections.Generic;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Algorithms.TextManipulation {
    internal class ChangeInLinesInfo {
        public IReadOnlyCollection<int> LinesToRemove { get; set; }
        public IReadOnlyCollection<KeyValuePair<TextPosition, string>> LinesToChange { get; set; }
    }
}

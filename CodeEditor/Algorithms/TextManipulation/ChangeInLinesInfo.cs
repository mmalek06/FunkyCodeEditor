using System.Collections.Generic;
using CodeEditor.Core.DataStructures;
using CodeEditor.Visuals;

namespace CodeEditor.Algorithms.TextManipulation {
    internal class ChangeInLinesInfo {
        public IReadOnlyCollection<int> LinesToRemove { get; set; }
        public IReadOnlyCollection<KeyValuePair<TextPosition, VisualTextLine>> LinesToChange { get; set; }
    }
}

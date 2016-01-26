using System.Collections.Generic;
using TextEditor.DataStructures;

namespace TextEditor.Views.TextView {
    internal class LinesRemovalInfo {
        public int[] LinesToRemove { get; set; }
        public IEnumerable<KeyValuePair<TextPosition, string>> LinesAffected { get; set; }
    }
}

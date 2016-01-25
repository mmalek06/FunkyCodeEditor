using System.Collections.Generic;

namespace TextEditor.Commands {
    internal class ViewState {

        #region public properties

        public int ActiveLineIndex { get; set; } = -1;

        public int ActiveColumnIndex { get; set; } = -1;

        public int LineCount { get; set; } = 0;

        public Dictionary<int, string> LineStates { get; set; } = new Dictionary<int, string>();

        #endregion

    }
}

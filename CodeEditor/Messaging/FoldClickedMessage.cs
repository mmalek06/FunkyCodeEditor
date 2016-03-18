using CodeEditor.Algorithms.Folding;

namespace CodeEditor.Messaging {
    internal class FoldClickedMessage {

        #region properties

        public FoldingStates State { get; set; }

        public int Line { get; set; }

        #endregion

    }
}

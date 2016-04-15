using CodeEditor.Algorithms.Folding;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Messaging {
    internal class FoldClickedMessage {

        #region properties

        public FoldingStates State { get; set; }

        public TextRange Area { get; set; }

        public string OpeningTag { get; set; }

        public string ClosingTag { get; set; }

        #endregion

    }
}

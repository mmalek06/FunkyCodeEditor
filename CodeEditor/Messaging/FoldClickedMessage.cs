using CodeEditor.DataStructures;
using CodeEditor.Enums;

namespace CodeEditor.Messaging {
    public class FoldClickedMessage {

        #region properties

        public FoldingStates State { get; set; }

        public TextRange AreaBeforeFolding { get; set; }

        public TextRange AreaAfterFolding { get; set; }

        public string OpeningTag { get; set; }

        public string ClosingTag { get; set; }

        #endregion

    }
}

using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Enums;

namespace CodeEditor.Core.Messaging {
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

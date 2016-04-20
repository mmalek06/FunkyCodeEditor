using CodeEditor.Core.DataStructures;

namespace CodeEditor.Views.Caret {
    internal partial class CaretView : ICaretViewReader {

        #region properties

        public TextPosition CaretPosition { get; private set; }

        #endregion

    }
}

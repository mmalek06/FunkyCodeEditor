using CodeEditor.Core.DataStructures;

namespace CodeEditor.Views.Caret {
    internal interface ICaretViewReader {

        #region properties

        TextPosition CaretPosition { get; }

        #endregion

    }
}

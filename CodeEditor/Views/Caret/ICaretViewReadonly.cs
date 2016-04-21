using System.Windows.Input;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Views.Caret {
    internal interface ICaretViewReadonly {

        #region properties

        TextPosition CaretPosition { get; }

        #endregion

        #region public properties

        TextPosition GetNextPosition(Key key);

        #endregion

    }
}

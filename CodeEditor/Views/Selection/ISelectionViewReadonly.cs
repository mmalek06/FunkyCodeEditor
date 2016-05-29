using CodeEditor.DataStructures;

namespace CodeEditor.Views.Selection {
    internal interface ISelectionViewReadonly {

        #region public methods

        TextRange GetCurrentSelectionArea();

        bool HasSelection();

        #endregion

    }
}

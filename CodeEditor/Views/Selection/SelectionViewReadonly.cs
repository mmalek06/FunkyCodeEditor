using CodeEditor.Core.DataStructures;

namespace CodeEditor.Views.Selection {
    internal partial class SelectionView : ISelectionViewReadonly {

        #region public methods

        public TextRange GetCurrentSelectionArea() {
            if (lastSelectionStart == null || lastSelectionEnd == null) {
                return null;
            }

            return new TextRange {
                StartPosition = lastSelectionStart <= lastSelectionEnd ? lastSelectionStart : lastSelectionEnd,
                EndPosition = lastSelectionStart <= lastSelectionEnd ? lastSelectionEnd : lastSelectionStart
            };
        }

        public bool HasSelection() => lastSelectionStart != null && lastSelectionEnd != null && isSelecting;

        #endregion

    }
}

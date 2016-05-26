using System.Windows.Input;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Views.Caret {
    internal partial class CaretView : ICaretViewReadonly {

        #region properties

        public TextPosition CaretPosition { get; private set; }

        #endregion

        #region public methods

        public TextPosition GetNextPosition(Key key) {
            TextPosition coordinates;
            int column = 0;
            int line = 0;

            if (IsStep(key)) {
                coordinates = GetStep(key);
                column = CaretPosition.Column + coordinates.Column;
                line = CaretPosition.Line + coordinates.Line;
            } else {
                coordinates = GetJump(key);
                column = coordinates.Column;
                line = coordinates.Line;

                if (coordinates.Column == int.MinValue) {
                    column = 0;
                }
            }

            return new TextPosition(column: column, line: line);
        }

        #endregion

        #region methods

        private bool IsCaretInbetweenTags(TextRange range) =>
            CaretPosition >= range.StartPosition && CaretPosition <= range.EndPosition;

        private bool IsFoldMultiline(TextRange range) =>
            range.StartPosition.Line != range.EndPosition.Line;

        #endregion

    }
}

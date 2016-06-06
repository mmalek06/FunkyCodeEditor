using System.Windows.Input;
using CodeEditor.Configuration;
using CodeEditor.Extensions;
using CodeEditor.DataStructures;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;
using CodeEditor.Visuals.Base;

namespace CodeEditor.Algorithms.Selection {
    internal class TextSelectionAlgorithm {

        #region fields

        private ITextViewReadonly textViewReader;

        private ICaretViewReadonly caretViewReader;

        private SelectionView parent;
        
        #endregion

        #region constructor

        public TextSelectionAlgorithm(ICaretViewReadonly caretViewReader, ITextViewReadonly textViewReader, SelectionView parent) {
            this.caretViewReader = caretViewReader;
            this.textViewReader = textViewReader;
            this.parent = parent;
        }

        #endregion

        #region public methods

        public TextPosition StandardSelection(MouseEventArgs mouseEvent) => mouseEvent.GetPosition(parent).GetDocumentPosition(TextConfiguration.GetCharSize());

        public SelectionInfo WordSelection(KeyEventArgs keyboardEvent) {
            var activeLine = textViewReader.GetLine(caretViewReader.CaretPosition.Line);

            return null;
        }

        public SelectionInfo WordSelection(MouseButtonEventArgs mouseEvent) {
            var clickPosition = mouseEvent.GetPosition(parent).GetDocumentPosition(TextConfiguration.GetCharSize());
            var activeLine = textViewReader.GetVisualLine(clickPosition.Line);
            var lineLength = textViewReader.GetLineLength(clickPosition.Line);
            int[] selectionRange;

            if (!activeLine.GetCharInfoAt(clickPosition.Column).IsCharacter) {
                selectionRange = GetCollapseSelectionRange(clickPosition, activeLine, lineLength);
            } else {
                selectionRange = GetStandardWordSelectionRange(clickPosition, activeLine, lineLength);
            }

            return new SelectionInfo {
                StartPosition = new TextPosition(column: selectionRange[0], line: clickPosition.Line),
                EndPosition = new TextPosition(column: selectionRange[1], line: clickPosition.Line),
                CursorPosition = new TextPosition(column: selectionRange[1], line: clickPosition.Line)
            };
        }

        public SelectionInfo LineSelection(MouseButtonEventArgs mouseEvent) {
            var clickPosition = mouseEvent.GetPosition(parent).GetDocumentPosition(TextConfiguration.GetCharSize());
            var startPosition = new TextPosition(column: 0, line: clickPosition.Line);
            var endPosition = new TextPosition(column: textViewReader.GetLineLength(clickPosition.Line), line: clickPosition.Line);
            var cursorColumn = clickPosition.Line + 1 < textViewReader.LinesCount ? 0 : textViewReader.GetLineLength(clickPosition.Line);
            var cursorLine = clickPosition.Line + 1 < textViewReader.LinesCount ? clickPosition.Line + 1 : clickPosition.Line;

            return new SelectionInfo {
                StartPosition = startPosition,
                EndPosition = endPosition,
                CursorPosition = new TextPosition(column: cursorColumn, line: cursorLine)
            };
        }

        public TextPosition GetSelectionPosition(KeyEventArgs keyboardEvent) {
            TextPosition endingPosition = null;

            switch (keyboardEvent.Key) {
                case Key.Left:
                    endingPosition = new TextPosition(column: caretViewReader.CaretPosition.Column - 1, line: caretViewReader.CaretPosition.Line);
                    break;
                case Key.Home:
                    endingPosition = new TextPosition(column: 0, line: caretViewReader.CaretPosition.Line);
                    break;
                case Key.Right:
                    endingPosition = new TextPosition(column: caretViewReader.CaretPosition.Column + 1, line: caretViewReader.CaretPosition.Line);
                    break;
                case Key.End:
                    endingPosition = new TextPosition(column: textViewReader.GetLineLength(caretViewReader.CaretPosition.Line), line: caretViewReader.CaretPosition.Line);
                    break;
                case Key.Up:
                    endingPosition = new TextPosition(column: caretViewReader.CaretPosition.Column, line: caretViewReader.CaretPosition.Line - 1);
                    break;
                case Key.PageUp:
                    endingPosition = new TextPosition(column: caretViewReader.CaretPosition.Column, line: caretViewReader.CaretPosition.Line - GlobalConstants.PageSize);
                    break;
                case Key.Down:
                    endingPosition = new TextPosition(column: caretViewReader.CaretPosition.Column, line: caretViewReader.CaretPosition.Line + 1);
                    break;
                case Key.PageDown:
                    endingPosition = new TextPosition(column: caretViewReader.CaretPosition.Column, line: caretViewReader.CaretPosition.Line + GlobalConstants.PageSize);
                    break;
            }

            return endingPosition;
        }

        #endregion

        #region methods

        private int[] GetStandardWordSelectionRange(TextPosition clickPosition, VisualTextLine activeLine, int lineLength) {
            var startColumn = clickPosition.Column;
            var endColumn = clickPosition.Column;

            if (endColumn + 1 <= lineLength) {
                endColumn += 1;
            } else {
                startColumn -= 1;
            }
            // move left from current position
            for (var i = clickPosition.Column; i >= 0; i--) {
                var charInfo = activeLine.GetCharInfoAt(i);

                if (charInfo.IsCharacter && char.IsLetterOrDigit(charInfo.Text[0])) {
                    startColumn = i;
                } else {
                    break;
                }
            }
            // move right from current position
            for (var i = clickPosition.Column; i < lineLength; i++) {
                var charInfo = activeLine.GetCharInfoAt(i);

                if (charInfo.IsCharacter && char.IsLetterOrDigit(charInfo.Text[0])) {
                    endColumn = i + 1;
                } else {
                    break;
                }
            }

            return new[] { startColumn, endColumn };
        }

        private int[] GetCollapseSelectionRange(TextPosition clickPosition, VisualTextLine activeLine, int lineLength) {
            var startColumn = clickPosition.Column;
            var endColumn = clickPosition.Column;

            if (endColumn + 1 <= lineLength) {
                endColumn += 1;
            } else {
                startColumn -= 1;
            }
            // move left from current position
            for (var i = clickPosition.Column; i >= 0; i--) {
                var charInfo = activeLine.GetCharInfoAt(i);

                if (!charInfo.IsCharacter && !char.IsLetterOrDigit(charInfo.Text[0])) {
                    startColumn = i;
                } else {
                    break;
                }
            }
            // move right from current position
            for (var i = clickPosition.Column; i < lineLength; i++) {
                var charInfo = activeLine.GetCharInfoAt(i);

                if (!charInfo.IsCharacter && !char.IsLetterOrDigit(charInfo.Text[0])) {
                    endColumn = i + 1;
                } else {
                    break;
                }
            }

            return new[] { startColumn, endColumn };
        }

        #endregion

    }
}

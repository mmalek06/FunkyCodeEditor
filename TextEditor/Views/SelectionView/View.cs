using System.Collections.Generic;
using System.Windows.Input;
using TextEditor.DataStructures;
using TextEditor.Extensions;
using LocalTextInfo = TextEditor.Views.TextView.TextInfo;

namespace TextEditor.Views.SelectionView {
    internal class View : ViewBase {

        #region fields

        private TextPosition lastSelectionStart;
        private TextPosition lastSelectionEnd;
        private LocalTextInfo textInfo;
        private bool isSelecting;

        #endregion

        #region constructor

        public View(LocalTextInfo textInfo) : base() {
            this.textInfo = textInfo;
            isSelecting = false;
        }

        #endregion

        #region public methods

        public void Select(TextPosition position) {
            var selection = new VisualElement();

            visuals.Clear();
            SetSelectionState(position);
            selection.Draw(GetSelectionPoints(lastSelectionStart, lastSelectionEnd));
            visuals.Add(selection);
        }

        public void Deselect() {
            lastSelectionStart = null;
            lastSelectionEnd = null;
            isSelecting = false;

            visuals.Clear();
        }

        public TextPositionsPair GetCurrentSelectionArea() {
            if (lastSelectionStart == null || lastSelectionEnd == null) {
                return null;
            }

            return new TextPositionsPair {
                StartPosition = lastSelectionStart,
                EndPosition = lastSelectionEnd
            };
        }

        public bool HasSelection() => lastSelectionStart != null && lastSelectionEnd != null && isSelecting;

        #endregion

        #region event handlers

        public void HandleMouseDown(MouseButtonEventArgs e) {
            Deselect();
        }

        #endregion

        #region methods

        private void SetSelectionState(TextPosition position) {
            if (!isSelecting) {
                isSelecting = true;

                lastSelectionStart = position;
                lastSelectionEnd = lastSelectionStart;
            } else {
                lastSelectionEnd = position;
            }
        }

        private IEnumerable<PointsPair> GetSelectionPoints(TextPosition start, TextPosition end) {
            var pairs = new List<PointsPair>();
            var realStart = start <= end ? start : end;
            var realEnd = start <= end ? end : start;

            visuals.Clear();

            for (int i = realStart.Line; i <= realEnd.Line; i++) {
                TextPosition tmpStartPos = new TextPosition { Column = 0, Line = i };
                TextPosition tmpEndPos = new TextPosition { Column = 0, Line = i };

                if (i == realStart.Line) {
                    tmpStartPos.Column = realStart.Column;
                }

                int lineLen = textInfo.GetTextLineLength(i);

                if (i == realEnd.Line) {
                    tmpEndPos.Column = realEnd.Column > lineLen ? lineLen : realEnd.Column;
                } else {
                    tmpEndPos.Column = lineLen;
                }

                pairs.Add(new PointsPair {
                    StartingPoint = tmpStartPos.GetPositionRelativeToParent().AlignToVisualLineTop(),
                    EndingPoint = tmpEndPos.GetPositionRelativeToParent().AlignToVisualLineBottom()
                });
            }

            return pairs;
        }

        #endregion

    }
}

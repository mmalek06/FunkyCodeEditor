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

        public void Select(TextPosition position) => MakeSelection(position);

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

        #endregion

        #region event handlers

        public void HandleMouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                MakeSelection(e.GetPosition(this).GetDocumentPosition());
            }
        }

        public void HandleMouseDown(object sender, MouseButtonEventArgs e) {
            Deselect();
        }

        #endregion

        #region methods

        private void MakeSelection(TextPosition position) {
            var selection = new VisualElement();

            visuals.Clear();
            SetSelectionState(position);
            selection.Draw(GetSelectionPoints(lastSelectionStart, lastSelectionEnd));
            visuals.Add(selection);
        }

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

            visuals.Clear();

            for (int i = start.Line; i <= end.Line; i++) {
                TextPosition tmpStartPos = new TextPosition { Column = 0, Line = i };
                TextPosition tmpEndPos = new TextPosition { Column = 0, Line = i };

                if (i == start.Line) {
                    tmpStartPos.Column = start.Column;
                }

                int lineLen = textInfo.GetTextLineLength(i);

                if (i == end.Line) {
                    tmpEndPos.Column = end.Column > lineLen ? lineLen : end.Column;
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

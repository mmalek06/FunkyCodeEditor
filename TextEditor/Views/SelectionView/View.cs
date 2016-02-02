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

        public void Select(TextPosition start, TextPosition end) {
            var selection = new VisualElement();

            visuals.Clear();
            selection.Draw(GetSelectionPoints(start, end));
            visuals.Add(selection);

            lastSelectionStart = start;
            lastSelectionEnd = end;
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
                var selection = new VisualElement();

                visuals.Clear();

                if (!isSelecting) {
                    isSelecting = true;

                    lastSelectionStart = e.GetPosition(this).GetDocumentPosition();
                    lastSelectionEnd = lastSelectionStart;
                } else {
                    lastSelectionEnd = e.GetPosition(this).GetDocumentPosition();
                }

                selection.Draw(GetSelectionPoints(lastSelectionStart, lastSelectionEnd));
                visuals.Add(selection);
            }
        }

        public void HandleMouseDown(object sender, MouseButtonEventArgs e) {
            ClearSelection();
        }

        public void HandleKeyDown(object sender, KeyEventArgs e) {
            ClearSelection();
        }

        #endregion

        #region methods

        private void ClearSelection() {
            lastSelectionStart = null;
            lastSelectionEnd = null;
            isSelecting = false;

            visuals.Clear();
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

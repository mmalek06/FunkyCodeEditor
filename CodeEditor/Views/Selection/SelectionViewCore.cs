using System.Collections.Generic;
using System.Windows.Input;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;
using CodeEditor.Messaging;
using CodeEditor.Views.BaseClasses;
using CodeEditor.Views.Text;

namespace CodeEditor.Views.Selection {
    internal partial class SelectionView : InputViewBase {

        #region fields

        private readonly ITextViewReadonly textViewReader;

        private TextPosition lastSelectionStart;

        private TextPosition lastSelectionEnd;

        private bool isSelecting;

        #endregion

        #region constructor

        public SelectionView(ITextViewReadonly textViewReader) : base() {
            isSelecting = false;
            this.textViewReader = textViewReader;
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

        #endregion

        #region event handlers

        public void HandleMouseDown(MouseButtonEventArgs e) => Deselect();

        public override void HandleTextFolding(FoldClickedMessage message) => Deselect();

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
            visuals.Clear();

            if (start.Line <= end.Line) {
                return GetSelectionPointsForward(start, end);
            } else {
                return GetSelectionPointsInverted(start, end);
            }
        }

        private IEnumerable<PointsPair> GetSelectionPointsForward(TextPosition start, TextPosition end) {
            var pairs = new List<PointsPair>();

            for (var i = start.Line; i <= end.Line; i++) {
                var tmpStartColumn = 0;
                var tmpStartLine = i;
                var tmpEndColumn = 0;
                var tmpEndLine = i;

                if (i == start.Line) {
                    tmpStartColumn = start.Column;
                }

                var lineLen = textViewReader.GetLineLength(i);

                if (i == end.Line) {
                    tmpEndColumn = end.Column > lineLen ? lineLen : end.Column;
                } else {
                    tmpEndColumn = lineLen;
                }

                pairs.Add(new PointsPair {
                    StartingPoint = (new TextPosition(column: tmpStartColumn, line: tmpStartLine)).GetPositionRelativeToParent(TextConfiguration.GetCharSize())
                                                                                                  .AlignToVisualLineTop(TextConfiguration.GetCharSize()),
                    EndingPoint = (new TextPosition(column: tmpEndColumn, line: tmpEndLine)).GetPositionRelativeToParent(TextConfiguration.GetCharSize())
                                                                                            .AlignToVisualLineBottom(TextConfiguration.GetCharSize())
                });
            }

            return pairs;
        }

        private IEnumerable<PointsPair> GetSelectionPointsInverted(TextPosition start, TextPosition end) {
            var pairs = new List<PointsPair>();

            for (var i = start.Line; i >= end.Line; i--) {
                var tmpStartColumn = 0;
                var tmpStartLine = i;
                var tmpEndColumn = 0;
                var tmpEndLine = i;
                var lineLen = textViewReader.GetLineLength(i);

                if (i == start.Line) {
                    tmpStartColumn = start.Column;
                } else if (i == end.Line) {
                    tmpStartColumn = end.Column > lineLen ? lineLen : end.Column;
                    tmpEndColumn = lineLen;
                } else {
                    tmpStartColumn = 0;
                    tmpEndColumn = lineLen;
                }

                pairs.Add(new PointsPair {
                    StartingPoint = (new TextPosition(column: tmpStartColumn, line: tmpStartLine)).GetPositionRelativeToParent(TextConfiguration.GetCharSize())
                                                                                                  .AlignToVisualLineTop(TextConfiguration.GetCharSize()),
                    EndingPoint = (new TextPosition(column: tmpEndColumn, line: tmpEndLine)).GetPositionRelativeToParent(TextConfiguration.GetCharSize())
                                                                                            .AlignToVisualLineBottom(TextConfiguration.GetCharSize())
                });
            }

            return pairs;
        }

        #endregion

    }
}

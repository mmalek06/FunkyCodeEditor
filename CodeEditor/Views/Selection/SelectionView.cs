using System;
using System.Collections.Generic;
using System.Windows.Input;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Extensions;
using CodeEditor.Messaging;
using CodeEditor.Views.BaseClasses;
using LocalTextInfo = CodeEditor.Views.Text.TextInfo;

namespace CodeEditor.Views.Selection {
    internal class SelectionView : InputViewBase {

        #region fields

        private TextPosition lastSelectionStart;
        private TextPosition lastSelectionEnd;
        private LocalTextInfo textInfo;
        private bool isSelecting;

        #endregion

        #region constructor

        public SelectionView(LocalTextInfo textInfo) : base() {
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
                StartPosition = lastSelectionStart <= lastSelectionEnd ? lastSelectionStart : lastSelectionEnd,
                EndPosition = lastSelectionStart <= lastSelectionEnd ? lastSelectionEnd : lastSelectionStart
            };
        }

        public bool HasSelection() => lastSelectionStart != null && lastSelectionEnd != null && isSelecting;

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

            for (int i = start.Line; i <= end.Line; i++) {
                int tmpStartColumn = 0;
                int tmpStartLine = i;
                int tmpEndColumn = 0;
                int tmpEndLine = i;

                if (i == start.Line) {
                    tmpStartColumn = start.Column;
                }

                int lineLen = textInfo.GetTextLineLength(i);

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

            for (int i = start.Line; i >= end.Line; i--) {
                int tmpStartColumn = 0;
                int tmpStartLine = i;
                int tmpEndColumn = 0;
                int tmpEndLine = i;
                int lineLen = textInfo.GetTextLineLength(i);

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

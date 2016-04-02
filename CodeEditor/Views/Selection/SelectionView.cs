using System.Collections.Generic;
using System.Windows.Input;
using CodeEditor.Algorithms.Selection;
using CodeEditor.Core.DataStructures;
using CodeEditor.Messaging;
using CodeEditor.Views.BaseClasses;
using CodeEditor.Views.Text;

namespace CodeEditor.Views.Selection {
    internal class SelectionView : InputViewBase {

        #region fields

        private TextPosition lastSelectionStart;

        private TextPosition lastSelectionEnd;

        private TextSelector selectionAlgorithm;

        private bool isSelecting;

        #endregion

        #region properties

        public TextSelector SelectionAlgorithm => selectionAlgorithm;

        #endregion

        #region constructor

        public SelectionView(TextView.TextViewInfo textViewInfo) : base() {
            isSelecting = false;
            selectionAlgorithm = new TextSelector(textViewInfo, this);
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
                return selectionAlgorithm.GetSelectionPointsForward(start, end);
            } else {
                return selectionAlgorithm.GetSelectionPointsInverted(start, end);
            }
        }

        #endregion

    }
}

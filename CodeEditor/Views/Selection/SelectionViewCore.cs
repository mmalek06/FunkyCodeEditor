using System.Collections.Generic;
using System.Windows.Input;
using CodeEditor.Algorithms.Selection;
using CodeEditor.Core.DataStructures;
using CodeEditor.Messaging;
using CodeEditor.Views.BaseClasses;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Text;

namespace CodeEditor.Views.Selection {
    internal partial class SelectionView : InputViewBase {

        #region fields

        private TextPosition lastSelectionStart;

        private TextPosition lastSelectionEnd;

        private TextSelectionAlgorithm selectionAlgorithm;

        private bool isSelecting;

        #endregion

        #region properties

        public TextSelectionAlgorithm SelectionAlgorithm => selectionAlgorithm;

        #endregion

        #region constructor

        public SelectionView(ITextViewReadonly textViewReader, ICaretViewReadonly caretViewReader) : base() {
            isSelecting = false;
            selectionAlgorithm = new TextSelectionAlgorithm(caretViewReader, textViewReader, this);
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
                return selectionAlgorithm.GetSelectionPointsForward(start, end);
            } else {
                return selectionAlgorithm.GetSelectionPointsInverted(start, end);
            }
        }

        #endregion

    }
}

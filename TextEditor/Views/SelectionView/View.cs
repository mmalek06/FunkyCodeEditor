using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TextEditor.DataStructures;
using TextEditor.Events;
using TextEditor.Extensions;

namespace TextEditor.Views.SelectionView {
    public class View : ViewBase {

        #region fields

        private IList<PointsPair> pairs;
        private bool isSelecting;

        #endregion

        #region properties

        public Point SelectionStart { get; set; }
        public Point SelectionEnd { get; set; }
        public List<int> LineLengths { get; private set; }

        #endregion

        #region constructor

        public View() : base() {
            isSelecting = false;
            pairs = new List<PointsPair>();
            LineLengths = new List<int>();
        }

        #endregion

        #region event handlers

        public void HandleTextChange(object sender, TextChangedEventArgs e) {

        }

        public void HandleMouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                var rawPosition = e.GetPosition(this);
                var alignedPosition = rawPosition.AlignToVisualLineTop();
                var selection = new VisualElement();

                visuals.Clear();

                if (!isSelecting) {
                    isSelecting = true;

                    SelectionStart = alignedPosition;
                    SelectionEnd = SelectionStart;
                } else {
                    SelectionEnd = rawPosition.AlignToVisualLineBottom();
                }

                pairs.Add(LimitSelection(new PointsPair { StartingPoint = SelectionStart, EndingPoint = SelectionEnd }));

                selection.Draw(pairs);
                visuals.Add(selection);
            }
        }

        public void HandleMouseDown(object sender, MouseButtonEventArgs e) {
            pairs.Clear();
            visuals.Clear();
        }

        public void HandleMouseUp(object sender, MouseButtonEventArgs e) {
            isSelecting = false;
        }

        #endregion

        #region methods

        private bool IsSelectionRequested(KeyEventArgs e) {
            bool isArrowKey = (new[] { Key.Left, Key.Right, Key.Up, Key.Down }).Contains(e.Key);

            return isArrowKey && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);
        }

        private PointsPair LimitSelection(PointsPair box) {
            return box;
        }

        #endregion

    }
}

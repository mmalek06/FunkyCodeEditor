using System.Windows.Media;
using CodeEditor.Configuration;
using CodeEditor.Enums;
using CodeEditor.Messaging;
using CodeEditor.Views.BaseClasses;

namespace CodeEditor.Views.Lines {
    internal class LinesView : HelperViewBase {

        #region fields

        private int linesCount;


        #endregion

        #region constructor

        public LinesView() {
            bgBrush = SharedEditorConfiguration.GetLinesColumnBrush();
            linesCount = 1;
        }

        #endregion

        #region event handlers

        public override void HandleTextRemove(TextRemovedMessage message) {}

        public override void HandleLinesRemove(LinesRemovedMessage message) {
            linesCount -= message.Count;

            while (message.Count > 0) { 
                Pop();

                message.Count--;
            }

            UpdateSize();
        }

        public override void HandleTextInput(TextAddedMessage message) {
            if (message.Text == TextProperties.Properties.NEWLINE) {
                linesCount++;
                Push();
                UpdateSize();
            }
        }

        public void HandleFolding(FoldClickedMessage message) {
            if (message.State == FoldingStates.FOLDED) {
                int diff = message.AreaBeforeFolding.EndPosition.Line - message.AreaBeforeFolding.StartPosition.Line;

                linesCount -= diff;

                while (diff > 0) {
                    Pop();

                    diff--;
                }
            } else {
                int diff = message.AreaAfterFolding.EndPosition.Line - message.AreaAfterFolding.StartPosition.Line;

                linesCount += diff;

                while (diff > 0) {
                    Push();

                    diff--;
                }
            }

            UpdateSize();
        }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            Push();
        }

        #endregion

        #region methods

        protected override double GetWidth() => SharedEditorConfiguration.GetLinesColumnWidth();

        private void UpdateSize() {
            double h = linesCount * TextConfiguration.GetCharSize().Height;

            if (h > ActualHeight) {
                Height = h;
            }
        }

        private void Push() {
            visuals.Add(new VisualElement(visuals.Count + 1));
        }

        private void Pop() {
            visuals.RemoveAt(visuals.Count - 1);
        }

        #endregion

    }
}

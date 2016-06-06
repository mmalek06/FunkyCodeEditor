using System.Windows.Media;
using CodeEditor.Configuration;
using CodeEditor.Enums;
using CodeEditor.Messaging;
using CodeEditor.Views.BaseClasses;

namespace CodeEditor.Views.Lines {
    internal class LinesView : HelperViewBase {

        #region fields

        private int linesCount;

        private bool initialRendering;

        #endregion

        #region constructor

        public LinesView() {
            bgBrush = SharedEditorConfiguration.GetLinesColumnBrush();
            linesCount = 1;
            initialRendering = true;
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
                var diff = message.AreaBeforeFolding.EndPosition.Line - message.AreaBeforeFolding.StartPosition.Line;

                linesCount -= diff;

                while (diff > 0) {
                    Pop();

                    diff--;
                }
            } else {
                var diff = message.AreaAfterFolding.EndPosition.Line - message.AreaAfterFolding.StartPosition.Line;

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

            if (initialRendering) {
                Push();

                initialRendering = false;
            }
        }

        #endregion

        #region methods

        protected override double GetWidth() => SharedEditorConfiguration.GetLinesColumnWidth();

        private void UpdateSize() {
            var h = linesCount * TextConfiguration.GetCharSize().Height;

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

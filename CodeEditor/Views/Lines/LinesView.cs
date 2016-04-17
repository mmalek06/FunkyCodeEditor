using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Messaging;
using CodeEditor.TextProperties;
using CodeEditor.Views.BaseClasses;

namespace CodeEditor.Views.Lines {
    internal class LinesView : HelperViewBase {

        #region fields

        private int linesCount;
        private TextFormatter formatter;
        private SimpleParagraphProperties paragraphProperties;

        #endregion

        public VisualCollection VS => visuals;

        #region constructor

        public LinesView() : base() {
            bgBrush = EditorConfiguration.GetLinesColumnBrush();
            linesCount = 1;
            formatter = TextFormatter.Create();
            paragraphProperties = new SimpleParagraphProperties { defaultTextRunProperties = TextConfiguration.GetGlobalTextRunProperties() };
        }

        #endregion

        #region event handlers

        public override void HandleLinesRemove(int count) {
            int initialLinesCount = linesCount;

            linesCount -= count;

            while (count > 0) { 
                Pop();

                count--;
            }

            UpdateSize();
        }

        public override void HandleTextInput(string text, TextPosition activePosition) {
            if (text == TextProperties.Properties.NEWLINE) {
                linesCount++;
                Push();
                UpdateSize();
            }
        }

        public void HandleFoldRemove(FoldClickedMessage m) {
            int diff = m.Area.EndPosition.Line - m.Area.StartPosition.Line;
            
            if (m.State == Algorithms.Folding.FoldingStates.FOLDED) {
                linesCount -= diff;

                for (int i = linesCount; i <= diff; i++) {
                    Pop();
                }
            } else {
                int initialLinesCount = linesCount;

                linesCount += diff;

                for (int i = initialLinesCount; i <= diff; i++) {
                    Push();
                }
            }

            UpdateSize();
        }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            Push();
        }

        protected override double GetWidth() => EditorConfiguration.GetLinesColumnWidth();

        #endregion

        #region methods

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

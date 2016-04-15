using System.Collections.Generic;
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

            for (int i = initialLinesCount; i < count; i++) {
                RemoveLine(i);
            }

            UpdateSize();
        }

        public override void HandleTextInput(string text, TextPosition activePosition) {
            if (text == TextProperties.Properties.NEWLINE) {
                linesCount++;
                AddLine(linesCount);
                UpdateSize();
            }
        }

        public void HandleFoldRemove(FoldClickedMessage m) {
            int diff = m.Area.EndPosition.Line - m.Area.StartPosition.Line;
            
            if (m.State == Algorithms.Folding.FoldingStates.FOLDED) {
                linesCount -= diff;

                for (int i = linesCount; i <= diff; i++) {
                    RemoveLine(linesCount);
                }
            } else {
                int initialLinesCount = linesCount;

                linesCount += diff;

                for (int i = initialLinesCount; i <= diff; i++) {
                    AddLine(i + initialLinesCount);
                }
            }

            UpdateSize();
        }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            AddLine(linesCount);
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

        private void AddLine(int lineNum) {
            visuals.Add(new VisualElement(lineNum));
        }

        private void RemoveLine(int lineNum) {
            visuals.RemoveAt(lineNum);
        }

        #endregion

    }
}

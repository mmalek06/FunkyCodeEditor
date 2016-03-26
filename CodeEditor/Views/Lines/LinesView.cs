using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Extensions;
using CodeEditor.Messaging;
using CodeEditor.TextProperties;
using CodeEditor.Views.BaseClasses;

namespace CodeEditor.Views.Lines {
    internal class LinesView : HelperViewBase {

        #region enums

        private enum TextInputType { ADD, REMOVE };

        #endregion

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
            linesCount -= count;

            for (int i = 0; i < count; i++) {
                RedrawLines(TextInputType.REMOVE);
            }

            UpdateSize();
        }

        public override void HandleTextInput(string text, TextPosition activePosition) {
            if (text == TextProperties.Properties.NEWLINE) {
                linesCount++;
                RedrawLines(TextInputType.ADD);
                UpdateSize();
            }
        }

        public void HandleFoldRemove(FoldClickedMessage m) {
            int diff = m.Area.EndPosition.Line - m.Area.StartPosition.Line;

            if (m.State == Algorithms.Folding.FoldingStates.FOLDED) {
                linesCount -= diff;

                for (int i = 0; i < diff; i++) {
                    RedrawLines(TextInputType.REMOVE);
                }
            } else {
                linesCount += diff;
                RedrawLines(TextInputType.ADD);
            }

            UpdateSize();
        }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            RedrawLines(TextInputType.ADD);
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

        private void RedrawLines(TextInputType inputType) {
            if (inputType == TextInputType.ADD) {
                visuals.Add(new VisualElement(linesCount));
            } else {
                var lastNum = visuals[visuals.Count - 1];

                visuals.Remove(lastNum);
            }
        }

        #endregion

    }
}

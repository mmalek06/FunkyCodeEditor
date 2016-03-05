﻿using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
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
            paragraphProperties = new SimpleParagraphProperties { defaultTextRunProperties = Configuration.TextConfiguration.GetGlobalTextRunProperties() };
        }

        #endregion

        #region event handlers

        public override void HandleTextRemove(Key key, TextPosition activePosition) {
            if (activePosition.Line == 0 && activePosition.Column == 0 && key == Key.Back) {
                return;
            }
            if (key == Key.Back && activePosition.Column == 0) {
                linesCount--;
                RedrawLines();
            } else if (key == Key.Delete) {
                linesCount--;
                RedrawLines();
            }
        }

        public override void HandleTextInput(string text, TextPosition activePosition) {
            if (text == TextProperties.Properties.NEWLINE) {
                linesCount++;
                RedrawLines();
            }
        }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            RedrawLines();
        }

        protected override double GetWidth() => EditorConfiguration.GetLinesColumnWidth();

        #endregion

        #region methods

        private void RedrawLines() {
            var lineNumbers = Enumerable.Range(1, linesCount).ToArray();

            visuals.Clear();

            foreach (int num in lineNumbers) {
                var el = new VisualElement(num);

                visuals.Add(el);
            }
        }

        #endregion

    }
}
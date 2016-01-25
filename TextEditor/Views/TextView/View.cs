using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using TextEditor.DataStructures;
using TextEditor.Events;
using TextEditor.Extensions;
using TextEditor.TextProperties;

namespace TextEditor.Views.TextView {
    public class View : ViewBase {

        #region events

        public event TextChangedEventHandler TextChanged;

        #endregion

        #region fields

        private List<SimpleTextSource> textSources;
        private TextFormatter formatter;
        private TextRunProperties runProperties;
        private SimpleParagraphProperties paragraphProperties;
        private TextLineTransformer transformer;

        #endregion

        #region properties

        public int ActiveLineIndex { get; private set; } = 0;
        public int ActiveColumnIndex { get; private set; } = 0;

        #endregion

        #region constructor

        public View() : base() {
            formatter = TextFormatter.Create();
            runProperties = this.CreateGlobalTextRunProperties();
            textSources = new List<SimpleTextSource> { new SimpleTextSource(string.Empty, runProperties) };
            paragraphProperties = new SimpleParagraphProperties { defaultTextRunProperties = runProperties };
            transformer = new TextLineTransformer(textSources);
        }

        #endregion

        #region event handlers

        public void HandleCaretMove(object sender, CaretMovedEventArgs e) {
            ActiveLineIndex = e.NewPosition.Line;
            ActiveColumnIndex = e.NewPosition.Column;
        }

        public void HandleGotFocus(object sender, RoutedEventArgs e) => Focus();

        public void HandleMouseDown(object sender, MouseButtonEventArgs e) => Focus();

        #endregion

        #region public methods

        public void EnterText(string enteredText) {
            var newLines = transformer.CreateLines(enteredText, ActiveLineIndex, ActiveColumnIndex);
            
            UpdateTextData(newLines);
            UpdateCursorPosition(enteredText);
            DrawLines(newLines.Select(line => line.Key.Line));

            newLines.Clear();
        }

        public void RemoveText(Key key) {
            string lineToModify = textSources[ActiveLineIndex].Text;
            string newLineContent = string.Empty;
            
            if (key == Key.Delete) {
                textSources[ActiveLineIndex].Text = lineToModify.Substring(0, ActiveColumnIndex) + lineToModify.Substring(ActiveColumnIndex + 1);
            } else {
                bool attachRest = ActiveColumnIndex < textSources[ActiveLineIndex].Text.Length;
                
                textSources[ActiveLineIndex].Text = lineToModify.Substring(0, ActiveColumnIndex - 1) + (attachRest ? lineToModify.Substring(ActiveColumnIndex) : string.Empty);
                ActiveColumnIndex -= 1;
            }
            
            DrawLine(ActiveLineIndex);
        }

        public int GetTextLinesCount() => textSources.Count;

        public int GetTextLineLength(int index) => textSources[index].Text.Length;

        public string GetTextLine(int index) => index >= textSources.Count ? string.Empty : string.Copy(textSources[index].Text);

        public IEnumerable<string> GetTextLines() => textSources.Select(source => source.Text);

        public void TriggerTextChanged() => TextChanged(this, new TextChangedEventArgs { CurrentColumn = ActiveColumnIndex, CurrentLine = ActiveLineIndex });

        #endregion

        #region methods

        private void UpdateCursorPosition(string text) {
            var replacedText = transformer.SpecialCharsRegex.Replace(text, string.Empty);

            if (text == TextConfiguration.NEWLINE) {
                ActiveColumnIndex = 0;
                ActiveLineIndex += 1;
            } else if (replacedText.Length == 1) {
                ActiveColumnIndex += 1;
            } else {
                var parts = text.Split(TextConfiguration.NEWLINE[0]);

                ActiveColumnIndex = parts.Last().Length;
                ActiveLineIndex += parts.Length - 1;
            }
        }

        private void UpdateTextData(Dictionary<TextPosition, string> changedLines) {
            foreach (var kvp in changedLines) {
                if (kvp.Key.Line < textSources.Count) {
                    textSources[kvp.Key.Line].Text = kvp.Value;
                } else {
                    textSources.Add(new SimpleTextSource(kvp.Value, runProperties));
                }
            }
        }

        private void DrawLines(IEnumerable<int> lineIndices) {
            foreach (int index in lineIndices) {
                DrawLine(index);
            }
        }

        private void DrawLine(int index) {
            using (TextLine textLine = formatter.FormatLine(
                                textSources[index],
                                0,
                                96 * 6,
                                paragraphProperties,
                                null)) {
                double top = index * textLine.Height;

                if (index < visuals.Count) {
                    ((VisualTextLine)visuals[index]).Redraw(textLine, top);
                } else {
                    visuals.Add(new VisualTextLine(textLine, top));
                }
            }
        }

        #endregion

    }
}
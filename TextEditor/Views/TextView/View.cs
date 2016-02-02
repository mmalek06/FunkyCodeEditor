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
        private TextLineUpdater updatingAlgorithm;
        private TextLineRemover removingAlgorithm;

        #endregion

        #region properties

        public IList<string> Lines => textSources.Select(ts => string.Copy(ts.Text)).ToList();

        public int ActiveLineIndex { get; private set; } = 0;

        public int ActiveColumnIndex { get; private set; } = 0;

        #endregion

        #region constructor

        public View() : base() {
            formatter = TextFormatter.Create();
            runProperties = this.CreateGlobalTextRunProperties();
            textSources = new List<SimpleTextSource> { new SimpleTextSource(string.Empty, runProperties) };
            paragraphProperties = new SimpleParagraphProperties { defaultTextRunProperties = runProperties };
            updatingAlgorithm = new TextLineUpdater();
            removingAlgorithm = new TextLineRemover();
        }

        #endregion

        #region event handlers

        internal void HandleCaretMove(object sender, CaretMovedEventArgs e) {
            ActiveLineIndex = e.NewPosition.Line;
            ActiveColumnIndex = e.NewPosition.Column;
        }

        internal void HandleGotFocus(object sender, RoutedEventArgs e) => Focus();

        internal void HandleMouseDown(object sender, MouseButtonEventArgs e) => Focus();

        #endregion

        #region public methods

        public void EnterText(string enteredText) {
            var newLines = updatingAlgorithm.UpdateLines(textSources, new TextPosition { Line = ActiveLineIndex, Column = ActiveColumnIndex }, enteredText);

            UpdateTextData(newLines);
            UpdateCursorPosition(enteredText);
            DrawLines(newLines.Select(lineInfo => lineInfo.Key));

            newLines.Clear();
        }

        public void ReplaceText(IEnumerable<TextPositionsPair> oldText, IEnumerable<TextPositionsPair> newText) {

        }

        public void RemoveText(Key key) {
            var removalInfo = removingAlgorithm.TransformLines(textSources, new TextPosition { Column = ActiveColumnIndex, Line = ActiveLineIndex }, key);

            if (removalInfo.LinesAffected.Any()) {
                DeleteText(removalInfo);
            }
        }

        public void RemoveText(TextPositionsPair ranges) {
            var removalInfo = removingAlgorithm.TransformLines(textSources, ranges);

            if (removalInfo.LinesAffected.Any()) {
                DeleteText(removalInfo);
            }
        }

        public void TriggerTextChanged() {
            if (TextChanged != null) {
                TextChanged(this, new TextChangedEventArgs { CurrentColumn = ActiveColumnIndex, CurrentLine = ActiveLineIndex });
            }
        }

        #endregion

        #region methods

        private void DeleteText(LinesRemovalInfo removalInfo) {
            RemoveLines(removalInfo.LinesToRemove);
            UpdateTextData(removalInfo.LinesAffected.Select(pair => new KeyValuePair<int, string>(pair.Key.Line, pair.Value)));
            UpdateCursorPosition(removalInfo.LinesAffected.First().Key);
            DrawLines(removalInfo.LinesAffected.Select(lineInfo => lineInfo.Key.Line));
        }

        private void UpdateCursorPosition(TextPosition position) {
            ActiveColumnIndex = position.Column;
            ActiveLineIndex = position.Line;
        }

        private void UpdateCursorPosition(string text) {
            var replacedText = updatingAlgorithm.SpecialCharsRegex.Replace(text, string.Empty);

            if (text == TextConfiguration.NEWLINE) {
                ActiveColumnIndex = 0;
                ActiveLineIndex += 1;
            } else if (text == TextConfiguration.TAB) {
                ActiveColumnIndex += TextConfiguration.TabSize;
            } else if (replacedText.Length == 1) {
                ActiveColumnIndex += 1;
            } else {
                var parts = text.Split(TextConfiguration.NEWLINE[0]);

                ActiveColumnIndex = parts.Last().Length;
                ActiveLineIndex += parts.Length - 1;
            }
        }

        private void RemoveLines(IEnumerable<int> indices) {
            var textSourcesToRemove = textSources.Select((obj, index) => new { index, obj }).Where(obj => indices.Contains(obj.index)).ToArray();
            List<VisualTextLine> visualsToRemove = new List<VisualTextLine>();

            foreach (var txtSource in textSourcesToRemove) {
                textSources.Remove(txtSource.obj);
            }
            foreach (var visual in visuals) {
                var line = (VisualTextLine)visual;

                if (indices.Contains(line.Index)) {
                    visualsToRemove.Add(line);
                }
            }
            foreach (var line in visualsToRemove) {
                visuals.Remove(line);
            }
        }

        private void UpdateTextData(IEnumerable<KeyValuePair<int, string>> changedLines) {
            foreach (var kvp in changedLines) {
                if (kvp.Key < textSources.Count) {
                    textSources[kvp.Key].Text = kvp.Value;
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
                    visuals.Add(new VisualTextLine(textLine, top, index));
                }
            }
        }

        #endregion

    }
}
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using CodeEditor.DataStructures;
using CodeEditor.Events;
using CodeEditor.TextProperties;
using CodeEditor.Algorithms.TextManipulation;

namespace CodeEditor.Views.TextView {
    internal class View : ViewBase {

        #region events

        public event TextChangedEventHandler TextChanged;

        #endregion

        #region fields

        private List<SimpleTextSource> textSources;
        private TextFormatter formatter;
        private SimpleParagraphProperties paragraphProperties;
        private TextUpdater updatingAlgorithm;
        private TextRemover removingAlgorithm;

        #endregion

        #region properties

        public IList<string> Lines => textSources.Select(ts => string.Copy(ts.Text)).ToList();

        public TextPosition ActivePosition { get; private set; } = new TextPosition(column: 0, line: 0);

        #endregion

        #region constructor

        public View() : base() {
            formatter = TextFormatter.Create();
            textSources = new List<SimpleTextSource> { new SimpleTextSource(string.Empty, Configuration.TextConfiguration.GetGlobalTextRunProperties()) };
            paragraphProperties = new SimpleParagraphProperties { defaultTextRunProperties = Configuration.TextConfiguration.GetGlobalTextRunProperties() };
            updatingAlgorithm = new TextUpdater();
            removingAlgorithm = new TextRemover();
        }

        #endregion

        #region event handlers

        public void HandleCaretMove(object sender, CaretMovedEventArgs e) => UpdateCursorPosition(e.NewPosition);

        public void HandleGotFocus(object sender, RoutedEventArgs e) => Focus();

        public void HandleMouseDown(MouseButtonEventArgs e) => Focus();

        #endregion

        #region public methods

        public void EnterText(string enteredText) => InputText(enteredText);

        public void ReplaceText(string enteredText, TextPositionsPair range) {
            RemoveText(range);
            InputText(enteredText);
        }

        public void RemoveText(Key key) {
            var removalInfo = removingAlgorithm.TransformLines(textSources, ActivePosition, key);

            if (removalInfo.LinesToChange.Any()) {
                DeleteText(removalInfo);
            }
        }

        public void RemoveText(TextPositionsPair ranges) {
            var removalInfo = removingAlgorithm.TransformLines(textSources, ranges);

            if (removalInfo.LinesToChange.Any()) {
                DeleteText(removalInfo);
            }
        }

        public void TriggerTextChanged() {
            if (TextChanged != null) {
                TextChanged(this, new TextChangedEventArgs { CurrentColumn = ActivePosition.Column, CurrentLine = ActivePosition.Line });
            }
        }

        #endregion

        #region methods

        private void InputText(string enteredText) {
            var newLines = updatingAlgorithm.UpdateLines(textSources, ActivePosition, enteredText);

            UpdateTextData(newLines);
            UpdateCursorPosition(enteredText);
            DrawLines(newLines.Select(lineInfo => lineInfo.Key));
        }

        private void DeleteText(LinesRemovalInfo removalInfo) {
            RemoveLines(removalInfo.LinesToRemove);
            UpdateTextData(removalInfo.LinesToChange.ToDictionary(pair => pair.Key.Line, pair => pair.Value));
            UpdateCursorPosition(removalInfo.LinesToChange.First().Key);
            DrawLines(removalInfo.LinesToChange.Select(lineInfo => lineInfo.Key.Line));
        }

        private void UpdateCursorPosition(TextPosition position) => ActivePosition = new TextPosition(column: position.Column, line: position.Line);

        private void UpdateCursorPosition(string text) {
            var replacedText = updatingAlgorithm.SpecialCharsRegex.Replace(text, string.Empty);
            int column = -1;
            int line = -1;

            if (text == TextConfiguration.NEWLINE) {
                column = 0;
                line = ActivePosition.Line + 1;
            } else if (text == TextConfiguration.TAB) {
                column = ActivePosition.Column + TextConfiguration.TabSize;
            } else if (replacedText.Length == 1) {
                column = ActivePosition.Column + 1;
            } else {
                var parts = text.Split(TextConfiguration.NEWLINE[0]);

                column = parts.Last().Length;
                line = ActivePosition.Line + parts.Length - 1;
            }

            ActivePosition = new TextPosition(column: column > -1 ? column : ActivePosition.Column, line: line > -1 ? line : ActivePosition.Line);
        }

        private void RemoveLines(IEnumerable<int> indices) {
            var textSourcesToRemove = textSources.Select((obj, index) => new { index, obj }).Where(obj => indices.Contains(obj.index)).ToArray();
            List<VisualElement> visualsToRemove = new List<VisualElement>();

            foreach (var txtSource in textSourcesToRemove) {
                textSources.Remove(txtSource.obj);
            }
            foreach (var visual in visuals) {
                var line = (VisualElement)visual;

                if (indices.Contains(line.Index)) {
                    visualsToRemove.Add(line);
                }
            }
            foreach (var line in visualsToRemove) {
                visuals.Remove(line);
            }
        }

        private void UpdateTextData(IDictionary<int, string> changedLines) {
            foreach (var kvp in changedLines) {
                if (kvp.Key < textSources.Count) {
                    textSources[kvp.Key].Text = kvp.Value;
                } else {
                    textSources.Add(new SimpleTextSource(kvp.Value, Configuration.TextConfiguration.GetGlobalTextRunProperties()));
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
                    ((VisualElement)visuals[index]).Redraw(textLine, top);
                } else {
                    visuals.Add(new VisualElement(textLine, top, index));
                }
            }
        }

        #endregion

    }
}
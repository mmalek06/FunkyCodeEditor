using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Core.DataStructures;
using CodeEditor.Events;
using CodeEditor.Algorithms.TextManipulation;
using CodeEditor.Configuration;
using CodeEditor.Views.BaseClasses;
using CodeEditor.Messaging;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Visuals;

namespace CodeEditor.Views.Text {
    internal class TextView : InputViewBase {

        #region events

        public event TextChangedEventHandler TextChanged;

        #endregion

        #region fields

        private List<SimpleTextSource> textSources;
        private Dictionary<TextPositionsPair, List<string>> collapsedLines;
        private TextUpdater updatingAlgorithm;
        private TextRemover removingAlgorithm;
        private TextCollapser collapsingAlgorithm;

        #endregion

        #region properties

        public IEnumerable<string> VisibleTextLines => textSources.Select(source => source.Text);

        public IEnumerable<string> AllTextLines => textSources.Select(source => source.Text);

        public TextPosition ActivePosition { get; private set; } = new TextPosition(column: 0, line: 0);

        #endregion

        #region constructor

        public TextView() : base() {
            collapsedLines = new Dictionary<TextPositionsPair, List<string>>();
            textSources = new List<SimpleTextSource> { new SimpleTextSource(string.Empty, TextConfiguration.GetGlobalTextRunProperties()) };
            updatingAlgorithm = new TextUpdater();
            removingAlgorithm = new TextRemover();
            collapsingAlgorithm = new TextCollapser();
        }

        #endregion

        #region event handlers

        public void HandleCaretMove(object sender, CaretMovedEventArgs e) => UpdateCursorPosition(e.NewPosition);

        public void HandleGotFocus(object sender, RoutedEventArgs e) => Focus();

        public void HandleMouseDown(MouseButtonEventArgs e) => Focus();

        public override void HandleTextFolding(FoldClickedMessage message) {
            if (message.State == FoldingStates.EXPANDED) {
                ExpandText(message);
            } else {
                CollapseText(message);
            }
        }

        #endregion

        #region public methods

        public void EnterText(string enteredText) {
            InputText(enteredText);
            UpdateSize();
        }

        public void ReplaceText(string enteredText, TextPositionsPair range) {
            RemoveText(range);
            InputText(enteredText);
            UpdateSize();
        }

        public void RemoveText(Key key) {
            var removalInfo = removingAlgorithm.RemoveLines(textSources, ActivePosition, key);

            if (removalInfo.LinesToChange.Any()) {
                DeleteText(removalInfo);
                UpdateSize();
            } else if (removalInfo.LinesToRemove.Any()) {
                DeleteLines(removalInfo.LinesToRemove);
                UpdateSize();
            }
        }

        public void RemoveText(TextPositionsPair ranges) {
            var removalInfo = removingAlgorithm.RemoveLines(textSources, ranges);

            if (removalInfo.LinesToChange.Any()) {
                DeleteText(removalInfo);
                UpdateSize();
            }
        }

        public void CollapseText(FoldClickedMessage message) {
            var collapsedLine = collapsingAlgorithm.CollapseTextRange(message.Area, textSources, message.Area.StartPosition.Line);
            var removeTextRange = new TextPositionsPair {
                StartPosition = new TextPosition(column: message.Area.StartPosition.Column, line: message.Area.StartPosition.Line),
                EndPosition = new TextPosition(column: textSources[message.Area.EndPosition.Line].Text.Length, line: message.Area.EndPosition.Line)
            };

            RemoveText(removeTextRange);

            visuals[message.Area.StartPosition.Line] = null;
            visuals[message.Area.StartPosition.Line] = collapsedLine;

            DrawLine(message.Area.StartPosition.Line);
            UpdateCursorPosition(message.Area.StartPosition);
        }

        public void ExpandText(FoldClickedMessage message) {
            collapsingAlgorithm.ExpandTextRange(message.Area, textSources, ActivePosition.Line);
        }

        public void TriggerTextChanged() {
            if (TextChanged != null) {
                TextChanged(this, new TextChangedEventArgs { CurrentColumn = ActivePosition.Column, CurrentLine = ActivePosition.Line });
            }
        }

        public void TriggerTextChanged(string text) {
            if (TextChanged != null) {
                TextChanged(this, new TextChangedEventArgs { Text = text, CurrentColumn = ActivePosition.Column, CurrentLine = ActivePosition.Line });
            }
        }

        #endregion

        #region methods

        private void InputText(string enteredText) {
            var newLines = updatingAlgorithm.UpdateLines(textSources, ActivePosition, enteredText);

            UpdateTextData(newLines);
            UpdateCursorPosition(enteredText);
            DrawLines(newLines.Keys);
        }

        private void DeleteText(LinesRemovalInfo removalInfo) {
            RemoveLines(removalInfo.LinesToRemove);
            UpdateTextData(removalInfo.LinesToChange.ToDictionary(pair => pair.Key.Line, pair => pair.Value));
            UpdateCursorPosition(removalInfo.LinesToChange.First().Key);
            DrawLines(removalInfo.LinesToChange.Select(lineInfo => lineInfo.Key.Line));
        }

        private void DeleteLines(IEnumerable<int> linesToRemove) {
            RemoveLines(linesToRemove);

            int textLen = textSources[linesToRemove.Min() - 1].Text.Length;

            UpdateCursorPosition(new TextPosition(column: textLen > 0 ? textLen - 1 : 0, line: linesToRemove.Min() - 1));
        }

        private void UpdateSize() {
            Height = Convert.ToInt32(VisibleTextLines.Count() * TextConfiguration.GetCharSize().Height);
            Width = VisibleTextLines.Max(line => line.Length) * TextConfiguration.GetCharSize().Width;
        }

        private void UpdateCursorPosition(TextPosition position) => ActivePosition = new TextPosition(column: position.Column, line: position.Line);

        private void UpdateCursorPosition(string text) {
            var replacedText = updatingAlgorithm.SpecialCharsRegex.Replace(text, string.Empty);
            int column = -1;
            int line = -1;

            if (text == TextProperties.Properties.NEWLINE) {
                column = 0;
                line = ActivePosition.Line + 1;
            } else if (text == TextProperties.Properties.TAB) {
                column = ActivePosition.Column + TextProperties.Properties.TabSize;
            } else if (replacedText.Length == 1) {
                column = ActivePosition.Column + 1;
            } else {
                var parts = text.Split(TextProperties.Properties.NEWLINE[0]);

                column = parts.Last().Length;
                line = ActivePosition.Line + parts.Length - 1;
            }

            ActivePosition = new TextPosition(column: column > -1 ? column : ActivePosition.Column, line: line > -1 ? line : ActivePosition.Line);
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

        private void UpdateTextData(IDictionary<int, string> changedLines) {
            foreach (var kvp in changedLines) {
                if (kvp.Key < textSources.Count) {
                    textSources[kvp.Key].Text = kvp.Value;
                } else {
                    textSources.Add(new SimpleTextSource(kvp.Value, TextConfiguration.GetGlobalTextRunProperties()));
                }
            }
        }

        private void DrawLines(IEnumerable<int> lineIndices) {
            foreach (int index in lineIndices) {
                DrawLine(index);
            }
        }

        private void DrawLine(int index) {
            if (index < visuals.Count) {
                ((VisualTextLine)visuals[index]).Redraw();
            } else {
                visuals.Add(VisualTextLine.Create(textSources[index], index));
            }
        }

        #endregion

    }
}
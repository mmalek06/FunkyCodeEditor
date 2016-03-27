﻿using System;
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
    internal partial class TextView : InputViewBase {

        #region events

        public event TextChangedEventHandler TextChanged;

        #endregion

        #region fields

        private TextUpdater updatingAlgorithm;

        private TextRemover removingAlgorithm;

        private TextCollapser collapsingAlgorithm;

        #endregion

        #region properties

        public TextPosition ActivePosition { get; private set; } = new TextPosition(column: 0, line: 0);

        #endregion

        #region constructor

        public TextView() : base() {
            updatingAlgorithm = new TextUpdater();
            removingAlgorithm = new TextRemover();
            collapsingAlgorithm = new TextCollapser();

            visuals.Add(new SingleVisualTextLine(new SimpleTextSource(string.Empty, TextConfiguration.GetGlobalTextRunProperties()), 0));
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
            var removalInfo = removingAlgorithm.RemoveLines(Lines, ActivePosition, key);

            if (removalInfo.LinesToChange.Any()) {
                DeleteText(removalInfo);
                UpdateSize();
            } else if (removalInfo.LinesToRemove.Any()) {
                DeleteLines(removalInfo.LinesToRemove);
                UpdateSize();
            }
        }

        public void RemoveText(TextPositionsPair ranges) {
            var removalInfo = removingAlgorithm.RemoveLines(Lines, ranges);

            if (removalInfo.LinesToChange.Any()) {
                DeleteText(removalInfo);
                UpdateSize();
            }
        }

        public void CollapseText(FoldClickedMessage message) {
            var collapsedLine = collapsingAlgorithm.CollapseTextRange(message.Area, Lines, message.Area.StartPosition.Line);
            var removeTextRange = new TextPositionsPair {
                StartPosition = new TextPosition(column: message.Area.StartPosition.Column, line: message.Area.StartPosition.Line),
                EndPosition = new TextPosition(column: Lines[message.Area.EndPosition.Line].Length, line: message.Area.EndPosition.Line)
            };

            RemoveText(removeTextRange);

            visuals[message.Area.StartPosition.Line] = null;
            visuals[message.Area.StartPosition.Line] = collapsedLine;

            //DrawLine(message.Area.StartPosition.Line);
            UpdateCursorPosition(message.Area.StartPosition);
        }

        public void ExpandText(FoldClickedMessage message) {
            collapsingAlgorithm.ExpandTextRange(message.Area, Lines, ActivePosition.Line);
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
            var newLines = updatingAlgorithm.UpdateLines(Lines, ActivePosition, enteredText);

            UpdateCursorPosition(enteredText);
            DrawLines(newLines);
        }

        private void DeleteText(LinesRemovalInfo removalInfo) {
            RemoveLines(removalInfo.LinesToRemove);
            UpdateCursorPosition(removalInfo.LinesToChange.First().Key);
            DrawLines(removalInfo.LinesToChange.ToDictionary(pair => pair.Key.Line, pair => pair.Value));
        }

        private void DeleteLines(IEnumerable<int> linesToRemove) {
            RemoveLines(linesToRemove);

            int textLen = Lines[linesToRemove.Min() - 1].Length;

            UpdateCursorPosition(new TextPosition(column: textLen > 0 ? textLen - 1 : 0, line: linesToRemove.Min() - 1));
        }

        private void UpdateSize() {
            int maxLineLen = 0;

            foreach (var visual in visuals) {
                var line = (VisualTextLine)visual;

                if (line.Text.Length > maxLineLen) {
                    maxLineLen = line.Text.Length;
                }
            }

            Width = maxLineLen * TextConfiguration.GetCharSize().Width;
            Height = Convert.ToInt32(visuals.Count * TextConfiguration.GetCharSize().Height);
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
            var textSourcesToRemove = Lines.Select((obj, index) => new { index, obj }).Where(obj => indices.Contains(obj.index)).ToArray();
            List<VisualTextLine> visualsToRemove = new List<VisualTextLine>();

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

        private void DrawLines(IDictionary<int, string> linesData) {
            foreach (var pair in linesData) {
                DrawLine(pair.Key, pair.Value);
            }
        }

        private void DrawLine(int index, string newText) {
            VisualTextLine line;

            if (index < visuals.Count) {
                line = (VisualTextLine)visuals[index];

                line.UpdateText(newText);
                line.Draw();
            } else {
                line = VisualTextLine.Create(newText, index);
                line.Draw();

                visuals.Add(line);
            }
        }

        #endregion

    }
}
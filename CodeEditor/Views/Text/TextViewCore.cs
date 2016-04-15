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
using CodeEditor.TextProperties;
using CodeEditor.Algorithms.Parsing;
using CodeEditor.Core.Extensions;

namespace CodeEditor.Views.Text {
    /// <summary>
    /// provides core functionality for entering text
    /// </summary>
    internal partial class TextView : InputViewBase {

        #region events

        public event TextChangedEventHandler TextChanged;

        #endregion

        #region fields

        private TextUpdater updatingAlgorithm;

        private TextRemover removingAlgorithm;

        private TextCollapser collapsingAlgorithm;

        private TextParser parsingAlgorithm;

        #endregion

        #region properties

        public TextViewInfo Info { get; set; }

        private TextPosition ActivePosition { get; set; } = new TextPosition(column: 0, line: 0);

        #endregion

        #region constructor

        public TextView() : base() {
            updatingAlgorithm = new TextUpdater();
            removingAlgorithm = new TextRemover();
            collapsingAlgorithm = new TextCollapser();
            parsingAlgorithm = new TextParser();
            Info = TextViewInfo.GetInstance(this);

            visuals.Add(new SingleVisualTextLine(new SimpleTextSource(string.Empty, TextConfiguration.GetGlobalTextRunProperties()), 0));
        }

        #endregion

        #region event handlers

        public void HandleCaretMove(object sender, CaretMovedEventArgs e) => UpdateActivePosition(e.NewPosition);

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

        public void ReplaceText(string enteredText, TextRange range) {
            RemoveText(range);
            InputText(enteredText);
            UpdateSize();
        }

        public void RemoveText(Key key) {
            var removalInfo = removingAlgorithm.GetChangeInLines(Info.GetActualLines(), ActivePosition, key);

            if (removalInfo.LinesToChange.Any()) {
                DeleteText(removalInfo);
                UpdateSize();
            } else if (removalInfo.LinesToRemove.Any()) {
                DeleteLines(removalInfo.LinesToRemove);
                UpdateSize();
            }
        }

        public void RemoveText(TextRange range) {
            var removalInfo = removingAlgorithm.GetChangeInLines(Info.GetActualLines(), range);

            if (removalInfo.LinesToChange.Any()) {
                DeleteText(removalInfo);
                UpdateSize();
            }
        }

        public void CollapseText(FoldClickedMessage message) {
            var collapsedLine = collapsingAlgorithm.CollapseTextRange(message.Area, Info.GetScreenLines(), message.Area.StartPosition.Line);
            var linesToRedraw = collapsingAlgorithm.GetLinesToRedrawAfterCollapse(visuals.ToEnumerableOf<VisualTextLine>().ToList(), collapsedLine, message.Area.EndPosition.Line);

            visuals.RemoveRange(message.Area.StartPosition.Line, (message.Area.EndPosition.Line + 1) - message.Area.StartPosition.Line);
            RedrawCollapsedLine(collapsedLine, message.Area.StartPosition.Line);
            AddLines(linesToRedraw);

            if (message.Area.Contains(ActivePosition)) {
                UpdateActivePosition(message.Area.StartPosition);
            }

            UpdateSize();
        }

        public void ExpandText(FoldClickedMessage message) {
            int collapseIndex = message.Area.StartPosition.Line;
            var collapsedLineContent = ((VisualTextLine)visuals[collapseIndex]).GetStringContents();
            var expandedLines = collapsedLineContent.Select((line, index) => VisualTextLine.Create(line, collapseIndex + index));
            var linesToRedraw = 
                collapsingAlgorithm.GetLinesToRedrawAfterCollapse(visuals.ToEnumerableOf<VisualTextLine>().ToList(), (VisualTextLine)visuals[message.Area.StartPosition.Line], message.Area.StartPosition.Line);

            visuals.RemoveRange(collapseIndex, Info.LinesCount - collapseIndex);
            
            foreach (var line in expandedLines) {
                visuals.Insert(line.Index, line);
                line.Draw();
            }

            AddLines(linesToRedraw);
            UpdateSize();
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
            var newLines = updatingAlgorithm.GetChangeInLines(Info.GetActualLines(), ActivePosition, enteredText);

            UpdateActivePosition(enteredText);
            DrawLines(newLines.Select(entry => VisualTextLine.Create(entry.Value, entry.Key)));
        }

        private void DeleteText(ChangeInLinesInfo removalInfo) {
            RemoveLines(removalInfo.LinesToRemove);
            UpdateActivePosition(removalInfo.LinesToChange.First().Key);
            DrawLines(removalInfo.LinesToChange.Select(pair => VisualTextLine.Create(pair.Value, pair.Key.Line)));
        }

        private void DeleteLines(IReadOnlyCollection<int> linesToRemove) {
            RemoveLines(linesToRemove);

            int minLine = linesToRemove.Min() - 1;
            int textLen = Info.GetActualLines()[minLine].Length;

            UpdateActivePosition(new TextPosition(column: textLen > 0 ? textLen - 1 : 0, line: minLine));
        }

        private void UpdateSize() {
            int maxLineLen = 0;

            foreach (var visual in visuals) {
                var line = (VisualTextLine)visual;

                if (line.RenderedText.Length > maxLineLen) {
                    maxLineLen = line.RenderedText.Length;
                }
            }

            Width = maxLineLen * TextConfiguration.GetCharSize().Width;
            Height = Convert.ToInt32(visuals.Count * TextConfiguration.GetCharSize().Height);
        }

        private void UpdateActivePosition(TextPosition position) => ActivePosition = new TextPosition(column: position.Column, line: position.Line);

        private void UpdateActivePosition(string text) {
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

        #endregion

    }
}
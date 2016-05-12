using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Core.DataStructures;
using CodeEditor.Algorithms.TextManipulation;
using CodeEditor.Configuration;
using CodeEditor.Views.BaseClasses;
using CodeEditor.Messaging;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Visuals;
using CodeEditor.TextProperties;
using CodeEditor.Algorithms.Parsing;
using CodeEditor.Core.Extensions;
using CodeEditor.Views.Caret;

namespace CodeEditor.Views.Text {
    /// <summary>
    /// provides core functionality for entering text
    /// </summary>
    internal partial class TextView : InputViewBase {

        #region fields

        private TextUpdatingAlgorithm updatingAlgorithm;

        private TextRemovingAlgorithm removingAlgorithm;

        private TextCollapsingAlgorithm collapsingAlgorithm;

        private TextParsingAlgorithm parsingAlgorithm;

        private ICaretViewReadonly caretViewReader;

        #endregion

        #region constructor

        public TextView(ICaretViewReadonly caretViewReader) : base() {
            updatingAlgorithm = new TextUpdatingAlgorithm();
            removingAlgorithm = new TextRemovingAlgorithm();
            collapsingAlgorithm = new TextCollapsingAlgorithm();
            parsingAlgorithm = new TextParsingAlgorithm();
            this.caretViewReader = caretViewReader;

            visuals.Add(new SingleVisualTextLine(new SimpleTextSource(string.Empty, TextConfiguration.GetGlobalTextRunProperties()), 0));
        }

        #endregion

        #region event handlers

        public void HandleGotFocus(object sender, RoutedEventArgs e) {
            Focus();

            e.Handled = true;
        }

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
            var removalInfo = removingAlgorithm.GetChangeInLines(GetVisualLines(), caretViewReader.CaretPosition, key);

            if (removalInfo.LinesToChange.Any()) {
                DeleteText(removalInfo);
                UpdateSize();
            } else if (removalInfo.LinesToRemove.Any()) {
                DeleteLines(removalInfo.LinesToRemove);
                UpdateSize();
            }
        }

        public void RemoveText(TextRange range) {
            var removalInfo = removingAlgorithm.GetChangeInLines(GetVisualLines(), range);

            if (removalInfo.LinesToChange.Any()) {
                DeleteText(removalInfo);
                UpdateSize();
            }
        }

        public void CollapseText(FoldClickedMessage message) {
            var algorithm = CollapseRepresentationAlgorithmFactory.GetAlgorithm(ConfigManager.GetConfig(EditorCode).FormattingType);
            var collapsedLine = collapsingAlgorithm.CollapseTextRange(message.AreaBeforeFolding, GetScreenLines(), algorithm);
            var linesToRedraw = collapsingAlgorithm.GetLinesToRedrawAfterCollapse(visuals.ToEnumerableOf<VisualTextLine>().ToList(), collapsedLine, message.AreaBeforeFolding);

            if (message.AreaBeforeFolding.StartPosition.Line != message.AreaBeforeFolding.EndPosition.Line) {
                visuals.RemoveRange(message.AreaBeforeFolding.StartPosition.Line, visuals.Count - (message.AreaBeforeFolding.StartPosition.Line + 1));
            }

            RedrawCollapsedLine(collapsedLine, message.AreaBeforeFolding.StartPosition.Line);
            AddLines(linesToRedraw);
            UpdateSize();
        }

        public void ExpandText(FoldClickedMessage message) {
            int collapseIndex = message.AreaBeforeFolding.StartPosition.Line;
            var collapsedLineContent = ((VisualTextLine)visuals[collapseIndex]).GetStringContents();
            var expandedLines = collapsedLineContent.Select((line, index) => VisualTextLine.Create(line, collapseIndex + index));
            var linesToRedraw = collapsingAlgorithm.GetLinesToRedrawAfterExpand(visuals.ToEnumerableOf<VisualTextLine>().Where(line => line.Index > message.AreaBeforeFolding.StartPosition.Line), expandedLines.Count() - 1);

            visuals.RemoveRange(collapseIndex, LinesCount - collapseIndex);
            
            foreach (var line in expandedLines) {
                visuals.Insert(line.Index, line);
                line.Draw();
            }

            AddLines(linesToRedraw);
            UpdateSize();
        }

        #endregion

        #region methods

        private void InputText(string enteredText) {
            var newLines = updatingAlgorithm.GetChangeInLines(GetActualLines(), caretViewReader.CaretPosition, enteredText);

            DrawLines(newLines.Select(entry => VisualTextLine.Create(entry.Value, entry.Key)));
        }

        private void DeleteText(ChangeInLinesInfo removalInfo) {
            RemoveLines(removalInfo.LinesToRemove);
            DrawLines(removalInfo.LinesToChange.Select(pair => pair.Value));
        }

        private void DeleteLines(IReadOnlyCollection<int> linesToRemove) => RemoveLines(linesToRemove);

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
        
        #endregion

    }
}
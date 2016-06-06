using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Algorithms.TextManipulation;
using CodeEditor.Algorithms.Parsing;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
using CodeEditor.TextProperties;
using CodeEditor.Views.BaseClasses;
using CodeEditor.Views.Caret;
using CodeEditor.Visuals;
using CodeEditor.Visuals.Base;

namespace CodeEditor.Views.Text {
    /// <summary>
    /// provides core functionality for entering text
    /// </summary>
    internal partial class TextView : InputViewBase {

        #region fields

        private readonly TextUpdatingAlgorithm updatingAlgorithm;

        private readonly TextRemovingAlgorithm removingAlgorithm;

        private readonly TextCollapsingAlgorithm collapsingAlgorithm;

        private TextParsingAlgorithm parsingAlgorithm;

        private readonly ICaretViewReadonly caretViewReader;

        #endregion

        #region constructor

        public TextView(ICaretViewReadonly caretViewReader) {
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
            var maxLineLen = (from VisualTextLine line in visuals select line.RenderedText.Length).Concat(new[] {0}).Max();
            var charHeight = TextConfiguration.GetCharSize().Height;

            Width = maxLineLen * TextConfiguration.GetCharSize().Width;
            Height = Convert.ToInt32(visuals.Count * charHeight);
        }
        
        #endregion

    }
}
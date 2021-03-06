﻿using System;
using System.Windows.Input;
using CodeEditor.Algorithms.Selection;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class TextSelectionCommand : ICommand {

        #region fields

        private readonly ITextViewReadonly textViewReader;

        private readonly CaretView caretView;

        private readonly SelectionView selectionView;

        private readonly TextSelectionAlgorithm algorithm;
       
        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public TextSelectionCommand(ITextViewReadonly textViewReader, SelectionView selectionView, CaretView caretView) {
            this.textViewReader = textViewReader;
            this.selectionView = selectionView;
            this.caretView = caretView;
            algorithm = new TextSelectionAlgorithm(caretView, textViewReader, selectionView);
        }

        #endregion

        #region public methods

        public bool CanExecute(object parameter) {
            var keyboardEvent = parameter as KeyEventArgs;
            var mouseClickEvent = parameter as MouseButtonEventArgs;
            var mouseMoveEvent = parameter as MouseEventArgs;

            if (keyboardEvent != null) {
                return CanExecuteKeyboard(keyboardEvent);
            }
            if (mouseClickEvent != null) {
                return CanExecuteMouse(mouseClickEvent);
            }
            if (mouseMoveEvent != null) {
                return CanExecuteMouse(mouseMoveEvent);
            }

            return false;
        }

        public void Execute(object parameter) {
            var keyboardEvent = parameter as KeyEventArgs;
            var mouseClickEvent = parameter as MouseButtonEventArgs;
            var mouseMoveEvent = parameter as MouseEventArgs;

            if (keyboardEvent != null) {
                ExecuteKeyboard(keyboardEvent);

                keyboardEvent.Handled = true;
            } else if (mouseClickEvent != null) {
                ExecuteMouse(mouseClickEvent);
            } else if (mouseMoveEvent != null) {
                ExecuteMouse(mouseMoveEvent);
            }
        }

        #endregion

        #region methods

        private bool CanExecuteKeyboard(KeyEventArgs keyboardEvent) {
            if (Keyboard.IsKeyDown(Key.RightShift) && keyboardEvent.Key == Key.Left && caretView.CaretPosition.Column == 0) {
                return false;
            }
            if (Keyboard.IsKeyDown(Key.RightShift) && keyboardEvent.Key == Key.Right && caretView.CaretPosition.Column == textViewReader.GetLineLength(caretView.CaretPosition.Line)) {
                return false;
            }
            if (Keyboard.IsKeyDown(Key.RightShift) && keyboardEvent.Key == Key.Up && caretView.CaretPosition.Line == 0) {
                return false;
            }
            if (Keyboard.IsKeyDown(Key.RightShift) && keyboardEvent.Key == Key.Down && caretView.CaretPosition.Line == textViewReader.LinesCount - 1) {
                return false;
            }

            return IsByCharSelectionRequested(keyboardEvent) || IsByWordSelectionRequested(keyboardEvent);
        }

        private bool IsByCharSelectionRequested(KeyEventArgs keyboardEvent) =>
            Keyboard.IsKeyDown(Key.RightShift) &&
            (keyboardEvent.Key == Key.Left || keyboardEvent.Key == Key.Right ||
            keyboardEvent.Key == Key.Up || keyboardEvent.Key == Key.Down ||
            keyboardEvent.Key == Key.Home || keyboardEvent.Key == Key.End ||
            keyboardEvent.Key == Key.PageUp || keyboardEvent.Key == Key.PageDown);

        private bool IsByWordSelectionRequested(KeyEventArgs keyboardEvent) =>
            Keyboard.IsKeyDown(Key.RightShift) && Keyboard.IsKeyDown(Key.RightCtrl) &&
                (keyboardEvent.Key == Key.Right || keyboardEvent.Key == Key.Left);

        private bool CanExecuteMouse(MouseButtonEventArgs mouseEvent) => mouseEvent.ClickCount == 3 || mouseEvent.ClickCount == 2;

        private bool CanExecuteMouse(MouseEventArgs mouseEvent) =>
            mouseEvent.LeftButton == MouseButtonState.Pressed &&
                textViewReader.IsInTextRange(mouseEvent.GetPosition(caretView).GetDocumentPosition(TextConfiguration.GetCharSize()));

        private void ExecuteKeyboard(KeyEventArgs keyboardEvent) {
            var endingPosition = algorithm.GetSelectionPosition(keyboardEvent);

            if (!selectionView.HasSelection()) {
                selectionView.Select(new TextPosition(column: caretView.CaretPosition.Column, line: caretView.CaretPosition.Line));
            }

            selectionView.Select(endingPosition);
            caretView.MoveCursor(endingPosition);
        }

        private void ExecuteMouse(MouseButtonEventArgs mouseEvent) {
            if (mouseEvent.ClickCount == 2) {
                var selectionInfo = algorithm.WordSelection(mouseEvent);

                selectionView.Select(selectionInfo.StartPosition);
                selectionView.Select(selectionInfo.EndPosition);
                caretView.MoveCursor(selectionInfo.CursorPosition);
            } else if (mouseEvent.ClickCount == 3) {
                var selectionInfo = algorithm.LineSelection(mouseEvent);

                selectionView.Select(selectionInfo.StartPosition);
                selectionView.Select(selectionInfo.EndPosition);
                caretView.MoveCursor(selectionInfo.CursorPosition);
            }
        }

        private void ExecuteMouse(MouseEventArgs mouseEvent) {
            var endPosition = algorithm.StandardSelection(mouseEvent);

            selectionView.Select(endPosition);
            caretView.MoveCursor(endPosition);
        }

        #endregion

    }
}

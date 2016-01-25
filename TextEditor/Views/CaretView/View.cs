using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using TextEditor.Configuration;
using TextEditor.DataStructures;
using TextEditor.Events;
using TextEditor.Extensions;

namespace TextEditor.Views.CaretView {
    public class View : ViewBase {

        #region constants

        const int BLINK_INTERVAL = 600;

        #endregion

        #region fields

        private DispatcherTimer blinkTimer;
        private Timer checkTimer;
        private TextPosition caretPosition;
        private TextRunProperties textRunProperties;
        private bool isCaretVisible;

        #endregion

        #region events

        public event CaretMovedEventHandler CaretMoved;

        #endregion

        #region properties

        public HashSet<Key> StepKeys { get; set; }

        public HashSet<Key> JumpKeys { get; set; }

        public TextPosition CaretPosition {
            get { return caretPosition; }
            private set { caretPosition = value; }
        }
        
        #endregion

        #region constructor

        public View() : base() {
            StepKeys = new HashSet<Key>(new[] { Key.Left, Key.Right, Key.Up, Key.Down });
            JumpKeys = new HashSet<Key>(new[] { Key.End, Key.Home, Key.PageUp, Key.PageDown });
            textRunProperties = this.CreateGlobalTextRunProperties();
            caretPosition = new TextPosition(0, 0);
            isCaretVisible = true;

            InitBlinker();
            DrawCaret();
        }

        #endregion

        #region event handlers

        public void HandleTextChange(object sender, TextChangedEventArgs e) => MoveCaret(new TextPosition { Column = e.CurrentColumn, Line = e.CurrentLine });

        public void HandleMouseDown(object sender, MouseButtonEventArgs e) { }

        #endregion

        #region public methods

        public void MoveCursor(TextPosition newPos) {
            var oldPos = caretPosition;

            MoveCaret(newPos);
            TriggerCaretMoved(newPos, oldPos);
        }

        public TextPosition GetNextPosition(Key key) {
            TextPosition coordinates;
            int column = 0;
            int line = 0;

            if (IsStep(key)) {
                coordinates = GetStep(key);
                column = caretPosition.Column + coordinates.Column;
                line = caretPosition.Line + coordinates.Line;
            } else {
                coordinates = GetJump(key);
                column = coordinates.Column;
                line = coordinates.Line;

                if (coordinates.Column == int.MinValue) {
                    column = 0;
                }
            }

            return new TextPosition {
                Column = column,
                Line = line
            };
        }

        public bool IsMoveRequested(KeyEventArgs e) => StepKeys.Contains(e.Key) || JumpKeys.Contains(e.Key);

        #endregion

        #region methods

        private void DrawCaret() {
            blinkTimer.Stop();
            checkTimer?.Dispose();

            var caret = new VisualElement { Position = caretPosition };

            caret.Draw(textRunProperties);
            visuals.Clear();
            visuals.Add(caret);

            isCaretVisible = true;
        }

        private void TriggerCaretMoved(TextPosition newPosition, TextPosition oldPosition) {
            CaretMoved(this, new CaretMovedEventArgs {
                NewPosition = newPosition,
                OldPosition = oldPosition
            });
        }

        private void MoveCaret(TextPosition position) {
            caretPosition.Column = position.Column;
            caretPosition.Line = position.Line;

            DrawCaret();
            RestartCheckTimer();
        }

        private bool IsStep(Key key) {
            return StepKeys.Contains(key);
        }

        private TextPosition GetStep(Key key) {
            int x = 0;
            int y = 0;

            switch (key) {
                case Key.Left: x = -1; break;
                case Key.Right: x = 1; break;
                case Key.Up: y = -1; break;
                case Key.Down: y = 1; break;
                default: break;
            }

            return new TextPosition(x, y);
        }

        private TextPosition GetJump(Key key) {
            int x = caretPosition.Column;
            int y = caretPosition.Line;

            switch (key) {
                case Key.Home: x = 0; break;
                case Key.End: x = int.MaxValue; break;
                case Key.PageUp: y = -GlobalConstants.PageSize; break;
                case Key.PageDown: y = GlobalConstants.PageSize; break;
                default: break;
            }

            return new TextPosition(x, y);
        }

        private void InitBlinker() {
            blinkTimer = new DispatcherTimer();
            blinkTimer.Tick += (sender, e) => {
                Application.Current.Dispatcher.Invoke(() => {
                    if (isCaretVisible) {
                        var caret = new VisualElement { Position = caretPosition };

                        caret.Draw(textRunProperties);
                        visuals.Add(caret);
                    } else {
                        visuals.Clear();
                    }

                    isCaretVisible = !isCaretVisible;
                });
            };
            blinkTimer.Interval = new TimeSpan(0, 0, 0, 0, BLINK_INTERVAL);
        }

        private void RestartCheckTimer() {
            bool wasTimerNull = checkTimer == null;

            checkTimer = new Timer((obj) => {
                isCaretVisible = false;

                blinkTimer.Start();
                checkTimer.Dispose();
            }, null, wasTimerNull ? 0 : BLINK_INTERVAL, Timeout.Infinite);
        }

        #endregion

    }
}

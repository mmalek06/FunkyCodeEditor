using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
using CodeEditor.Enums;
using CodeEditor.Messaging;
using CodeEditor.Events;
using CodeEditor.Views.BaseClasses;
using CodeEditor.Views.Text;

namespace CodeEditor.Views.Caret {
    internal partial class CaretView : InputViewBase {

        #region constants

        const int BLINK_INTERVAL = 600;

        #endregion

        #region fields

        private DispatcherTimer blinkTimer;

        private Timer checkTimer;

        private bool isCaretVisible;

        #endregion

        #region properties

        public HashSet<Key> StepKeys { get; set; }

        public HashSet<Key> JumpKeys { get; set; }

        public ITextViewReadonly TextViewReader { get; set; }

        #endregion

        #region constructor

        public CaretView() {
            StepKeys = new HashSet<Key>(new[] { Key.Left, Key.Right, Key.Up, Key.Down });
            JumpKeys = new HashSet<Key>(new[] { Key.End, Key.Home, Key.PageUp, Key.PageDown });
            CaretPosition = TextPosition.Zero;
            isCaretVisible = true;
            
            InitBlinker();
            DrawCaret();
        }

        #endregion

        #region event handlers

        public void HandleTextChange(string newText) => MoveCaret(GetNewCaretPosition(newText));

        public void HandleTextChange(object sender, TextChangedEventArgs e) => MoveCaret(new TextPosition(column: e.CurrentColumn, line: e.CurrentLine));

        public void HandleTextRemove(TextPosition newPosition) => MoveCaret(newPosition);

        public override void HandleTextFolding(FoldClickedMessage message) {
            if (message.State == FoldingStates.FOLDED) {
                if (IsCaretInbetweenTags(message.AreaAfterFolding)) {
                    MoveCaret(message.AreaAfterFolding.EndPosition);
                } else if (IsFoldMultiline(message.AreaBeforeFolding)) {
                    MoveCaret(message.AreaAfterFolding.EndPosition);
                }
            } else {
                if (IsCaretInbetweenTags(message.AreaBeforeFolding)) {
                    MoveCaret(new TextPosition(column: message.AreaAfterFolding.EndPosition.Column + 1, line: message.AreaAfterFolding.EndPosition.Line));
                }
            }
        }

        #endregion

        #region public methods

        public void MoveCursor(TextPosition newPos) {
            var oldPos = CaretPosition;

            MoveCaret(newPos);
        }

        #endregion

        #region methods

        private void DrawCaret() {
            blinkTimer.Stop();
            checkTimer?.Dispose();

            var caret = new VisualElement();

            caret.Draw(CaretPosition);
            visuals.Clear();
            visuals.Add(caret);

            isCaretVisible = true;
        }

        private void MoveCaret(TextPosition position) {
            CaretPosition = position;

            DrawCaret();
            RestartCheckTimer();
        }

        private bool IsStep(Key key) {
            return StepKeys.Contains(key);
        }

        private TextPosition GetStep(Key key) {
            var x = 0;
            var y = 0;

            switch (key) {
                case Key.Left: x = -1; break;
                case Key.Right: x = 1; break;
                case Key.Up: y = -1; break;
                case Key.Down: y = 1; break;
            }

            return new TextPosition(x, y);
        }

        private TextPosition GetJump(Key key) {
            var x = CaretPosition.Column;
            var y = CaretPosition.Line;

            switch (key) {
                case Key.Home: x = 0; break;
                case Key.End: x = int.MaxValue; break;
                case Key.PageUp: y = -GlobalConstants.PageSize; break;
                case Key.PageDown: y = GlobalConstants.PageSize; break;
            }

            return new TextPosition(x, y);
        }

        private TextPosition GetNewCaretPosition(string text) {
            var specialCharsRegex = new Regex("[\a|\b|\n|\r|\f|\t|\v]");
            var replacedText = specialCharsRegex.Replace(text, string.Empty);
            var column = -1;
            var line = -1;

            if (text == TextProperties.Properties.NEWLINE) {
                column = 0;
                line = CaretPosition.Line + 1;
            } else if (text == TextProperties.Properties.TAB) {
                column = CaretPosition.Column + TextProperties.Properties.TabSize;
            } else if (replacedText.Length == 1) {
                column = CaretPosition.Column + 1;
            } else {
                var parts = text.Split(TextProperties.Properties.NEWLINE[0]);

                column = parts.Last().Length;
                line = CaretPosition.Line + parts.Length - 1;
            }

            return new TextPosition(column: column > -1 ? column : CaretPosition.Column, line: line > -1 ? line : CaretPosition.Line);
        }

        private void InitBlinker() {
            blinkTimer = new DispatcherTimer();
            blinkTimer.Tick += (sender, e) => {
                Application.Current.Dispatcher.Invoke((Action)(() => {
                    if (isCaretVisible) {
                        var caret = new VisualElement();

                        caret.Draw(CaretPosition);
                        visuals.Add(caret);
                    } else {
                        visuals.Clear();
                    }

                    isCaretVisible = !isCaretVisible;
                }));
            };
            blinkTimer.Interval = new TimeSpan(0, 0, 0, 0, BLINK_INTERVAL);
        }

        private void RestartCheckTimer() {
            var wasTimerNull = checkTimer == null;

            checkTimer = new Timer((obj) => {
                isCaretVisible = false;

                blinkTimer.Start();
                checkTimer.Dispose();
            }, null, wasTimerNull ? 0 : BLINK_INTERVAL, Timeout.Infinite);
        }

        #endregion

    }
}

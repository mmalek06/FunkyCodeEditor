﻿using System.Windows.Input;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class CaretMoveCommandSteps {

        #region steps

        [When(@"I move caret to column number '(.*)' in line '(.*)'")]
        public void WhenIMoveCaretToColumnNumberInLine(int column, int line) {
            if (Common.Context.CaretView.CaretPosition.Line < line) {
                // move up
                var stopAt = line - Common.Context.CaretView.CaretPosition.Line;

                for (var i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Down);
                }
            } else if (Common.Context.CaretView.CaretPosition.Line > line) {
                // move down
                var stopAt = Common.Context.CaretView.CaretPosition.Line - line;

                for (var i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Up);
                }
            }
            if (Common.Context.CaretView.CaretPosition.Column < column) {
                // move right
                var stopAt = column - Common.Context.CaretView.CaretPosition.Column;

                for (var i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Right);
                }
            } else if (Common.Context.CaretView.CaretPosition.Column > column) {
                // move left
                var stopAt = Common.Context.CaretView.CaretPosition.Column - column;

                for (var i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Left);
                }
            }
        }

        #endregion

        #region helper methods

        private void MoveCaret(Key key) {
            var evtArgs = EventGenerator.CreateKeyEventArgs(key);

            if (Common.Context.CaretMoveCommand.CanExecute(evtArgs)) {
                Common.Context.CaretMoveCommand.Execute(evtArgs);
            }
        }

        #endregion

    }
}

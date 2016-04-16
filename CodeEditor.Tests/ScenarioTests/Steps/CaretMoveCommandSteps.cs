using System.Windows.Input;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class CaretMoveCommandSteps {

        #region fields

        private EditorContext ctx;

        #endregion

        #region constructor

        public CaretMoveCommandSteps(EditorContext context) {
            ctx = context;
        }

        #endregion

        #region steps

        [When(@"I move caret to column number '(.*)' in line '(.*)'")]
        public void WhenIMoveCaretToColumnNumberInLine(int column, int line) {
            if (ctx.TextView.ActivePosition.Line < line) {
                // move up
                int stopAt = line - ctx.TextView.ActivePosition.Line;

                for (int i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Down);
                }
            } else if (ctx.TextView.ActivePosition.Line > line) {
                // move down
                int stopAt = ctx.TextView.ActivePosition.Line - line;

                for (int i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Up);
                }
            }
            if (ctx.TextView.ActivePosition.Column < column) {
                // move right
                int stopAt = column - ctx.TextView.ActivePosition.Column;

                for (int i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Right);
                }
            } else if (ctx.TextView.ActivePosition.Column > column) {
                // move left
                int stopAt = ctx.TextView.ActivePosition.Column - column;

                for (int i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Left);
                }
            }
        }

        #endregion

        #region helper methods

        private void MoveCaret(Key key) {
            var evtArgs = EventGenerator.CreateKeyEventArgs(key);

            if (ctx.CaretMoveCommand.CanExecute(evtArgs)) {
                ctx.CaretMoveCommand.Execute(evtArgs);
            }
        }

        #endregion

    }
}

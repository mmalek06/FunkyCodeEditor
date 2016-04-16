using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class CommonSteps {

        #region fields

        private EditorContext ctx;

        #endregion

        #region constructor

        public CommonSteps(EditorContext context) {
            ctx = context;
        }

        #endregion

        #region steps

        [Given(@"Text to enter is '(.*)'")]
        public void GivenTextToEnterIs(string textToEnter) {
            ctx.TextsToEnter.Add(textToEnter);
        }

        [Given(@"Text to enter is newline")]
        public void GivenTextToEnterIsNewline() {
            ctx.TextsToEnter.Add("\r");
        }

        [When(@"I enter text")]
        public void WhenIEnterText() {
            foreach (string text in ctx.TextsToEnter) {
                foreach (char letter in text) {
                    var evtArgs = EventGenerator.CreateTextCompositionEventArgs(letter.ToString());

                    if (ctx.EnterTextCommand.CanExecute(evtArgs)) {
                        ctx.EnterTextCommand.Execute(evtArgs);
                    }
                }
            }
        }

        [When(@"I move caret to column number '(.*)' in line '(.*)'")]
        public void WhenIMoveCaretToColumnNumberInLine(int column, int line) {
            if (ctx.TextInfo.ActivePosition.Line < line) {
                // move up
                int stopAt = line - ctx.TextInfo.ActivePosition.Line;

                for (int i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Up);
                }
            } else if (ctx.TextInfo.ActivePosition.Line > line) {
                // move down
                int stopAt = ctx.TextInfo.ActivePosition.Line - line;

                for (int i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Down);
                }
            }
            if (ctx.TextInfo.ActivePosition.Column < column) {
                // move right
                int stopAt = column - ctx.TextInfo.ActivePosition.Column;

                for (int i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Right);
                }
            } else if (ctx.TextInfo.ActivePosition.Column > column) {
                // move left
                int stopAt = ctx.TextInfo.ActivePosition.Column - column;

                for (int i = 0; i < stopAt; i++) {
                    MoveCaret(Key.Left);
                }
            }
        }

        [When(@"I hit enter key")]
        public void WhenIHitEnterKey() {
            var evtArgs = EventGenerator.CreateTextCompositionEventArgs("\r");

            if (ctx.EnterTextCommand.CanExecute(evtArgs)) {
                ctx.EnterTextCommand.Execute(evtArgs);
            }
        }

        [Then(@"I should see '(.*)' lines")]
        public void ThenIShouldSeeLines(int numberOfVisibleLines) {
            var actualLinesCount = ctx.TextInfo.GetScreenLines().Count;

            Assert.AreEqual(numberOfVisibleLines, actualLinesCount);
        }

        [Then(@"The '(.*)' line should be equal to '(.*)'")]
        public void ThenTheLineShouldBeEqualTo(int lineNo, string text) {
            var actualLines = ctx.TextInfo.GetScreenLines();

            Assert.AreEqual(text, actualLines[lineNo]);
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

using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class EnterTextCommandSteps {

        #region fields

        private EditorContext ctx;

        #endregion

        #region constructor

        public EnterTextCommandSteps(EditorContext context) {
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
                foreach (char character in text) {
                    var evtArgs = EventGenerator.CreateTextCompositionEventArgs(character.ToString());

                    if (ctx.EnterTextCommand.CanExecute(evtArgs)) {
                        ctx.EnterTextCommand.Execute(evtArgs);
                        ctx.LinesView.HandleTextInput(character.ToString(), ctx.TextView.ActivePosition);
                        ctx.FoldingView.HandleTextInput(character.ToString(), ctx.TextView.ActivePosition);
                    }
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

        #endregion

    }
}

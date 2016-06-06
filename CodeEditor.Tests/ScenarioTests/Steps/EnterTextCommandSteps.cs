using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class EnterTextCommandSteps {

        #region steps

        [Given(@"Text to enter is '(.*)'")]
        public void GivenTextToEnterIs(string textToEnter) {
            Common.Context.TextsToEnter.Add(textToEnter);
        }

        [Given(@"Text to enter is newline")]
        public void GivenTextToEnterIsNewline() {
            Common.Context.TextsToEnter.Add("\r");
        }

        [When(@"I enter text")]
        public void WhenIEnterText() {
            foreach (var text in Common.Context.TextsToEnter) {
                foreach (var character in text) {
                    var evtArgs = EventGenerator.CreateTextCompositionEventArgs(character.ToString());

                    if (Common.Context.EnterTextCommand.CanExecute(evtArgs)) {
                        Common.Context.EnterTextCommand.Execute(evtArgs);
                    }
                }
            }
        }

        [When(@"I hit enter key")]
        public void WhenIHitEnterKey() {
            var evtArgs = EventGenerator.CreateTextCompositionEventArgs("\r");

            if (Common.Context.EnterTextCommand.CanExecute(evtArgs)) {
                Common.Context.EnterTextCommand.Execute(evtArgs);
            }
        }

        #endregion

    }
}

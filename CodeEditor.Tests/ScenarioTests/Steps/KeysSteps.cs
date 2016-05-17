using System.Windows.Input;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class KeysSteps {

        #region steps

        [When(@"I hit delete key")]
        public void WhenIHitDeleteKey() {
            var key = Key.Delete;
            var evtArgs = EventGenerator.CreateKeyEventArgs(key);

            if (Common.Context.RemoveTextCommand.CanExecute(evtArgs)) {
                Common.Context.RemoveTextCommand.Execute(evtArgs);
            }
        }

        [When(@"I hit backspace key")]
        public void WhenIHitBackspaceKey() {
            var key = Key.Back;
            var evtArgs = EventGenerator.CreateKeyEventArgs(key);

            if (Common.Context.RemoveTextCommand.CanExecute(evtArgs)) {
                Common.Context.RemoveTextCommand.Execute(evtArgs);
            }
        }

        #endregion

    }
}

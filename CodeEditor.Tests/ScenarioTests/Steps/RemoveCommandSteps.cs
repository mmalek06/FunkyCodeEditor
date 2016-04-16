using System.Windows.Input;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class RemoveCommandSteps {

        #region fields

        private EditorContext ctx;

        #endregion

        #region constructor

        public RemoveCommandSteps(EditorContext context) {
            ctx = context;
        }

        #endregion

        #region steps

        [When(@"I hit delete key")]
        public void WhenIHitDeleteKey() {
            var evtArgs = EventGenerator.CreateKeyEventArgs(Key.Delete);

            if (ctx.RemoveTextCommand.CanExecute(evtArgs)) {
                ctx.RemoveTextCommand.Execute(evtArgs);
            }
        }

        [When(@"I hit backspace key")]
        public void WhenIHitBackspaceKey() {
            var evtArgs = EventGenerator.CreateKeyEventArgs(Key.Back);

            if (ctx.RemoveTextCommand.CanExecute(evtArgs)) {
                ctx.RemoveTextCommand.Execute(evtArgs);
            }
        }

        #endregion

    }
}

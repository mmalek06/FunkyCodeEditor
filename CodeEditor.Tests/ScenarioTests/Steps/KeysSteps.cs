using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class KeysSteps {

        #region fields

        private EditorContext ctx;

        private List<string> pressedModifierKeys;

        #endregion

        #region constructor

        public KeysSteps(EditorContext context) {
            ctx = context;
            pressedModifierKeys = new List<string>();
        }

        #endregion

        #region steps

        [When(@"I hit delete key")]
        public void WhenIHitDeleteKey() {
            var key = Key.Delete;
            var evtArgs = EventGenerator.CreateKeyEventArgs(key);

            if (ctx.RemoveTextCommand.CanExecute(evtArgs)) {
                ctx.RemoveTextCommand.Execute(evtArgs);
            }
        }

        [When(@"I hit backspace key")]
        public void WhenIHitBackspaceKey() {
            var key = Key.Back;
            var evtArgs = EventGenerator.CreateKeyEventArgs(key);

            if (ctx.RemoveTextCommand.CanExecute(evtArgs)) {
                ctx.RemoveTextCommand.Execute(evtArgs);
            }
        }

        #endregion

    }
}

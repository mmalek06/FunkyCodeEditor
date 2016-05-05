using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class Common {

        #region fields

        public static EditorContext Context { get; private set; }

        #endregion

        #region hooks

        [Before]
        public void BeforeScenario() {
            Context = new EditorContext();
        }

        [After]
        public void AfterScenario() {
            Context.RemoveMessages();
            Context = null;
        }

        #endregion

    }
}

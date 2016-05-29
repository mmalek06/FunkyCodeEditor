using CodeEditor.DataStructures;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class BracketsFoldingSteps {

        #region steps

        [When(@"I request folding for position starting at '(.*)' '(.*)'")]
        public void WhenIRequestFoldingForPositionStartingAtAndEndingAt(int columnStart, int lineStart) {
            PrivateMembersHelper.InvokeMethod(Common.Context.FoldingView, "RunFoldingOnClick", new[] { new TextPosition(column: columnStart, line: lineStart) });
        }

        [When(@"I request unfolding for position starting at '(.*)' '(.*)'")]
        public void WhenIRequestUnfoldingForPositionStartingAtAndEndingAt(int columnStart, int lineStart) {
            PrivateMembersHelper.InvokeMethod(Common.Context.FoldingView, "RunFoldingOnClick", new[] { new TextPosition(column: columnStart, line: lineStart) });
        }

        #endregion
        
    }
}

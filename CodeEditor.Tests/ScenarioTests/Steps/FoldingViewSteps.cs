using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class FoldingViewSteps {

        #region fields

        private EditorContext ctx;

        #endregion

        #region constructor

        public FoldingViewSteps(EditorContext context) {
            ctx = context;
        }

        #endregion

        #region steps

        [Then(@"I should see folding on position starting at '(.*)' '(.*)' and ending at '(.*)' '(.*)'")]
        public void ThenIShouldSeeFoldingOnPositionStartingAtAndEndingAt(int startingColumn, int startingLine, int endingColumn, int endingLine) {
            PrivateMembersHelper.GetPropertyValue(ctx.FoldingView, "foldingPositions");
        }

        #endregion

    }
}

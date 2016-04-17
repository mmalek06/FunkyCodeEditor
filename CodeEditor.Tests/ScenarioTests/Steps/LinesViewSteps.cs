using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class LinesViewSteps {

        #region fields

        private EditorContext ctx;

        #endregion

        #region constructor

        public LinesViewSteps(EditorContext context) {
            ctx = context;
        }

        #endregion

        #region steps

        [Then(@"Shown number of lines in the lines panel should be '(.*)'")]
        public void ThenShownNumberOfLinesInTheLinesPanelShouldBe(int linesNo) {
            int value = (int)PrivateMembersHelper.GetPropertyValue(ctx.LinesView, "VisualChildrenCount");

            Assert.AreEqual(linesNo, value);
        }

        #endregion

    }
}

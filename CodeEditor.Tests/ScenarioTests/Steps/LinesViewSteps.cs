using NUnit.Framework;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class LinesViewSteps {

        #region steps

        [Then(@"Shown number of lines in the lines panel should be '(.*)'")]
        public void ThenShownNumberOfLinesInTheLinesPanelShouldBe(int linesNo) {
            var value = (int)PrivateMembersHelper.GetPropertyValue(Common.Context.LinesView, "VisualChildrenCount");

            Assert.AreEqual(linesNo, value);
        }

        #endregion

    }
}

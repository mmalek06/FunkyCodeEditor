using System.Collections;
using System.Reflection;
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
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var fieldInfo = ctx.LinesView.GetType().GetField("visuals", bindFlags);
            var value = (fieldInfo.GetValue(ctx.LinesView) as ICollection).Count;

            Assert.AreEqual(linesNo, value);
        }

        #endregion

    }
}

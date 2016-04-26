using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class TextViewSteps {

        #region steps

        [Then(@"I should see '(.*)' lines")]
        public void ThenIShouldSeeLines(int numberOfVisibleLines) {
            var actualLinesCount = Common.Context.TextView.GetScreenLines().Count;

            Assert.AreEqual(numberOfVisibleLines, actualLinesCount);
        }

        [Then(@"The '(.*)' line should be equal to '(.*)'")]
        public void ThenTheLineShouldBeEqualTo(int lineNo, string text) {
            var actualLines = Common.Context.TextView.GetScreenLines();

            Assert.AreEqual(text, actualLines[lineNo]);
        }

        [Then(@"Cursor should be at '(.*)' '(.*)'")]
        public void ThenCursorShouldBeAt(int column, int line) {
            Assert.AreEqual(column, Common.Context.CaretView.CaretPosition.Column);
            Assert.AreEqual(line, Common.Context.CaretView.CaretPosition.Line);
        }

        #endregion

    }
}

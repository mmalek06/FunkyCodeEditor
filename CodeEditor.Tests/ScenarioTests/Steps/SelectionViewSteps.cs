using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class SelectionViewSteps {

        #region fields

        private EditorContext ctx;

        #endregion

        #region constructor

        public SelectionViewSteps(EditorContext context) {
            ctx = context;
        }

        #endregion

        #region steps

        [When(@"I select text from '(.*)' '(.*)' to '(.*)' '(.*)'")]
        public void WhenISelectTextFromTo(int columnStart, int lineStart, int columnEnd, int lineEnd) {
            ctx.SelectionView.Select(new Core.DataStructures.TextPosition(column: columnStart, line: lineStart));
            ctx.SelectionView.Select(new Core.DataStructures.TextPosition(column: columnEnd, line: lineEnd));
        }

        [Then(@"Selected text should be '(.*)'")]
        public void ThenSelectedTextShouldBe(string text) {
            var selectionArea = ctx.SelectionView.GetCurrentSelectionArea();
            var selectedParts = ctx.TextView.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition);

            Assert.AreEqual(text, string.Join("", selectedParts));
        }


        #endregion

    }
}

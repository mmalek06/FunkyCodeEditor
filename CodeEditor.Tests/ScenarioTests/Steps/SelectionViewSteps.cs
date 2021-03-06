﻿using NUnit.Framework;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class SelectionViewSteps {

        #region steps

        [When(@"I select text from '(.*)' '(.*)' to '(.*)' '(.*)'")]
        public void WhenISelectTextFromTo(int columnStart, int lineStart, int columnEnd, int lineEnd) {
            Common.Context.SelectionView.Select(new DataStructures.TextPosition(column: columnStart, line: lineStart));
            Common.Context.SelectionView.Select(new DataStructures.TextPosition(column: columnEnd, line: lineEnd));
        }

        [Then(@"Selected text should be '(.*)'")]
        public void ThenSelectedTextShouldBe(string text) {
            var selectionArea = Common.Context.SelectionView.GetCurrentSelectionArea();
            var selectedParts = Common.Context.TextView.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition);

            Assert.AreEqual(text, string.Join("", selectedParts));
        }


        #endregion

    }
}

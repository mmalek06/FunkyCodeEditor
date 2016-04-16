﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class TextViewSteps {

        #region fields

        private EditorContext ctx;

        #endregion

        #region constructor

        public TextViewSteps(EditorContext context) {
            ctx = context;
        }

        #endregion

        #region steps

        [Then(@"I should see '(.*)' lines")]
        public void ThenIShouldSeeLines(int numberOfVisibleLines) {
            var actualLinesCount = ctx.TextInfo.GetScreenLines().Count;

            Assert.AreEqual(numberOfVisibleLines, actualLinesCount);
        }

        [Then(@"The '(.*)' line should be equal to '(.*)'")]
        public void ThenTheLineShouldBeEqualTo(int lineNo, string text) {
            var actualLines = ctx.TextInfo.GetScreenLines();

            Assert.AreEqual(text, actualLines[lineNo]);
        }

        [Then(@"Cursor should be at '(.*)' '(.*)'")]
        public void ThenCursorShouldBeAt(int column, int line) {
            Assert.AreEqual(column, ctx.TextInfo.ActivePosition.Column);
            Assert.AreEqual(line, ctx.TextInfo.ActivePosition.Line);
        }

        #endregion

    }
}
using System.Collections.Generic;
using System.Linq;
using CodeEditor.Core.DataStructures;
using CodeEditor.Views.Folding;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class FoldingViewSteps {

        #region steps

        [Then(@"I should see folding on position starting at '(.*)' '(.*)' and ending at '(.*)' '(.*)'")]
        public void ThenIShouldSeeFoldingOnPositionStartingAtAndEndingAt(int startingColumn, int startingLine, int endingColumn, int endingLine) {
            var foldingPositions = (Dictionary<FoldingPositionInfo, FoldingPositionInfo>)PrivateMembersHelper.GetFieldValue(Common.Context.FoldingView, "foldingPositions");

            Assert.AreEqual(new TextPosition(column: startingColumn, line: startingLine), foldingPositions.First().Key.Position);
            Assert.AreEqual(new TextPosition(column: endingColumn, line: endingLine), foldingPositions.First().Value.Position);
        }

        [Then(@"I should see '(.*)' folding on position starting at '(.*)' '(.*)' and ending at '(.*)' '(.*)'")]
        public void ThenIShouldSeeFoldingOnPositionStartingAtAndEndingAt(int nthFolding, int startingColumn, int startingLine, int endingColumn, int endingLine) {
            var foldingPositions = (Dictionary<FoldingPositionInfo, FoldingPositionInfo>)PrivateMembersHelper.GetFieldValue(Common.Context.FoldingView, "foldingPositions");
            var orderedFoldingPositions = foldingPositions.OrderBy(info => info.Key.Position);

            nthFolding -= 1;

            Assert.AreEqual(new TextPosition(column: startingColumn, line: startingLine), orderedFoldingPositions.ElementAt(nthFolding).Key.Position);
            Assert.AreEqual(new TextPosition(column: endingColumn, line: endingLine), orderedFoldingPositions.ElementAt(nthFolding).Value.Position);
        }

        [Then(@"I should see no folding")]
        public void ThenIShouldSeeNoFolding() {
            var foldingPositions = (Dictionary<FoldingPositionInfo, FoldingPositionInfo>)PrivateMembersHelper.GetFieldValue(Common.Context.FoldingView, "foldingPositions");

            Assert.AreEqual(0, foldingPositions.Where(pair => !pair.Key.Deleted).Count());
        }

        #endregion

    }
}

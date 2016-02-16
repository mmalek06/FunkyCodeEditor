using CodeEditor.Algorithms.Folding;
using CodeEditor.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests {
    [TestClass]
    public class BracketsFoldingTests {
        private BracketsFoldingAlgorithm fa;
        private Views.TextView.View tv;
        private Views.TextView.TextInfo ti;

        [TestInitialize]
        public void InitializeTest() {
            tv = new Views.TextView.View();
            ti = new Views.TextView.TextInfo(tv);
            fa = new BracketsFoldingAlgorithm();
        }

        [TestMethod]
        public void StartFoldingAt_0_0_ShouldBeNoFolding() {
            fa.RecreateFolds("{", new TextPosition(column: 0, line: 0));
            fa.RecreateFolds("}", new TextPosition(column: 1, line: 0));
            
            Assert.AreEqual(1, fa.FoldingPositions.Count);
        }

        [TestMethod]
        public void StartFoldingAt_0_0_ShouldHaveOneFold() {
            fa.RecreateFolds("{", new TextPosition(column: 0, line: 0));
            fa.RecreateFolds("}", new TextPosition(column: 0, line: 1));

            Assert.AreEqual(1, fa.FoldingPositions.Count);
        }

        [TestMethod]
        public void StartFoldingAt_0_0_ShouldHaveTwoFolds() {
            fa.RecreateFolds("{", new TextPosition(column: 0, line: 0));
            fa.RecreateFolds("{", new TextPosition(column: 5, line: 1));
            fa.RecreateFolds("{", new TextPosition(column: 3, line: 3));
            fa.RecreateFolds("}", new TextPosition(column: 8, line: 3));
            fa.RecreateFolds("}", new TextPosition(column: 0, line: 4));
            fa.RecreateFolds("}", new TextPosition(column: 3, line: 5));

            Assert.AreEqual(3, fa.FoldingPositions.Count);
        }

        [TestMethod]
        public void ClosingBracketsInTheSameLine_ShouldHaveThreeFoldings() {
            fa.RecreateFolds("{", new TextPosition(column: 0, line: 0));
            fa.RecreateFolds("{", new TextPosition(column: 5, line: 1));
            fa.RecreateFolds("{", new TextPosition(column: 3, line: 3));
            fa.RecreateFolds("}", new TextPosition(column: 0, line: 4));
            fa.RecreateFolds("}", new TextPosition(column: 1, line: 4));
            fa.RecreateFolds("}", new TextPosition(column: 2, line: 4));

            Assert.AreEqual(3, fa.FoldingPositions.Count);
        }

        [TestMethod]
        public void OpeningBracketsInTheSameLine_ShouldHaveThreeFoldings() {
            fa.RecreateFolds("{", new TextPosition(column: 0, line: 0));
            fa.RecreateFolds("{", new TextPosition(column: 1, line: 0));
            fa.RecreateFolds("{", new TextPosition(column: 2, line: 0));
            fa.RecreateFolds("}", new TextPosition(column: 8, line: 3));
            fa.RecreateFolds("}", new TextPosition(column: 0, line: 4));
            fa.RecreateFolds("}", new TextPosition(column: 3, line: 5));

            Assert.AreEqual(3, fa.FoldingPositions.Count);
        }
    }
}

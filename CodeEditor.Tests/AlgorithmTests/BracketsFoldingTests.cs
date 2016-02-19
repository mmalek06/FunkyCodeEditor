using System.Collections.Generic;
using System.Linq;
using CodeEditor.Algorithms.Folding;
using CodeEditor.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests {
    [TestClass]
    public class BracketsFoldingTests {
        private BracketsFoldingAlgorithm fa;
        private Views.TextView.View tv;
        private Views.TextView.TextInfo ti;
        private Dictionary<TextPosition, TextPosition> foldingPositions;

        [TestInitialize]
        public void InitializeTest() {
            tv = new Views.TextView.View();
            ti = new Views.TextView.TextInfo(tv);
            fa = new BracketsFoldingAlgorithm();
            foldingPositions = new Dictionary<TextPosition, TextPosition>();
        }

        [TestMethod]
        public void EnterLetter_ShouldBeNoFolding() {
            var folds = fa.CreateFolds("I saw Susie in a shoe shine shop", new TextPosition(column: 0, line: 0), foldingPositions);

            Assert.IsNull(folds);
        }

        [TestMethod]
        public void StartFoldingAt_0_0_ShouldHaveOneFold() {
            Fold("{", 0, 0);

            int folds = Fold("{", 1, 0);

            Assert.AreEqual(1, folds);
        }

        [TestMethod]
        public void StartFoldingAt_0_0_ShouldHaveTwoFolds() {
            Fold("{", 0, 0);
            Fold("{", 5, 1);
            Fold("{", 3, 3);
            Fold("}", 8, 3);
            Fold("}", 0, 4);

            int folds = Fold("}", 3, 5);

            Assert.AreEqual(3, folds);
        }

        [TestMethod]
        public void ClosingBracketsInTheSameLine_ShouldHaveThreeFoldings() {
            Fold("{", 0, 0);
            Fold("{", 5, 1);
            Fold("{", 3, 3);
            Fold("}", 0, 4);
            Fold("}", 1, 4);

            int folds = Fold("}", 2, 4);

            Assert.AreEqual(3, folds);
        }

        [TestMethod]
        public void OpeningBracketsInTheSameLine_ShouldHaveThreeFoldings() {
            Fold("{", 0, 0);
            Fold("{", 1, 0);
            Fold("{", 2, 0);
            Fold("}", 8, 3);
            Fold("}", 0, 4);

            int folds = Fold("}", 3, 5);

            Assert.AreEqual(3, folds);
        }

        private int Fold(string bracket, int col, int line) {
            var folds = fa.CreateFolds(bracket, new TextPosition(column: col, line: line), foldingPositions);

            foreach (var kvp in folds) {
                foldingPositions[kvp.Key] = kvp.Value;
            }

            return folds.Count();
        }
    }
}

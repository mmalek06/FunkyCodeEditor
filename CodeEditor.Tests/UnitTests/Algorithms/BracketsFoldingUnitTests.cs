using System.Collections.Generic;
using System.Linq;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Core.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests.UnitTests.Algorithms {
    [TestClass]
    public class BracketsFoldingUnitTests {
        private BracketsFoldingAlgorithm fa;
        private Dictionary<TextPosition, TextPosition> foldingPositions;

        [TestInitialize]
        public void InitializeTest() {
            fa = new BracketsFoldingAlgorithm();
            foldingPositions = new Dictionary<TextPosition, TextPosition>();
        }

        [TestMethod]
        public void EnterSentence_ShouldBeNoFolding() {
            var folds = fa.CreateFolds("I saw Susie in a shoe shine shop", new TextPosition(column: 0, line: 0), foldingPositions);

            Assert.IsNull(folds);
        }
        
        [TestMethod]
        public void EnterTwoOpeningBrackets_ShouldHaveTwoEmptyFolds() {
            Fold("{", 0, 0);

            var folds = Fold("{", 1, 0);

            Assert.AreEqual(2, folds.Count);
            Assert.AreEqual(new TextPosition(column: 0, line: 0), folds[0].Key);
            Assert.IsNull(folds[0].Value);
            Assert.AreEqual(new TextPosition(column: 1, line: 0), folds[1].Key);
            Assert.IsNull(folds[1].Value);
        }

        [TestMethod]
        public void EnterOpeningAndClosingBrackets_ShouldHaveOneFold() {
            Fold("{", 0, 0);

            var folds = Fold("}", 0, 1);

            Assert.AreEqual(1, folds.Count);
            Assert.AreEqual(new TextPosition(column: 0, line: 0), folds[0].Key);
        }

        [TestMethod]
        public void StartFoldingAt_0_0_ShouldHaveThreeFolds() {
            Fold("{", 0, 0);
            Fold("{", 5, 1);
            Fold("{", 3, 3);
            Fold("}", 8, 3);
            Fold("}", 0, 4);

            var folds = Fold("}", 3, 5);

            Assert.AreEqual(3, folds.Count);
            Assert.AreEqual(new TextPosition(column: 0, line: 0), folds[0].Key);
            Assert.AreEqual(new TextPosition(column: 3, line: 5), folds[0].Value);
            Assert.AreEqual(new TextPosition(column: 5, line: 1), folds[1].Key);
            Assert.AreEqual(new TextPosition(column: 0, line: 4), folds[1].Value);
            Assert.AreEqual(new TextPosition(column: 3, line: 3), folds[2].Key);
            Assert.AreEqual(new TextPosition(column: 8, line: 3), folds[2].Value);
        }

        [TestMethod]
        public void ClosingBracketsInTheSameLine_ShouldHaveThreeFolds() {
            Fold("{", 0, 0);
            Fold("{", 5, 1);
            Fold("{", 3, 3);
            Fold("}", 0, 4);
            Fold("}", 1, 4);

            var folds = Fold("}", 2, 4);

            Assert.AreEqual(3, folds.Count);
            Assert.AreEqual(new TextPosition(column: 0, line: 0), folds[0].Key);
            Assert.AreEqual(new TextPosition(column: 2, line: 4), folds[0].Value);
            Assert.AreEqual(new TextPosition(column: 5, line: 1), folds[1].Key);
            Assert.AreEqual(new TextPosition(column: 1, line: 4), folds[1].Value);
            Assert.AreEqual(new TextPosition(column: 3, line: 3), folds[2].Key);
            Assert.AreEqual(new TextPosition(column: 0, line: 4), folds[2].Value);
        }

        [TestMethod]
        public void OpeningBracketsInTheSameLine_ShouldHaveThreeFolds() {
            Fold("{", 0, 0);
            Fold("{", 1, 0);
            Fold("{", 2, 0);
            Fold("}", 8, 3);
            Fold("}", 0, 4);

            var folds = Fold("}", 3, 5);

            Assert.AreEqual(3, folds.Count);
            Assert.AreEqual(new TextPosition(column: 0, line: 0), folds[0].Key);
            Assert.AreEqual(new TextPosition(column: 3, line: 5), folds[0].Value);
            Assert.AreEqual(new TextPosition(column: 1, line: 0), folds[1].Key);
            Assert.AreEqual(new TextPosition(column: 0, line: 4), folds[1].Value);
            Assert.AreEqual(new TextPosition(column: 2, line: 0), folds[2].Key);
            Assert.AreEqual(new TextPosition(column: 8, line: 3), folds[2].Value);
        }

        [TestMethod]
        public void ThreeBracketsEnteredOneNotPaired_ShouldHaveTwoFullFoldsAndOneEmpty() {
            Fold("{", 0, 0);
            Fold("{", 5, 1);
            Fold("{", 3, 3);
            Fold("}", 0, 4);

            var folds = Fold("}", 3, 5);

            Assert.AreEqual(3, folds.Count);
            Assert.AreEqual(new TextPosition(column: 0, line: 0), folds[0].Key);
            Assert.IsNull(folds[0].Value);
            Assert.AreEqual(new TextPosition(column: 5, line: 1), folds[1].Key);
            Assert.AreEqual(new TextPosition(column: 3, line: 5), folds[1].Value);
            Assert.AreEqual(new TextPosition(column: 3, line: 3), folds[2].Key);
            Assert.AreEqual(new TextPosition(column: 0, line: 4), folds[2].Value);            
        }

        [TestMethod]
        public void TwoBracketsAtTheSameLevel_ShouldHaveThreeFolds() {
            Fold("{", 0, 0);
            Fold("{", 0, 1);
            Fold("}", 0, 2);
            Fold("{", 0, 3);
            Fold("}", 0, 4);

            var folds = Fold("}", 0, 5);

            Assert.AreEqual(3, folds.Count);
            Assert.AreEqual(new TextPosition(column: 0, line: 0), folds[0].Key);
            Assert.AreEqual(new TextPosition(column: 0, line: 5), folds[0].Value);
            Assert.AreEqual(new TextPosition(column: 0, line: 1), folds[1].Key);
            Assert.AreEqual(new TextPosition(column: 0, line: 2), folds[1].Value);
            Assert.AreEqual(new TextPosition(column: 0, line: 3), folds[2].Key);
            Assert.AreEqual(new TextPosition(column: 0, line: 4), folds[2].Value);
        }

        [TestMethod]
        public void ClosingBracketAdded_ShouldBeNoFolding() {
            var folds = Fold("}", 0, 0);

            Assert.AreEqual(0, folds.Count);
        }

        private List<KeyValuePair<TextPosition, TextPosition>> Fold(string bracket, int col, int line) {
            var folds = fa.CreateFolds(bracket, new TextPosition(column: col, line: line), foldingPositions);

            foreach (var kvp in folds) {
                foldingPositions[kvp.Key] = kvp.Value;
            }

            return folds.OrderBy(kvp => kvp.Key).ToList();
        }
    }
}

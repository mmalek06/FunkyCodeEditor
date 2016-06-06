using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CodeEditor.Algorithms.TextManipulation;
using CodeEditor.DataStructures;
using CodeEditor.Visuals.Base;
using NUnit.Framework;

namespace CodeEditor.Tests.UnitTests.Algorithms {
    [TestFixture]
    public class RemovingTextUnitTests {
        private static TextRemovingAlgorithm algorithm;

        [OneTimeSetUp]
        public void Initialize() {
            algorithm = new TextRemovingAlgorithm();
        }

        [Test]
        public void TwoNonEmptyLinesEnteredDelPressedAtTheEndOfFirstOne_LineShouldBeEqualToText1Text2() {
            var text1 = "asdf";
            var text2 = "zxcv";
            var lines = new List<VisualTextLine> {
                VisualTextLine.Create(text1, 0),
                VisualTextLine.Create(text2, 1)
            };
            var newLines = algorithm.GetChangeInLines(lines, new TextPosition(column: 4, line: 0), Key.Delete);

            Assert.AreEqual(text1 + text2, newLines.LinesToChange.First().Value.GetStringContents()[0]);
        }

        [Test]
        public void TwoNonEmptyLinesEnteredBackspacePressedAtTheBeginningOfSecond_LineShouldBeEqualToText1Text2() {
            var text1 = "asdf";
            var text2 = "zxcv";
            var lines = new List<VisualTextLine> {
                VisualTextLine.Create(text1, 0),
                VisualTextLine.Create(text2, 1)
            };
            var newLines = algorithm.GetChangeInLines(lines, new TextPosition(column: 0, line: 1), Key.Back);

            Assert.AreEqual(text1 + text2, newLines.LinesToChange.First().Value.GetStringContents()[0]);
        }

        [Test]
        public void ThreeNonEmptyLinesEnteredBackspacePressedAtCharOneBeforeTheLastOneInTheLastLine_LinesShouldBeText4() {
            var text1 = "some text";
            var text2 = "";
            var text3 = "totally unimportant text";
            var text4 = "text that stays";
            var lines = new List<VisualTextLine> {
                VisualTextLine.Create(text1, 0),
                VisualTextLine.Create(text2, 1),
                VisualTextLine.Create(text3 + text4, 2)
            };
            var newLines = algorithm.GetChangeInLines(lines, new TextRange {
                StartPosition = new TextPosition(column: 0, line: 0),
                EndPosition = new TextPosition(column: 24, line: 2)
            });

            Assert.AreEqual(text4, newLines.LinesToChange.First().Value.GetStringContents()[0]);
            Assert.IsTrue(Enumerable.SequenceEqual((new[] { 0, 1, 2 }).OrderBy(key => key).ToArray(), newLines.LinesToRemove.OrderBy(key => key).ToArray()));
        }

        [Test]
        public void OneNonEmptyLineEnteredText2SelectedBackspacePressed_LineShouldBeText1Text3() {
            var text1 = "some ";
            var text2 = "text";
            var text3 = " asdf";
            var lines = new List<VisualTextLine> {
                VisualTextLine.Create(text1 + text2 + text3, 0)
            };
            var newLines = algorithm.GetChangeInLines(lines, new TextRange {
                StartPosition = new TextPosition(column: 5, line: 0),
                EndPosition = new TextPosition(column: 9, line: 0)
            });

            Assert.AreEqual(text1 + text3, newLines.LinesToChange.First().Value.GetStringContents()[0]);
        }

        [Test]
        public void ThreeLinesEnteredBackspacePressedOnTheSecondOne_LinesShouldBeText1Text3AndLinesToRemoveShouldBeLast() {
            var text1 = "a";
            var text2 = "";
            var text3 = "b";
            var lines = new List<VisualTextLine> {
                VisualTextLine.Create(text1, 0),
                VisualTextLine.Create(text2, 1),
                VisualTextLine.Create(text3, 2)
            };
            var newLines = algorithm.GetChangeInLines(lines, new TextPosition(column: 0, line: 1), Key.Back);

            Assert.IsTrue(Enumerable.SequenceEqual((new[] { 2 }).OrderBy(key => key).ToArray(), newLines.LinesToRemove.ToArray()));
            Assert.AreEqual(text1, newLines.LinesToChange.First().Value.GetStringContents()[0]);
            Assert.AreEqual(text3, newLines.LinesToChange.Last().Value.GetStringContents()[0]);
        }

        [Test]
        public void FourLinesEnteredDeletePressedAtTheEndOfFirst_LinesShouldBeText1Text2Text3() {
            var text1 = "asdf";
            var text2 = "zxcv";
            var text3 = "qwer";
            var lines = new List<VisualTextLine> {
                VisualTextLine.Create(text1, 0),
                VisualTextLine.Create(string.Empty, 1),
                VisualTextLine.Create(text2, 2),
                VisualTextLine.Create(text3, 3)
            };
            var newLines = algorithm.GetChangeInLines(lines, new TextPosition(column: 4, line: 0), Key.Delete);
            var removedLinesIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(text1, newLines.LinesToChange.First().Value.GetStringContents()[0]);
            Assert.AreEqual(text2, newLines.LinesToChange.ElementAt(1).Value.GetStringContents()[0]);
            Assert.AreEqual(text3, newLines.LinesToChange.ElementAt(2).Value.GetStringContents()[0]);
            Assert.AreEqual(1, removedLinesIndexes.Length);
            Assert.AreEqual(3, removedLinesIndexes[0]);
        }

        [Test]
        public void FourLinesEnteredDeletePressedTwiceAtTheEndOfFirst_LinesShouldBeText1PlusText2Text3() {
            var text1 = "asdf";
            var text2 = "zxcv";
            var text3 = "qwer";
            var lines = new List<VisualTextLine> {
                VisualTextLine.Create(text1, 0),
                VisualTextLine.Create(string.Empty, 1),
                VisualTextLine.Create(text2, 2),
                VisualTextLine.Create(text3, 3)
            };

            algorithm.GetChangeInLines(lines, new TextPosition(column: 4, line: 0), Key.Delete);

            var newLines = algorithm.GetChangeInLines(lines, new TextPosition(column: 4, line: 0), Key.Delete);
            var removedLinesIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(text1, newLines.LinesToChange.First().Value.GetStringContents()[0]);
            Assert.AreEqual(text2, newLines.LinesToChange.ElementAt(1).Value.GetStringContents()[0]);
            Assert.AreEqual(text3, newLines.LinesToChange.ElementAt(2).Value.GetStringContents()[0]);
            Assert.AreEqual(1, removedLinesIndexes.Length);
            Assert.AreEqual(3, removedLinesIndexes[0]);
        }

        [Test]
        public void OneLineEnteredSelectionOfFourCharsDeletePressed_LineShouldBeText1Text3() {
            var text1 = "asdf";
            var text2 = "qwer";
            var text3 = "zxcv";
            var lines = new List<VisualTextLine> {
                VisualTextLine.Create(text1 + text2 + text3, 0)
            };
            var newLines = algorithm.GetChangeInLines(lines, new TextRange {
                StartPosition = new TextPosition(column: 4, line: 0),
                EndPosition = new TextPosition(column: 8, line: 0)
            });
            var removedLinesIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(0, removedLinesIndexes.Length);
            Assert.AreEqual(text1 + text3, newLines.LinesToChange.ElementAt(0).Value.GetStringContents()[0]);
        }

        [Test]
        public void TwoLinesEnteredSelectionInTheMiddleOfFirst_LinesShouldBeText1Text3Text4() {
            var text1 = "as";
            var text2 = " df ";
            var text3 = "qwer";
            var text4 = "zxcv";
            var lines = new List<VisualTextLine> {
                VisualTextLine.Create(text1 + text2 + text3, 0),
                VisualTextLine.Create(text4, 1)
            };
            var newLines = algorithm.GetChangeInLines(lines, new TextRange {
                StartPosition = new TextPosition(column: 2, line: 0),
                EndPosition = new TextPosition(column: 6, line: 0)
            });
            var removedLineIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(0, removedLineIndexes.Length);
            Assert.AreEqual(1, newLines.LinesToChange.Count());
            Assert.AreEqual(text1 + text3, newLines.LinesToChange.ElementAt(0).Value.GetStringContents()[0]);
        }

        [Test]
        public void FourLinesEnteredSelectionInbetweenDeletePressed_LinesShouldBeText1Text6() {
            var text1 = "as";
            var text2 = "df";
            var text3 = "qwer";
            var text4 = "zxcv";
            var text5 = "fg";
            var text6 = "hj";
            var lines = new List<VisualTextLine> {
                VisualTextLine.Create(text1 + text2, 0),
                VisualTextLine.Create(text3, 1),
                VisualTextLine.Create(text4, 2),
                VisualTextLine.Create(text5 + text6, 3)
            };
            var newLines = algorithm.GetChangeInLines(lines, new TextRange {
                StartPosition = new TextPosition(column: 2, line: 0),
                EndPosition = new TextPosition(column: 2, line: 3)
            });
            var removedLinesIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(4, removedLinesIndexes.Length);
            Assert.AreEqual(1, newLines.LinesToChange.Count());
            Assert.AreEqual(text1 + text6, newLines.LinesToChange.ElementAt(0).Value.GetStringContents()[0]);
        }

        [Test]
        public void FourCharactersEnteredSelectionForLastTwo_LineShouldBeText1() {
            var text1 = "as";
            var text2 = "df";
            var lines = new List<VisualTextLine> {
                VisualTextLine.Create(text1 + text2, 0)
            };
            var newLines = algorithm.GetChangeInLines(lines, new TextRange {
                StartPosition = new TextPosition(column: 2, line: 0),
                EndPosition = new TextPosition(column: 4, line: 0)
            });

            Assert.AreEqual(0, newLines.LinesToRemove.Count());
            Assert.AreEqual(text1, newLines.LinesToChange.ElementAt(0).Value.GetStringContents()[0]);
        }
    }
}

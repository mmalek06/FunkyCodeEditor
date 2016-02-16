using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using CodeEditor.Algorithms.TextManipulation;
using CodeEditor.TextProperties;

namespace CodeEditor.Tests {
    [TestClass]
    public class RemovingTextAlgTests {
        private static TextRemover algorithm;
        private static Views.TextView.View view;
        private static TextRunProperties runProperties;

        [ClassInitialize]
        public static void Initialize(TestContext context) {
            algorithm = new TextRemover();
            view = new Views.TextView.View();
            runProperties = Configuration.TextConfiguration.GetGlobalTextRunProperties();
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredDelPressedAtTheEndOfFirstOne_LineShouldBeEqualToText1Text2() {
            string text1 = "asdf";
            string text2 = "zxcv";
            var textSources = new List<SimpleTextSource> {
                new SimpleTextSource(text1, runProperties),
                new SimpleTextSource(text2, runProperties)
            };
            var newLines = algorithm.TransformLines(textSources, new DataStructures.TextPosition(column: 4, line: 0), Key.Delete);

            Assert.AreEqual(text1 + text2, newLines.LinesToChange.First().Value);
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredBackspacePressedAtTheBeginningOfSecond_LineShouldBeEqualToText1Text2() {
            string text1 = "asdf";
            string text2 = "zxcv";
            var textSources = new List<SimpleTextSource> {
                new SimpleTextSource(text1, runProperties),
                new SimpleTextSource(text2, runProperties)
            };
            var newLines = algorithm.TransformLines(textSources, new DataStructures.TextPosition(column: 0, line: 1), Key.Back);

            Assert.AreEqual(text1 + text2, newLines.LinesToChange.First().Value);
        }

        [TestMethod]
        public void ThreeNonEmptyLinesEnteredBackspacePressedAtCharOneBeforeTheLastOneInTheLastLine_LinesShouldBeText4AndShouldRemoveSelection() {
            string text1 = "some text";
            string text2 = "";
            string text3 = "totally unimportant text";
            string text4 = "text that stays";
            var textSources = new List<SimpleTextSource> {
                new SimpleTextSource(text1, runProperties),
                new SimpleTextSource(text2, runProperties),
                new SimpleTextSource(text3 + text4, runProperties)
            };
            var newLines = algorithm.TransformLines(textSources, new DataStructures.TextPositionsPair {
                StartPosition = new DataStructures.TextPosition(column: 0, line: 0),
                EndPosition = new DataStructures.TextPosition(column: 24, line: 2)
            });

            Assert.AreEqual(text4, newLines.LinesToChange.First().Value);
            Assert.IsTrue(Enumerable.SequenceEqual((new[] { 0, 1 }).OrderBy(key => key).ToArray(), newLines.LinesToRemove.OrderBy(key => key).ToArray()));
        }

        [TestMethod]
        public void FourLinesEnteredDeletePressedAtTheEndOfFirst_LinesShouldBeText1Text2Text3() {
            string text1 = "asdf";
            string text2 = "zxcv";
            string text3 = "qwer";
            var textSources = new List<SimpleTextSource> {
                new SimpleTextSource(text1, runProperties),
                new SimpleTextSource(string.Empty, runProperties),
                new SimpleTextSource(text2, runProperties),
                new SimpleTextSource(text3, runProperties)
            };
            var newLines = algorithm.TransformLines(textSources, new DataStructures.TextPosition(column: 4, line: 0), Key.Delete);
            var removedLinesIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(text1, newLines.LinesToChange.First().Value);
            Assert.AreEqual(text2, newLines.LinesToChange.ElementAt(1).Value);
            Assert.AreEqual(text3, newLines.LinesToChange.ElementAt(2).Value);
            Assert.AreEqual(1, removedLinesIndexes.Length);
            Assert.AreEqual(3, removedLinesIndexes[0]);
        }

        [TestMethod]
        public void FourLinesEnteredDeletePressedTwiceAtTheEndOfFirst_LinesShouldBeText1PlusText2Text3() {
            string text1 = "asdf";
            string text2 = "zxcv";
            string text3 = "qwer";
            var textSources = new List<SimpleTextSource> {
                new SimpleTextSource(text1, runProperties),
                new SimpleTextSource(string.Empty, runProperties),
                new SimpleTextSource(text2, runProperties),
                new SimpleTextSource(text3, runProperties)
            };

            algorithm.TransformLines(textSources, new DataStructures.TextPosition(column: 4, line: 0), Key.Delete);

            var newLines = algorithm.TransformLines(textSources, new DataStructures.TextPosition(column: 4, line: 0), Key.Delete);
            var removedLinesIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(text1, newLines.LinesToChange.First().Value);
            Assert.AreEqual(text2, newLines.LinesToChange.ElementAt(1).Value);
            Assert.AreEqual(text3, newLines.LinesToChange.ElementAt(2).Value);
            Assert.AreEqual(1, removedLinesIndexes.Length);
            Assert.AreEqual(3, removedLinesIndexes[0]);
        }

        [TestMethod]
        public void OneLineEnteredSelectionOfFourCharsDeletePressed_LineShouldBeText1Text3() {
            string text1 = "asdf";
            string text2 = "qwer";
            string text3 = "zxcv";
            var textSources = new List<SimpleTextSource> {
                new SimpleTextSource(text1 + text2 + text3, runProperties)
            };
            var newLines = algorithm.TransformLines(textSources, new DataStructures.TextPositionsPair {
                StartPosition = new DataStructures.TextPosition(column: 4, line: 0),
                EndPosition = new DataStructures.TextPosition(column: 8, line: 0)
            });
            var removedLinesIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(0, removedLinesIndexes.Length);
            Assert.AreEqual(text1 + text3, newLines.LinesToChange.ElementAt(0).Value);
        }

        [TestMethod]
        public void TwoLinesEnteredSelectionInTheMiddleOfFirst_LinesShouldBeText1Text3Text4() {
            string text1 = "as";
            string text2 = " df ";
            string text3 = "qwer";
            string text4 = "zxcv";
            var textSources = new List<SimpleTextSource> {
                new SimpleTextSource(text1 + text2 + text3, runProperties),
                new SimpleTextSource(text4, runProperties)
            };
            var newLines = algorithm.TransformLines(textSources, new DataStructures.TextPositionsPair {
                StartPosition = new DataStructures.TextPosition(column: 2, line: 0),
                EndPosition = new DataStructures.TextPosition(column: 6, line: 0)
            });
            var removedLineIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(0, removedLineIndexes.Length);
            Assert.AreEqual(1, newLines.LinesToChange.Count());
            Assert.AreEqual(text1 + text3, newLines.LinesToChange.ElementAt(0).Value);
        }

        [TestMethod]
        public void FourLinesEnteredSelectionInbetweenDeletePressed_LinesShouldBeText1Text6() {
            string text1 = "as";
            string text2 = "df";
            string text3 = "qwer";
            string text4 = "zxcv";
            string text5 = "fg";
            string text6 = "hj";
            var textSources = new List<SimpleTextSource> {
                new SimpleTextSource(text1 + text2, runProperties),
                new SimpleTextSource(text3, runProperties),
                new SimpleTextSource(text4, runProperties),
                new SimpleTextSource(text5 + text6, runProperties)
            };
            var newLines = algorithm.TransformLines(textSources, new DataStructures.TextPositionsPair {
                StartPosition = new DataStructures.TextPosition(column: 2, line: 0),
                EndPosition = new DataStructures.TextPosition(column: 2, line: 3)
            });
            var removedLinesIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(3, removedLinesIndexes.Length);
            Assert.AreEqual(1, newLines.LinesToChange.Count());
            Assert.AreEqual(text1 + text6, newLines.LinesToChange.ElementAt(0).Value);
        }

        [TestMethod]
        public void FourCharactersEnteredSelectionForLastTwo_LineShouldBeText1() {
            string text1 = "as";
            string text2 = "df";
            var textSources = new List<SimpleTextSource> {
                new SimpleTextSource(text1 + text2, runProperties)
            };
            var newLines = algorithm.TransformLines(textSources, new DataStructures.TextPositionsPair {
                StartPosition = new DataStructures.TextPosition(column: 2, line: 0),
                EndPosition = new DataStructures.TextPosition(column: 4, line: 0)
            });

            Assert.AreEqual(0, newLines.LinesToRemove.Count());
            Assert.AreEqual(text1, newLines.LinesToChange.ElementAt(0).Value);
        }
    }
}

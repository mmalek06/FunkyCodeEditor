using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using TextEditor.Extensions;
using TextEditor.TextProperties;

namespace TextEditorTests.TextManipulationAlgorithmTests {
    [TestClass]
    public class RemovingTextAlgTests {
        private static TextEditor.Views.TextView.TextLineRemover algorithm;
        private static TextEditor.Views.TextView.View view;
        private static TextRunProperties runProperties;

        [ClassInitialize]
        public static void Initialize(TestContext context) {
            algorithm = new TextEditor.Views.TextView.TextLineRemover();
            view = new TextEditor.Views.TextView.View();
            runProperties = view.CreateGlobalTextRunProperties();
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredDelPressedAtTheEndOfFirstOne_LineShouldBeEqualToText1Text2() {
            string text1 = "asdf";
            string text2 = "zxcv";
            var textSources = new List<SimpleTextSource> {
                new SimpleTextSource(text1, runProperties),
                new SimpleTextSource(text2, runProperties)
            };

            var newLines = algorithm.TransformLines(textSources, new TextEditor.DataStructures.TextPosition { Column = 4, Line = 0 }, Key.Delete);

            Assert.AreEqual(text1 + text2, newLines.LinesAffected.First().Value);
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredBackspacePressedAtTheBeginningOfSecond_LineShouldBeEqualToText1Text2() {
            string text1 = "asdf";
            string text2 = "zxcv";
            var textSources = new List<SimpleTextSource> {
                new SimpleTextSource(text1, runProperties),
                new SimpleTextSource(text2, runProperties)
            };

            var newLines = algorithm.TransformLines(textSources, new TextEditor.DataStructures.TextPosition { Column = 0, Line = 1 }, Key.Back);

            Assert.AreEqual(text1 + text2, newLines.LinesAffected.First().Value);
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

            var newLines = algorithm.TransformLines(textSources, new TextEditor.DataStructures.TextPosition { Column = 4, Line = 0 }, Key.Delete);
            var removedLinesIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(text1, newLines.LinesAffected.First().Value);
            Assert.AreEqual(text2, newLines.LinesAffected.ElementAt(1).Value);
            Assert.AreEqual(text3, newLines.LinesAffected.ElementAt(2).Value);
            Assert.AreEqual(3, removedLinesIndexes.Length);
            Assert.AreEqual(1, removedLinesIndexes[0]);
            Assert.AreEqual(2, removedLinesIndexes[1]);
            Assert.AreEqual(3, removedLinesIndexes[2]);
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

            algorithm.TransformLines(textSources, new TextEditor.DataStructures.TextPosition { Column = 4, Line = 0 }, Key.Delete);

            var newLines = algorithm.TransformLines(textSources, new TextEditor.DataStructures.TextPosition { Column = 4, Line = 0 }, Key.Delete);
            var removedLinesIndexes = newLines.LinesToRemove.ToArray();

            Assert.AreEqual(text1, newLines.LinesAffected.First().Value);
            Assert.AreEqual(text2, newLines.LinesAffected.ElementAt(1).Value);
            Assert.AreEqual(text3, newLines.LinesAffected.ElementAt(2).Value);
            Assert.AreEqual(3, removedLinesIndexes.Length);
            Assert.AreEqual(1, removedLinesIndexes[0]);
            Assert.AreEqual(2, removedLinesIndexes[1]);
            Assert.AreEqual(3, removedLinesIndexes[2]);
        }
    }
}

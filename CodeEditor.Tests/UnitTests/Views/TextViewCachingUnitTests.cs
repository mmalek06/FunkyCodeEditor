using CodeEditor.Messaging;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Text;
using CodeEditor.Visuals.Base;
using NUnit.Framework;

namespace CodeEditor.Tests.UnitTests.Views {
    [TestFixture]
    public class TextViewCachingUnitTests {
        private TextView tv;

        private CaretView cv;

        [SetUp]
        public void InitializeTest() {
            cv = new CaretView();
            tv = new TextView(cv);

            tv.EnterText("one");
            tv.EnterText("\r");
            tv.EnterText("two");
            tv.EnterText("\r");
            tv.EnterText("three");
            tv.EnterText("\r");
            tv.EnterText("four");
            tv.EnterText("\r");
            tv.EnterText("five");
            tv.EnterText("\r");
            tv.EnterText("six");
        }

        [Test]
        public void ScrollTwoLinesDown_ShouldCacheFourLines() {
            tv.HandleScrolling(GetMessage(2, 2, 3));

            var visualLines = tv.GetVisualLines();

            Assert.That(tv.LinesCount, Is.EqualTo(6));
            Assert.IsInstanceOf<CachedVisualTextLine>(visualLines[0]);
            Assert.IsInstanceOf<CachedVisualTextLine>(visualLines[1]);
            Assert.IsInstanceOf<CachedVisualTextLine>(visualLines[4]);
            Assert.IsInstanceOf<CachedVisualTextLine>(visualLines[5]);
        }

        [Test]
        public void ScrollTwoLinesUp_ShouldCacheFourLines() {
            tv.HandleScrolling(GetMessage(2, 0, 1));

            var visualLines = tv.GetVisualLines();

            Assert.That(tv.LinesCount, Is.EqualTo(6));
            Assert.IsInstanceOf<CachedVisualTextLine>(visualLines[2]);
            Assert.IsInstanceOf<CachedVisualTextLine>(visualLines[3]);
            Assert.IsInstanceOf<CachedVisualTextLine>(visualLines[4]);
            Assert.IsInstanceOf<CachedVisualTextLine>(visualLines[5]);
        }

        private ScrollChangedMessage GetMessage(int changeInLines, int firstVisibleLineIdx, int lastVisibleLineIdx) =>
            new ScrollChangedMessage { LastVisibleLineIndex = lastVisibleLineIdx, LinesScrolled = changeInLines, FirstVisibleLineIndex = firstVisibleLineIdx };
    }
}

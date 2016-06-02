using CodeEditor.Messaging;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Text;
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
        }

        [Test]
        public void SixLinesEntered_ShouldCacheTwoFirstAndTwoLastsLines() {
            tv.EnterText("one");
            tv.EnterText("\n");
            tv.EnterText("two");
            tv.EnterText("\n");
            tv.EnterText("three");
            tv.EnterText("\n");
            tv.EnterText("four");
            tv.EnterText("\n");
            tv.EnterText("five");
            tv.EnterText("\n");
            tv.EnterText("six");
            tv.HandleScrolling(CreateScrollChangedMessage(1, 4, 2));

            var visualLines = tv.GetVisualLines();

            Assert.That(tv.LinesCount, Is.EqualTo(6));
            Assert.That(visualLines.Count, Is.EqualTo(2));
        }

        private ScrollChangedMessage CreateScrollChangedMessage(int topmostLine, int bottommostLine, int changeInLines) =>
            new ScrollChangedMessage {
                TopmostLine = topmostLine,
                BottommostLine = bottommostLine,
                ChangeInLines = changeInLines
            };
    }
}

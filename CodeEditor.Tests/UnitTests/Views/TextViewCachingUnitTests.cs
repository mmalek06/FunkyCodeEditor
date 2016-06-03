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

        [TestCase(2)]
        [TestCase(-2)]
        public void ScrollTwoLines(int changeInLines) {
            tv.HandleScrolling(new ScrollChangedMessage {
                ChangeInLines = changeInLines
            });

            var visualLines = tv.GetVisualLines();

            Assert.That(tv.LinesCount, Is.EqualTo(6));
            Assert.That(visualLines.Count, Is.EqualTo(4));
        }
    }
}

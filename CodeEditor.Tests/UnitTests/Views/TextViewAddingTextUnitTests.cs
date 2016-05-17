using CodeEditor.Views.Caret;
using CodeEditor.Views.Text;
using NUnit.Framework;

namespace CodeEditor.Tests.UnitTests.Views {
    [TestFixture]
    public class TextViewAddingTextUnitTests {
        private TextView tv;

        private CaretView cv;

        [SetUp]
        public void InitializeTest() {
            cv = new CaretView();
            tv = new TextView(cv);
        }

        [Test]
        public void FourCharsPasted_LinesShouldBe1() {
            tv.EnterText("asdf");

            Assert.AreEqual(1, tv.LinesCount);
        }

        [Test]
        public void FourCharsEntered_LineLengthShouldBe4() {
            tv.EnterText("a");
            tv.EnterText("b");
            tv.EnterText("c");
            tv.EnterText("d");

            Assert.AreEqual(4, tv.GetLineLength(0));
        }

        [Test]
        public void ThreeLinesAddedCharsEntered_LinesShouldBe3() {
            tv.EnterText("a");
            tv.EnterText("s");
            tv.EnterText("d");
            tv.EnterText("f");
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText("z");
            tv.EnterText("x");
            tv.EnterText("c");
            tv.EnterText("v");

            Assert.AreEqual(3, tv.LinesCount);
        }

        [Test]
        public void ThreeLinesPasted_LinesShouldBe3() {
            tv.EnterText("asdf");
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText("zxcv");

            Assert.AreEqual(3, tv.LinesCount);
        }
    }
}

using CodeEditor.Algorithms.TextManipulation;
using CodeEditor.Views.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests.AlgorithmTests {
    [TestClass]
    public class BracketsCollapsingTextTests {
        private TextCollapser tc;
        private TextView tv;

        [TestInitialize]
        public void InitializeTest() {
            tc = new TextCollapser();
            tv = new TextView();
        }
    }
}

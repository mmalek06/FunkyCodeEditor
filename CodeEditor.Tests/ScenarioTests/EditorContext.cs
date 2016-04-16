using System.Collections.Generic;
using CodeEditor.Commands;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Folding;
using CodeEditor.Views.Lines;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;

namespace CodeEditor.Tests.ScenarioTests {
    public class EditorContext {

        #region properties

        public List<string> TextsToEnter { get; set; }

        internal TextView TextView { get; set; }

        internal CaretView CaretView { get; set; }

        internal SelectionView SelectionView { get; set; }

        internal LinesView LinesView { get; set; }

        internal FoldingView FoldingView { get; set; }

        internal TextView.TextViewInfo TextInfo { get; set; }

        internal EnterTextCommand EnterTextCommand { get; set; }

        internal RemoveTextCommand RemoveTextCommand { get; set; }

        internal CaretMoveCommand CaretMoveCommand { get; set;}

        #endregion

        #region constructor

        public EditorContext() {
            TextsToEnter = new List<string>();
            TextView = new TextView();
            TextInfo = TextView.TextViewInfo.GetInstance(TextView);
            CaretView = new CaretView();
            SelectionView = new SelectionView(TextInfo);
            LinesView = new LinesView();
            FoldingView = new FoldingView();
            EnterTextCommand = new EnterTextCommand(TextView, SelectionView, TextInfo);
            RemoveTextCommand = new RemoveTextCommand(TextView, SelectionView, TextInfo);
            CaretMoveCommand = new CaretMoveCommand(CaretView, TextInfo);

            TextView.TextChanged += CaretView.HandleTextChange;
            CaretView.CaretMoved += TextView.HandleCaretMove;
        }

        #endregion

    }
}

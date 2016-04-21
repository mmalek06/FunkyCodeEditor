using System.Collections.Generic;
using CodeEditor.Commands;
using CodeEditor.Messaging;
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

        internal EnterTextCommand EnterTextCommand { get; set; }

        internal RemoveTextCommand RemoveTextCommand { get; set; }

        internal CaretMoveCommand CaretMoveCommand { get; set;}

        internal TextSelectionCommand SelectionCommand { get; set; }

        #endregion

        #region constructor

        public EditorContext() {
            TextsToEnter = new List<string>();
            CaretView = new CaretView();
            TextView = new TextView(CaretView);
            SelectionView = new SelectionView(TextView, CaretView);
            LinesView = new LinesView();
            FoldingView = new FoldingView();
            EnterTextCommand = new EnterTextCommand(TextView, CaretView, SelectionView);
            RemoveTextCommand = new RemoveTextCommand(TextView, CaretView, SelectionView);
            CaretMoveCommand = new CaretMoveCommand(CaretView, TextView);
            SelectionCommand = new TextSelectionCommand(TextView, SelectionView, CaretView);

            InitEvents();
            ForceDraw();
        }

        #endregion

        #region methods

        private void InitEvents() {
            Postbox.Instance.For(typeof(LinesRemovedMessage)).Invoke(message => {
                                var linesRemovedMessage = message as LinesRemovedMessage;

                                LinesView.HandleLinesRemove(linesRemovedMessage.Count);
                                FoldingView.HandleLinesRemove(linesRemovedMessage.Count);
                            })
                            .For(typeof(TextRemovedMessage)).Invoke(message => {
                                var textRemovedMessage = message as TextRemovedMessage;

                                LinesView.HandleTextRemove(textRemovedMessage);
                                FoldingView.HandleTextRemove(textRemovedMessage);
                            })
                            .For(typeof(TextAddedMessage)).Invoke(message => {
                                var textAddedMessage = message as TextAddedMessage;

                                LinesView.HandleTextInput(textAddedMessage);
                                FoldingView.HandleTextInput(textAddedMessage);
                            })
                            .For(typeof(FoldClickedMessage)).Invoke(message => LinesView.HandleFolding(message as FoldClickedMessage));
        }

        private void ForceDraw() {
            PrivateMembersHelper.InvokeMethod(LinesView, "Push", new object[] { });
        }

        #endregion

    }
}

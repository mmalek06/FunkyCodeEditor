using System.Collections.Generic;
using CodeEditor.Commands;
using CodeEditor.Core.Enums;
using CodeEditor.Messaging;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Folding;
using CodeEditor.Views.Lines;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;

namespace CodeEditor.Tests.ScenarioTests {
    public class EditorContext {

        #region properties

        internal List<string> TextsToEnter { get; set; }

        internal TextView TextView { get; set; }

        internal CaretView CaretView { get; set; }

        internal SelectionView SelectionView { get; set; }

        internal LinesView LinesView { get; set; }

        internal FoldingView FoldingView { get; set; }

        internal EnterTextCommand EnterTextCommand { get; set; }

        internal RemoveTextCommand RemoveTextCommand { get; set; }

        internal CaretMoveCommand CaretMoveCommand { get; set;}

        internal TextSelectionCommand SelectionCommand { get; set; }

        internal Postbox Postbox { get; set; }

        internal const int EditorCode = 1;

        #endregion

        #region constructor

        public EditorContext() {
            Postbox = Postbox.InstanceFor(EditorCode);
            Configuration.ConfigManager.AddEditorConfig(EditorCode, new Configuration.Config {
                Language = SupportedLanguages.JS,
                FormattingType = FormattingType.BRACKETS
            });

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

            CaretView.EditorCode = EditorCode;
            CaretView.Postbox = Postbox;
            TextView.EditorCode = EditorCode;
            TextView.Postbox = Postbox;
            SelectionView.EditorCode = EditorCode;
            SelectionView.Postbox = Postbox;
            LinesView.EditorCode = EditorCode;
            LinesView.Postbox = Postbox;
            FoldingView.EditorCode = EditorCode;
            FoldingView.Postbox = Postbox;

            InitEvents();
            ForceDraw();
        }

        #endregion

        #region public methods

        public void RemoveMessages() {
            var postbox = Postbox.InstanceFor(EditorCode);

            postbox.RemoveListener(typeof(LinesRemovedMessage))
                   .RemoveListener(typeof(TextRemovedMessage))
                   .RemoveListener(typeof(TextAddedMessage))
                   .RemoveListener(typeof(FoldClickedMessage));
        }

        #endregion

        #region methods

        private void InitEvents() {
            var postbox = Postbox.InstanceFor(EditorCode);

            postbox.For(typeof(LinesRemovedMessage)).Invoke(message => {
                       var linesRemovedMessage = message as LinesRemovedMessage;

                       LinesView.HandleLinesRemove(linesRemovedMessage);
                       FoldingView.HandleLinesRemove(linesRemovedMessage);
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
                   .For(typeof(FoldClickedMessage)).Invoke(message => {
                       var foldClickedMessage = message as FoldClickedMessage;

                       TextView.HandleTextFolding(foldClickedMessage);
                       LinesView.HandleFolding(foldClickedMessage);
                       CaretView.HandleTextFolding(foldClickedMessage);
                   });
        }

        private void ForceDraw() {
            PrivateMembersHelper.InvokeMethod(LinesView, "Push", new object[] { });
        }

        #endregion

    }
}

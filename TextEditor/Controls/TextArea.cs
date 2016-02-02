﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TextEditor.Commands;
using TextEditor.DataStructures;
using LocalTextInfo = TextEditor.Views.TextView.TextInfo;
using LocalViewBase = TextEditor.Views.ViewBase;

namespace TextEditor.Controls {
    public class TextArea : Control {

        #region fields

        private Editor master;
        private List<LocalViewBase> views;
        private LocalTextInfo textInfo;
        private Views.TextView.View textView;
        private Views.CaretView.View caretView;
        private Views.SelectionView.View selectionView;

        #endregion

        #region properties

        public TextPosition CaretPosition => caretView.CaretPosition;

        internal LocalTextInfo TextInfo => textInfo;

        protected override int VisualChildrenCount => views.Count;

        #endregion

        #region constructor

        public TextArea(Editor parent) {
            master = parent;
            views = new List<LocalViewBase>();
            
            SetViews();
            InitEvents();
        }

        #endregion

        #region event handlers

        protected override void OnRender(DrawingContext drawingContext) =>
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, RenderSize.Width, RenderSize.Height));

        protected override void OnKeyDown(KeyEventArgs e) {
            var removeTextCmd = new RemoveTextCommand(textView, textInfo);
            var caretMoveCmd = new CaretMoveCommand(textView, caretView, textInfo);
            var selectionCmd = new TextSelectionCommand(textView, selectionView, textInfo);

            if (removeTextCmd.CanExecute(e)) {
                ExecuteTextCommand(removeTextCmd, new UndoRemoveTextCommand(textView, textInfo), e);
            } else if (caretMoveCmd.CanExecute(e)) {
                caretMoveCmd.Execute(e);
            } else if (selectionCmd.CanExecute(e)) {
                selectionCmd.Execute(e);
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e) {
            var enterTextCmd = new EnterTextCommand(textView, textInfo);

            if (enterTextCmd.CanExecute(e)) {
                ExecuteTextCommand(enterTextCmd, new UndoEnterTextCommand(textView, textInfo), e);
            }
        }

        #endregion

        #region methods

        internal int GetLinesCount() => textInfo.GetTextLinesCount();

        protected override Visual GetVisualChild(int index) => views[index];

        private void SetViews() {
            textView = new Views.TextView.View();
            textInfo = new LocalTextInfo(textView);
            selectionView = new Views.SelectionView.View(textInfo);
            caretView = new Views.CaretView.View();

            foreach (var view in new LocalViewBase[] { selectionView, textView, caretView }) {
                views.Add(view);
                AddVisualChild(view);
                AddLogicalChild(view);
            }
        }

        private void InitEvents() {
            // standard control events
            MouseMove += selectionView.HandleMouseMove;
            MouseDown += selectionView.HandleMouseDown;
            MouseDown += textView.HandleMouseDown;
            MouseDown += caretView.HandleMouseDown;
            GotFocus += textView.HandleGotFocus;
            KeyDown += selectionView.HandleKeyDown;

            // custom editor events
            textView.TextChanged += caretView.HandleTextChange;
            caretView.CaretMoved += textView.HandleCaretMove;
        }

        private void ExecuteTextCommand(BaseTextViewCommand doCommand, BaseTextViewCommand undoCommand, EventArgs e) {
            doCommand.Execute(e);
            master.DoCommands.Push(doCommand);
            master.UndoCommands.Push(undoCommand);

            undoCommand.BeforeCommandExecutedState = doCommand.AfterCommandExecutedState;
            undoCommand.AfterCommandExecutedState = doCommand.BeforeCommandExecutedState;
        }

        #endregion

    }
}

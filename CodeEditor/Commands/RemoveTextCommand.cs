﻿using System.Collections.Generic;
using System.Windows.Input;
using CodeEditor.Core.DataStructures;
using CodeEditor.Messaging;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;
using LocalTextInfo = CodeEditor.Views.Text.TextInfo;

namespace CodeEditor.Commands {
    internal class RemoveTextCommand : BaseTextViewCommand {
        
        #region fields

        private HashSet<Key> removalKeys = new HashSet<Key> { Key.Delete, Key.Back };

        private SelectionView selectionView;

        #endregion

        #region constructor

        public RemoveTextCommand(SelectionView selectionView, TextView view, LocalTextInfo textInfo) : base(view, textInfo) {
            this.selectionView = selectionView;
        }

        #endregion

        #region public methods

        public override bool CanExecute(object parameter) {
            var e = parameter as KeyEventArgs;

            if (e == null) {
                return false;
            }
            if (!removalKeys.Contains(e.Key)) {
                return false;
            }
            if (e.Key == Key.Delete && view.ActivePosition.Line == textInfo.GetTextLinesCount() - 1 && view.ActivePosition.Column == textInfo.GetTextLineLength(view.ActivePosition.Line)) { 
                return false;
            }

            var area = selectionView.GetCurrentSelectionArea();

            if (area != null && area.StartPosition != null && area.EndPosition != null) {
                return area.StartPosition != area.EndPosition;
            }

            return true;
        }

        public override void Execute(object parameter) {
            var e = parameter as KeyEventArgs;
            var key = e.Key;
            var selectionArea = selectionView.GetCurrentSelectionArea();
            var prevPosition = view.ActivePosition;
            int linesCountBeforeRemove = textInfo.GetTextLinesCount();
            string removedText = string.Empty;

            UpdateCommandState(BeforeCommandExecutedState);

            if (selectionArea == null) {
                removedText = GetRemovedText(key);

                view.RemoveText(key);
            } else {
                removedText = string.Join("", textInfo.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition));

                view.RemoveText(selectionArea);
            }
            if (textInfo.GetTextLinesCount() < linesCountBeforeRemove) {
                Postbox.Instance.Send(new LineRemovedMessage {
                    Key = key,
                    Position = prevPosition,
                    LineLength = textInfo.GetTextLineLength(prevPosition.Line)
                });
            }

            Postbox.Instance.Send(new TextRemovedMessage {
                Key = e.Key,
                Position = view.ActivePosition,
                RemovedText = removedText
            });

            UpdateCommandState(AfterCommandExecutedState);

            view.TriggerTextChanged();

            e.Handled = true;
        }

        #endregion

        #region methods

        private string GetRemovedText(Key key) {
            if (textInfo.GetTextLineLength(view.ActivePosition.Line) == 0 || (key == Key.Delete && view.ActivePosition.Column >= textInfo.GetTextLine(view.ActivePosition.Line).Length)) {
                return string.Empty;
            }
            if (key == Key.Delete) {
                return textInfo.GetCharAt(view.ActivePosition).ToString();
            } else {
                return textInfo.GetCharAt(
                    new TextPosition(column: view.ActivePosition.Column > 0 ? view.ActivePosition.Column - 1 : 0, line: view.ActivePosition.Line)).ToString();
            }
        }

        #endregion

    }
}

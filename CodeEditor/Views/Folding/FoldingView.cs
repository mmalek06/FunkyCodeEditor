using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Extensions;
using CodeEditor.Messaging;
using CodeEditor.Views.BaseClasses;

namespace CodeEditor.Views.Folding {
    internal class FoldingView : HelperViewBase {

        #region fields

        private readonly List<VisualElementSymbol> symbols;
        private IFoldingAlgorithm foldingAlgorithm;
        private Dictionary<FoldingPositionInfo, FoldingPositionInfo> foldingPositions;

        #endregion

        #region constructor

        public FoldingView() : base() {
            bgBrush = EditorConfiguration.GetFoldingColumnBrush();
            symbols = new List<VisualElementSymbol>();
            foldingAlgorithm = EditorConfiguration.GetFoldingAlgorithm();
            foldingPositions = new Dictionary<FoldingPositionInfo, FoldingPositionInfo>();
            Margin = new Thickness(EditorConfiguration.GetLinesColumnWidth(), 0, 0, 0);
        }

        #endregion

        #region event handlers

        public override void HandleTextInput(string text, TextPosition activePosition) {
            if (!foldingAlgorithm.CanRun(text)) {
                return;
            }

            var folds = foldingAlgorithm.CreateFolds(text, new TextPosition(column: activePosition.Column - 1, line: activePosition.Line), GetPotentialFoldingPositions());

            if (folds == null || !folds.Any()) {
                return;
            }

            CreateFolds(folds);
            RedrawFolds();
        }

        public override void HandleTextRemove(TextRemovedMessage message) {
            if (message.RemovedText == string.Empty) {
                return;
            }

            var positions = GetClosedFoldingInfos().ToDictionary(pair => pair.Key.Position, pair => pair.Value.Position);
            var removedKey = foldingAlgorithm.DeleteFolds(message.RemovedText, message.NewCaretPosition, positions);

            if (removedKey == null) {
                return;
            }

            DeleteFolds(removedKey, message.RemovedText);
            RedrawFolds();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            var position = e.GetPosition(this).GetDocumentPosition(TextConfiguration.GetCharSize());
            var folding = foldingPositions.Keys.FirstOrDefault(foldingInfo => foldingInfo.Position.Line == position.Line);

            if (folding != null) {
                var state = folding.State == FoldingStates.EXPANDED ? FoldingStates.FOLDED : FoldingStates.EXPANDED;

                Postbox.Instance.Send(new FoldClickedMessage {
                    Area = new TextRange {
                        StartPosition = folding.Position,
                        EndPosition = foldingPositions[folding].Position
                    },
                    ClosingTag = foldingAlgorithm.GetClosingTag(),
                    OpeningTag = foldingAlgorithm.GetOpeningTag(),
                    State = state
                });
                folding.State = state;

                RedrawFolds();
            }
        }

        #endregion

        #region methods

        protected override double GetWidth() => EditorConfiguration.GetFoldingColumnWidth();

        private IDictionary<TextPosition, TextPosition> GetPotentialFoldingPositions() =>
            foldingPositions.Where(pair => pair.Key.Deleted || pair.Value == null || pair.Value.Deleted || pair.Value.Position == null)
                            .ToDictionary(pair => pair.Key.Position, pair => pair.Value.Position);

        private IDictionary<FoldingPositionInfo, FoldingPositionInfo> GetClosedFoldingInfos() =>
            foldingPositions.Where(pair => !pair.Key.Deleted && pair.Value != null && !pair.Value.Deleted && pair.Value.Position != null)
                            .ToDictionary(pair => pair.Key, pair => pair.Value);

        private void CreateFolds(IDictionary<TextPosition, TextPosition> folds) {
            foreach (var kvp in folds) {
                var tmpKey = new FoldingPositionInfo { Deleted = false, Position = kvp.Key };
                var existingKey = foldingPositions.Keys.FirstOrDefault(k => k.Equals(tmpKey));

                if (existingKey != null) {
                    existingKey.Deleted = false;
                } else {
                    existingKey = tmpKey;
                }

                foldingPositions[existingKey] = new FoldingPositionInfo { Deleted = false, Position = kvp.Value };
            }
        }

        private void DeleteFolds(TextPosition removedKey, string removedText) {
            var info = foldingPositions.Keys.First(k => k.Position == removedKey);

            if (foldingAlgorithm.IsOpeningTag(removedText)) {
                if (foldingPositions[info] == null || foldingPositions[info].Deleted || foldingPositions[info].Position == null) {
                    foldingPositions.Remove(info);
                } else {
                    info.Deleted = true;
                }
            } else {
                foldingPositions[info].Deleted = true;
            }
        }

        private void RedrawFolds() {
            visuals.Clear();
            symbols.Clear();

            foreach (var kvp in GetClosedFoldingInfos()) {
                var symbol = new VisualElementSymbol();
                int top = (int)kvp.Key.Position.GetPositionRelativeToParent(TextConfiguration.GetCharSize()).Y;

                symbol.DrawFolding(kvp.Key.State, top);

                symbols.Add(symbol);
                visuals.Add(symbol);
            }
        }

        #endregion

        #region classes

        private class FoldingPositionInfo {

            #region properties

            public TextPosition Position { get; set; }

            public FoldingStates State { get; set; }

            public bool Deleted { get; set; }

            #endregion

            #region public methods

            public override bool Equals(object obj) {
                var other = (FoldingPositionInfo)obj;

                if (other == null || other.Position == null) {
                    return false;
                }

                return Position.Equals(other.Position);
            }

            public override int GetHashCode() {
                return Position.GetHashCode();
            }

            public override string ToString() {
                return string.Format("{0}, Deleted: {1}, State: {2}", Position.ToString(), Deleted.ToString(), State.ToString());
            }

            #endregion

        }

        #endregion

    }
}

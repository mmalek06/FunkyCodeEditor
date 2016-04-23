using System.Collections.Generic;
using System.Linq;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Algorithms.Folding {
    internal class BracketsFoldingAlgorithm : IFoldingAlgorithm {

        #region public methods

        public bool CanRun(string text) {
            if (text.Length == 0) {
                return false;
            }

            return text == GetOpeningTag() || text == GetClosingTag();
        }

        public bool IsOpeningTag(string text) => text == GetOpeningTag();

        public string GetOpeningTag() => "{";

        public string GetClosingTag() => "}";

        public IDictionary<TextPosition, TextPosition> CreateFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            IDictionary<TextPosition, TextPosition> folds = null;

            if (text == GetOpeningTag()) {
                folds = CreateEmptyFold(position, foldingPositions);
            } else if (text == GetClosingTag()) {
                folds = RebuildFolds(position, foldingPositions);
            }
            if (folds != null) {
                GetRepeatingFolds(folds);
            }

            return folds;
        }

        public TextPosition DeleteFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            if (text == GetOpeningTag()) {
                return position;
            } else if (text == GetClosingTag()) {
                return DeleteFoldForClosePosition(position, foldingPositions);
            }

            return null;
        }

        public IEnumerable<TextPosition> GetRepeatingFolds(IDictionary<TextPosition, TextPosition> folds) =>
            folds.GroupBy(pair => pair.Key.Line)
                 .Where(group => group.Count() > 1)
                 .SelectMany(group => group)
                 .Select(pair => pair.Key);

        #endregion

        #region methods

        private IDictionary<TextPosition, TextPosition> CreateEmptyFold(TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            TextPosition outVal;

            foldingPositions.TryGetValue(position, out outVal);

            var tmpDict = new Dictionary<TextPosition, TextPosition>(foldingPositions);

            tmpDict[position] = outVal;

            return tmpDict;
        }

        private IDictionary<TextPosition, TextPosition> RebuildFolds(TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            var newFolds = new Dictionary<TextPosition, TextPosition>(foldingPositions);
            var candidates = foldingPositions.Where(kvp => position > kvp.Key && (kvp.Value == null || position < kvp.Value));
            var closestCandidate = candidates.Max(kvp => kvp.Key);

            if (closestCandidate != null) {
                if (newFolds[closestCandidate] == null) {
                    newFolds[closestCandidate] = position;
                } else {
                    newFolds = UpdateFolds(newFolds, closestCandidate, position);
                }
            }

            return newFolds;
        }

        private Dictionary<TextPosition, TextPosition> UpdateFolds(Dictionary<TextPosition, TextPosition> newFolds, TextPosition keyToUpdate, TextPosition foldValue) {
            var nextPositions = newFolds.Where(kvp => kvp.Key > keyToUpdate).OrderBy(kvp => kvp.Key);
            var oldPosition = newFolds[keyToUpdate];

            newFolds[keyToUpdate] = foldValue;

            foreach (var kvp in nextPositions) {
                var tmpPosition = kvp.Value;

                newFolds[kvp.Key] = oldPosition;
                oldPosition = tmpPosition;
            }

            return newFolds;
        }

        private TextPosition DeleteFoldForClosePosition(TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            var pair = foldingPositions.Where(kvp => kvp.Value == position).FirstOrDefault();

            return pair.Key;
        }

        #endregion

    }
}

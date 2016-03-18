using System.Collections.Generic;
using System.Linq;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Algorithms.Folding {
    internal class BracketsFoldingAlgorithm : IFoldingAlgorithm {

        #region fields

        private const char OPENING_BRACKET = '{';

        private const char CLOSING_BRACKET = '}';

        // mappedPositions are a part of this algorithm's internal logic
        // and therefore it belongs to this class
        private HashSet<TextPosition> mappedPositions;
        
        #endregion

        #region constructor

        public BracketsFoldingAlgorithm() {
            mappedPositions = new HashSet<TextPosition>();
        }

        #endregion

        #region public methods

        public bool CanRun(string text) {
            if (text.Length == 0) {
                return true;
            }

            char bracket = text[0];

            return bracket == OPENING_BRACKET || bracket == CLOSING_BRACKET;
        }

        public bool IsOpeningTag(string text) => text[0] == OPENING_BRACKET;

        public IDictionary<TextPosition, TextPosition> CreateFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            char bracket = text[0];

            if (bracket == OPENING_BRACKET) {
                return CreateEmptyFold(position, foldingPositions);
            } else if (bracket == CLOSING_BRACKET) {
                return RebuildFolds(position, foldingPositions);
            }

            return null;
        }

        public TextPosition DeleteFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            char bracket = text[0];

            if (bracket == OPENING_BRACKET) {
                DeleteFoldForOpenPosition(position);

                return position;
            } else if (bracket == CLOSING_BRACKET) {
                return DeleteFoldForClosePosition(position, foldingPositions);
            }

            return null;
        }

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

        private void DeleteFoldForOpenPosition(TextPosition position) => mappedPositions.Remove(position);

        private TextPosition DeleteFoldForClosePosition(TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            var pair = foldingPositions.Where(kvp => kvp.Value == position).FirstOrDefault();

            if (pair.Key != null && pair.Value != null) {
                mappedPositions.Remove(pair.Key);
            }

            return pair.Key;
        }

        #endregion

    }
}

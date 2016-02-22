using System.Collections.Generic;
using System.Linq;
using CodeEditor.DataStructures;

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
            char bracket = text[0];

            return bracket == OPENING_BRACKET || bracket == CLOSING_BRACKET;
        }

        public IEnumerable<KeyValuePair<TextPosition, TextPosition>> CreateFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            char bracket = text[0];

            if (bracket == OPENING_BRACKET) {
                return CreateFold(position, foldingPositions);
            } else if (bracket == CLOSING_BRACKET) {
                return UpdateFolds(position, foldingPositions);
            }

            return null;
        }

        public void GetFoldsToDelete(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            char bracket = text[0];

            if (bracket == OPENING_BRACKET) {
                DeleteFoldForOpenPosition(position);
            } else if (bracket == CLOSING_BRACKET) {
                DeleteFoldForClosePosition(position, foldingPositions);
            }
        }

        #endregion

        #region methods

        private IEnumerable<KeyValuePair<TextPosition, TextPosition>> CreateFold(TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            TextPosition outVal;

            if (!foldingPositions.TryGetValue(position, out outVal)) {
                foldingPositions[position] = null;

                return new[] { new KeyValuePair<TextPosition, TextPosition>(position, null) };
            }

            return null;
        }

        private IEnumerable<KeyValuePair<TextPosition, TextPosition>> UpdateFolds(TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            var parentPosition = foldingPositions.Keys.Where(key => !mappedPositions.Contains(key) && key <= position).Max();

            if (parentPosition == null) {
                return null;
            }

            var newFolds = new Dictionary<TextPosition, TextPosition>();
            var oldClosingPosition = foldingPositions[parentPosition];
            var nextOpeningPositions = foldingPositions.Keys.Where(key => key > parentPosition).ToArray();
            TextPosition previousValue = null;
            TextPosition nextValue = oldClosingPosition;

            foreach (var openPosition in nextOpeningPositions) {
                nextValue = foldingPositions[openPosition];
                newFolds[openPosition] = previousValue;
                previousValue = nextValue;
            }

            newFolds[parentPosition] = position;

            mappedPositions.Add(parentPosition);

            return newFolds;
        }

        private void DeleteFoldForOpenPosition(TextPosition position) {
            mappedPositions.Remove(position);
        }

        private void DeleteFoldForClosePosition(TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            var pair = foldingPositions.Where(kvp => kvp.Value == position).FirstOrDefault();

            if (!pair.Equals(default(KeyValuePair<TextPosition, TextPosition>))) {
                mappedPositions.Remove(pair.Key);
            }
        }

        #endregion

    }
}

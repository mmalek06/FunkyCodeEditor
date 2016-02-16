using System.Collections.Generic;
using System.Linq;
using CodeEditor.DataStructures;

namespace CodeEditor.Algorithms.Folding {
    internal class BracketsFoldingAlgorithm : IFoldingAlgorithm {

        #region fields

        private const char OPENING_BRACKET = '{';

        private const char CLOSING_BRACKET = '}';

        private HashSet<TextPosition> mappedPositions;

        private Dictionary<TextPosition, TextPosition> foldingPositions;

        #endregion

        #region properties

        public Dictionary<TextPosition, TextPosition> FoldingPositions => foldingPositions;

        #endregion

        #region constructor

        public BracketsFoldingAlgorithm() {
            mappedPositions = new HashSet<TextPosition>();
            foldingPositions = new Dictionary<TextPosition, TextPosition>();
        }

        #endregion

        #region public methods

        public bool CanHandle(string text) {
            char bracket = text[0];

            return bracket == OPENING_BRACKET || bracket == CLOSING_BRACKET;
        }

        public void RecreateFolds(string text, TextPosition position) {
            char bracket = text[0];

            if (bracket == OPENING_BRACKET) {
                UpdateFoldsForOpenPosition(position);
            } else if (bracket == CLOSING_BRACKET) {
                UpdateFoldsForClosePosition(position);
            }
        }

        public void DeleteFolds(string text, TextPosition position) {
            char bracket = text[0];

            if (bracket == OPENING_BRACKET) {
                DeleteFoldForOpenPosition(position);
            } else if (bracket == CLOSING_BRACKET) {
                DeleteFoldForClosePosition(position);
            }
        }

        #endregion

        #region methods

        private void UpdateFoldsForOpenPosition(TextPosition position) {
            TextPosition outVal;

            if (!foldingPositions.TryGetValue(position, out outVal)) {
                foldingPositions[position] = null;
            }
        }

        private void UpdateFoldsForClosePosition(TextPosition position) {
            var parentPosition = foldingPositions.Keys.Where(key => !mappedPositions.Contains(key) && key <= position).Max();

            if (parentPosition == null) {
                return;
            }

            var oldClosingPosition = foldingPositions[parentPosition];
            var nextOpeningPositions = foldingPositions.Keys.Where(key => key > parentPosition).ToArray();
            TextPosition previousValue = null;
            TextPosition nextValue = oldClosingPosition;

            foreach (var openPosition in nextOpeningPositions) {
                nextValue = foldingPositions[openPosition];
                foldingPositions[openPosition] = previousValue;
                previousValue = nextValue;
            }

            foldingPositions[parentPosition] = position;

            mappedPositions.Add(parentPosition);
        }

        private void DeleteFoldForOpenPosition(TextPosition position) {
            foldingPositions.Remove(position);
            mappedPositions.Remove(position);
        }

        private void DeleteFoldForClosePosition(TextPosition position) {
            var pair = foldingPositions.Where(kvp => kvp.Value == position).FirstOrDefault();

            if (!pair.Equals(default(KeyValuePair<TextPosition, TextPosition>))) {
                foldingPositions.Remove(pair.Key);
                mappedPositions.Remove(pair.Key);
            }
        }

        #endregion

    }
}

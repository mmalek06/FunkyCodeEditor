using System.Collections.Generic;
using System.Linq;
using CodeEditor.DataStructures;

namespace CodeEditor.Algorithms.Folding {
    internal class BracketsFoldingAlgorithm : IFoldingAlgorithm {

        #region fields

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

        public void Update(string text, TextPosition position) {
            if (text == "{") {
                UpdateFoldsForOpenPosition(position);
            }
            if (text == "}") {
                UpdateFoldsForClosePosition(position);
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
            var nextOpeningPositions = foldingPositions.Keys.Where(key => key > parentPosition);
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
        
        #endregion

    }
}

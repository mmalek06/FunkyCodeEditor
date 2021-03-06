﻿using System;
using System.Collections.Generic;
using CodeEditor.DataStructures;

namespace CodeEditor.Algorithms.Folding {
    internal class MarkupFoldingAlgorithm : IFoldingAlgorithm {

        #region public methods

        public bool CanRun(string text) {
            throw new NotImplementedException();
        }

        public IDictionary<TextPosition, TextPosition> CreateFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            throw new NotImplementedException();
        }

        public TextPosition DeleteFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions) {
            throw new NotImplementedException();
        }

        public IEnumerable<TextPosition> GetRepeatingFolds(IDictionary<TextPosition, TextPosition> folds) {
            throw new NotImplementedException();
        }

        public string GetClosingTag() {
            throw new NotImplementedException();
        }

        public string GetOpeningTag() {
            throw new NotImplementedException();
        }

        public string GetCollapsibleRepresentation() => "";

        public bool IsOpeningTag(string text) {
            throw new NotImplementedException();
        }

        public bool IsCollapseRepresentation(string text) => text == GetCollapsibleRepresentation();

        #endregion

    }
}

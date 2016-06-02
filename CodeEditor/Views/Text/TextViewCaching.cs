using System.Collections.Generic;
using CodeEditor.Caching;
using CodeEditor.Extensions;

namespace CodeEditor.Views.Text {
    internal partial class TextView {

        #region fields

        private readonly List<CachedLine> topCachedLines;

        private readonly List<CachedLine> bottomCachedLines;

        #endregion

        #region methods

        private void Cache(int topmostLine, int bottommostLine, int changeInLines) {
            // scrolled to top
            if (changeInLines < 0) {
                bottomCachedLines.AddRange(visuals.ConvertToCachedLines(bottommostLine, visuals.Count - bottommostLine));
            }
            // scrolled to bottom
            if (changeInLines > 0) {
                topCachedLines.AddRange(visuals.ConvertToCachedLines(topCachedLines.Count, changeInLines));
            }

            
        }
        
        #endregion

    }
}

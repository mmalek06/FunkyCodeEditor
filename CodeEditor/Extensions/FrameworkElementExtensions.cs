using System.Windows;

namespace CodeEditor.Extensions {
    internal static class FrameworkElementExtensions {

        #region public methods

        public static FrameworkElement GetEditor(this FrameworkElement fwElem) =>
            fwElem.Parent is Editor ? fwElem.Parent as FrameworkElement : (fwElem.Parent as FrameworkElement).GetEditor();

        #endregion

    }
}

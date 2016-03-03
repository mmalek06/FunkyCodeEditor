using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Controls {
    internal class InputPanel : Control {

        #region fields

        private InputViewsWrapper viewsWrapper;

        #endregion

        #region dependency properties

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(InputPanel),
            new PropertyMetadata(string.Empty, TextPropertyChanged));

        #endregion

        #region properties

        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        internal AutoTrimmingStack<ICommand> DoCommands { get; private set; }

        internal AutoTrimmingStack<ICommand> UndoCommands { get; private set; }

        protected override int VisualChildrenCount => 1;

        #endregion

        #region constructor

        public InputPanel() {
            DoCommands = new AutoTrimmingStack<ICommand>(100);
            UndoCommands = new AutoTrimmingStack<ICommand>(100);
            Focusable = false;
        }

        #endregion

        #region event handlers

        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);

            viewsWrapper = new InputViewsWrapper(this);

            AddVisualChild(viewsWrapper);
            AddLogicalChild(viewsWrapper);
        }

        #endregion

        #region methods

        protected override Visual GetVisualChild(int index) => viewsWrapper;

        /*private void SetAdorners() {
            var adornerLayer = AdornerLayer.GetAdornerLayer(viewsWrapper);
            var foldingAdorner = new Adorners.FoldingAdorner.Adorner(viewsWrapper) { Height = Height };
            var linesAdorner = new Adorners.LinesAdorner.Adorner(viewsWrapper) { Height = Height };

            adornerLayer.Add(foldingAdorner);
            adornerLayer.Add(linesAdorner);
            viewsWrapper.Adorners = new ReactiveAdorner[] { linesAdorner, foldingAdorner };
        }*/

        private void TextPropertyChanged(string text) {
            
        }
        
        #endregion

        #region static methods

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((InputPanel)d).TextPropertyChanged((string)e.NewValue);
        }

        #endregion

    }
}

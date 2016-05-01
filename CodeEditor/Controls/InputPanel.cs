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

        private static InputPanel self;

        #endregion

        #region dependency properties

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(InputPanel),
            new PropertyMetadata(string.Empty, TextPropertyChanged));

        #endregion

        #region properties

        public static InputPanel Instance => self;

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
            self = this;
        }

        #endregion

        #region event handlers

        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);

            viewsWrapper = new InputViewsWrapper(this);

            AddVisualChild(viewsWrapper);
            AddLogicalChild(viewsWrapper);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            viewsWrapper.Focus();

            e.Handled = true;
        }

        #endregion

        #region methods

        protected override Visual GetVisualChild(int index) => viewsWrapper;

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

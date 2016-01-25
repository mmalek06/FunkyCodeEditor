using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TextEditor.Configuration;
using TextEditor.Controls;
using TextEditor.DataStructures;

namespace TextEditor {
    public class Editor : Control {

        #region fields

        private int linesColumnWidth;
        private TextArea textArea;
        private Brush linesColumnColor;
        private Color linesColumnFontColor;

        #endregion

        #region dependency properties

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(Editor),
            new PropertyMetadata(string.Empty, TextPropertyChanged));

        #endregion

        #region properties

        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        internal AutoShiftStack<ICommand> DoCommands { get; private set; }

        internal AutoShiftStack<ICommand> UndoCommands { get; private set; }

        protected override int VisualChildrenCount => 1;

        #endregion

        #region constructor

        public Editor() {
            textArea = new TextArea(this);
            DoCommands = new AutoShiftStack<ICommand>(100);
            UndoCommands = new AutoShiftStack<ICommand>(100);
            Focusable = false;

            AddVisualChild(textArea);
            ConfigureLooks();
            Init();
        }

        #endregion

        #region methods

        protected override Visual GetVisualChild(int index) => textArea;

        private void TextPropertyChanged(string text) {
            //Text = text;

            RedrawLines();
        }

        private void RedrawLines() {
            int linesCount = textArea.GetLinesCount();
        }

        private void Init() {
            RedrawLines();
        }

        private void ConfigureLooks() {
            linesColumnWidth = EditorConfiguration.GetLinesColumnWidth();
            linesColumnColor = EditorConfiguration.GetLinesColumnBrush();
            linesColumnFontColor = EditorConfiguration.GetLinesColumnFontColor();

            textArea.Margin = new Thickness(linesColumnWidth, 0, 0, 0);
        }

        #endregion

        #region static methods

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((Editor)d).TextPropertyChanged((string)e.NewValue);
        }

        #endregion

    }
}

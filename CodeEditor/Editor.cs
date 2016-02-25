using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.Adorners;
using CodeEditor.Configuration;
using CodeEditor.Controls;
using CodeEditor.Core.DataStructures;
using CodeEditor.DataStructures;

namespace CodeEditor {
    public class Editor : Control {

        #region fields

        private ViewsWrapper textArea;

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

        internal AutoTrimmingStack<ICommand> DoCommands { get; private set; }

        internal AutoTrimmingStack<ICommand> UndoCommands { get; private set; }

        protected override int VisualChildrenCount => 1;

        #endregion

        #region constructor

        public Editor() {
            DoCommands = new AutoTrimmingStack<ICommand>(100);
            UndoCommands = new AutoTrimmingStack<ICommand>(100);
            Focusable = false;

            Loaded += EditorLoaded;
        }

        #endregion

        #region event handlers

        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);

            textArea = new ViewsWrapper(this);

            AddVisualChild(textArea);
            AddLogicalChild(textArea);

            ConfigureLooks();
        }

        private void EditorLoaded(object sender, RoutedEventArgs e) {
            SetAdorners();
            //RedrawLines(drawingContext);
        }

        #endregion

        #region methods

        protected override Visual GetVisualChild(int index) => textArea;

        private void SetAdorners() {
            var adornerLayer = AdornerLayer.GetAdornerLayer(textArea);
            var foldingAdorner = new Adorners.FoldingAdorner.Adorner(textArea);
            var linesAdorner = new Adorners.LinesAdorner.Adorner(textArea);

            adornerLayer.Add(foldingAdorner);
            adornerLayer.Add(linesAdorner);
            textArea.Adorners = new ReactiveAdorner[] { foldingAdorner };
        }

        private void TextPropertyChanged(string text) {
            
        }

        private void RedrawLines(DrawingContext drawingContext) {
            int linesCount = textArea.TextInfo.GetTextLinesCount();
            var lineNumbers = Enumerable.Range(1, linesCount).ToArray();
            var fontColor = EditorConfiguration.GetLinesColumnFontColor();
            double fontHeight = EditorConfiguration.GetFontHeight();
            var typeface = EditorConfiguration.GetTypeface();
            
            foreach (int num in lineNumbers) {
                drawingContext.DrawText(
                    new FormattedText(num.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontHeight, fontColor), 
                    new Point(0, fontHeight * (num - 1)));
            }
        }

        private void ConfigureLooks() {
            textArea.Margin = new Thickness(EditorConfiguration.GetLinesColumnWidth() * 2, 0, 0, 0);
        }

        #endregion

        #region static methods

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((Editor)d).TextPropertyChanged((string)e.NewValue);
        }

        #endregion

    }
}

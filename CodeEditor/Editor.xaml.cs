using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CodeEditor.Enums;

namespace CodeEditor {
    public partial class Editor : UserControl {

        #region dependency properties

        public static readonly DependencyProperty FormattingTypeProperty = DependencyProperty.Register(
            "FormattingType",
            typeof(FormattingType),
            typeof(FormattingType),
            null);

        #endregion

        #region properties

        public FormattingType FormattingType {
            get { return (FormattingType)GetValue(FormattingTypeProperty); }
            set { SetValue(FormattingTypeProperty, FormattingType); }
        }

        #endregion

        #region constructor

        public Editor() {
            InitializeComponent();
        }

        #endregion

    }
}

﻿using System;
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
using CodeEditor.Configuration;
using CodeEditor.Enums;
using CodeEditor.Languages;
using CodeEditor.Messaging;

namespace CodeEditor {
    public partial class Editor : UserControl {

        #region dependency properties

        public static readonly DependencyProperty CodingLangProperty = DependencyProperty.Register(
            "CodingLang",
            typeof(SupportedLanguages),
            typeof(SupportedLanguages),
            null);

        #endregion

        #region properties

        public SupportedLanguages CodingLang {
            get { return (SupportedLanguages)GetValue(CodingLangProperty); }
            set { SetValue(CodingLangProperty, value); }
        }

        #endregion

        #region constructor

        public Editor() {
            InitializeComponent();
        }

        #endregion

        #region events

        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);
            
            var code = GetHashCode();
            var formattingType = LanguageFeatureInfo.GetFormattingType(CodingLang);

            ConfigManager.AddEditorConfig(code, new Config {
                Language = CodingLang,
                FormattingType = formattingType
            });

            HelpersPanel.SetUpMessaging();
            InputPanel.SetUpMessaging();
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs evt) {
            if (evt.VerticalChange != 0) {
                var firstIdx = (int)Math.Round(evt.VerticalOffset / TextConfiguration.GetCharSize().Height);
                var lastIdx = (int)Math.Round((evt.VerticalOffset + evt.ViewportHeight) / TextConfiguration.GetCharSize().Height);

                Postbox.InstanceFor(GetHashCode()).Put(new ScrollChangedMessage {
                    FirstVisibleLineIndex = firstIdx,
                    LastVisibleLineIndex = lastIdx,
                    LinesScrolled = Math.Abs((int)Math.Ceiling(evt.VerticalChange / TextConfiguration.GetCharSize().Height))
                });
            }
        }

        #endregion

    }
}

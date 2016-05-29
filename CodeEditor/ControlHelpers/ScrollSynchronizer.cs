using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CodeEditor.ControlHelpers {
    /// <summary>
    /// code snippet taken from http://www.codeproject.com/Articles/39244/Scroll-Synchronization
    /// </summary>
    public class ScrollSynchronizer : DependencyObject {

        #region fields

        private static Dictionary<ScrollViewer, string> scrollViewers =
            new Dictionary<ScrollViewer, string>();

        private static Dictionary<string, double> verticalScrollOffsets =
            new Dictionary<string, double>();

        public static readonly DependencyProperty ScrollGroupProperty =
            DependencyProperty.RegisterAttached(
            "ScrollGroup",
            typeof(string),
            typeof(ScrollSynchronizer),
            new PropertyMetadata(new PropertyChangedCallback(
                OnScrollGroupChanged)));

        #endregion

        #region public methods

        public static void SetScrollGroup(DependencyObject obj, string scrollGroup) {
            obj.SetValue(ScrollGroupProperty, scrollGroup);
        }

        public static string GetScrollGroup(DependencyObject obj) {
            return (string)obj.GetValue(ScrollGroupProperty);
        }

        #endregion

        #region methods

        private static void OnScrollGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var scrollViewer = d as ScrollViewer;
            if (scrollViewer != null) {
                if (!string.IsNullOrEmpty((string)e.OldValue)) {
                    // Remove scrollviewer
                    if (scrollViewers.ContainsKey(scrollViewer)) {
                        scrollViewer.ScrollChanged -=
                          new ScrollChangedEventHandler(ScrollViewer_ScrollChanged);
                        scrollViewers.Remove(scrollViewer);
                    }
                }

                if (!string.IsNullOrEmpty((string)e.NewValue)) {
                    if (verticalScrollOffsets.Keys.Contains((string)e.NewValue)) {
                        scrollViewer.ScrollToVerticalOffset(verticalScrollOffsets[(string)e.NewValue]);
                    } else {
                        verticalScrollOffsets.Add((string)e.NewValue, scrollViewer.VerticalOffset);
                    }

                    // Add scrollviewer
                    scrollViewers.Add(scrollViewer, (string)e.NewValue);
                    scrollViewer.ScrollChanged += new ScrollChangedEventHandler(ScrollViewer_ScrollChanged);
                }
            }
        }

        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) {
            if (e.VerticalChange != 0 || e.HorizontalChange != 0) {
                var changedScrollViewer = sender as ScrollViewer;

                Scroll(changedScrollViewer);
            }
        }

        private static void Scroll(ScrollViewer changedScrollViewer) {
            var group = scrollViewers[changedScrollViewer];

            verticalScrollOffsets[group] = changedScrollViewer.VerticalOffset;

            foreach (var scrollViewer in scrollViewers.Where((s) => s.Value ==
                                              group && s.Key != changedScrollViewer)) {
                if (scrollViewer.Key.VerticalOffset != changedScrollViewer.VerticalOffset) {
                    scrollViewer.Key.ScrollToVerticalOffset(changedScrollViewer.VerticalOffset);
                }
            }
        }

        #endregion

    }
}

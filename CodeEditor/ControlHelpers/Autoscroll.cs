using System;
using System.Windows;
using System.Windows.Controls;

namespace CodeEditor.ControlHelpers {
    public class Autoscroll {

        #region fields

        public static readonly DependencyProperty AlwaysScrollToEndProperty = DependencyProperty.RegisterAttached(
            "AlwaysScrollToEnd", 
            typeof(bool), 
            typeof(Autoscroll), 
            new PropertyMetadata(false, AlwaysScrollToEndChanged));

        private static bool autoScrollVertical;

        private static bool autoScrollHorizontal;

        #endregion

        #region public methods

        public static bool GetAlwaysScrollToEnd(ScrollViewer scroll) {
            if (scroll == null) {
                throw new ArgumentNullException("scroll");
            }

            return (bool)scroll.GetValue(AlwaysScrollToEndProperty);
        }

        public static void SetAlwaysScrollToEnd(ScrollViewer scroll, bool alwaysScrollToEnd) {
            if (scroll == null) {
                throw new ArgumentNullException("scroll");
            }

            scroll.SetValue(AlwaysScrollToEndProperty, alwaysScrollToEnd);
        }

        #endregion

        #region methods

        private static void AlwaysScrollToEndChanged(object sender, DependencyPropertyChangedEventArgs e) {
            var scroll = sender as ScrollViewer;

            if (scroll != null) {
                var alwaysScrollToEnd = (e.NewValue != null) && (bool)e.NewValue;

                if (alwaysScrollToEnd) {
                    scroll.ScrollToEnd();
                    scroll.ScrollChanged += ScrollChanged;
                } else {
                    scroll.ScrollChanged -= ScrollChanged;
                }
            } else {
                throw new InvalidOperationException("The attached AlwaysScrollToEnd property can only be applied to ScrollViewer instances.");
            }
        }

        private static void ScrollChanged(object sender, ScrollChangedEventArgs e) {
            var scroll = sender as ScrollViewer;

            if (scroll == null) {
                throw new InvalidOperationException("The attached AlwaysScrollToEnd property can only be applied to ScrollViewer instances.");
            }

            // User scroll event : set or unset autoscroll mode
            if (e.ExtentHeightChange == 0) {
                autoScrollVertical = Math.Round(scroll.VerticalOffset) == Math.Round(scroll.ScrollableHeight);
            }
            if (e.ExtentWidthChange == 0) {
                autoScrollHorizontal = Math.Round(scroll.HorizontalOffset) == Math.Round(scroll.ScrollableWidth);
            }

            // Content scroll event : autoscroll eventually
            if (autoScrollVertical && e.ExtentHeightChange != 0) {
                scroll.ScrollToVerticalOffset(scroll.ExtentHeight);
            }
            if (autoScrollHorizontal && e.ExtentWidthChange != 0) {
                scroll.ScrollToHorizontalOffset(scroll.ExtentWidth);
            }
        }

        #endregion

    }
}

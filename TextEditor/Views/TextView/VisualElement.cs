﻿using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace TextEditor.Views.TextView {
    internal class VisualTextLine : DrawingVisual {
        public int Index { get; set; }

        public VisualTextLine(TextLine textLine, double top, int index) {
            Index = index;

            Redraw(textLine, top);
        }

        public void Redraw(TextLine textLine, double top) {
            using (var drawingContext = RenderOpen()) {
                textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
            }
        }

        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters) {
            return null;
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters) {
            return null;
        }
    }
}

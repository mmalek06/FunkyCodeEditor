using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using TextEditor.DataStructures;

namespace TextEditor.Views.SelectionView {
    internal class VisualElement : DrawingVisual {
        private double radiusX;
        private double radiusY;
        private Pen pen;
        private Brush brush;

        public VisualElement() {
            ConfigureRadiuses();
            ConfigurePen();
            ConfigureBrush();
        }

        public void Draw(IEnumerable<PointsPair> boundingBoxes) {
            Rect rectangle;

            using (var drawingContext = RenderOpen()) {
                foreach (var pair in boundingBoxes) {
                    rectangle = new Rect(pair.StartingPoint, pair.EndingPoint);

                    drawingContext.DrawRoundedRectangle(brush, pen, rectangle, radiusX, radiusY);
                }
            }
        }
        
        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters) {
            return null;
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters) {
            return null;
        }

        private void ConfigureRadiuses() {
            radiusX = 5;
            radiusY = 5;
        }

        private void ConfigureBrush() {
            brush = Brushes.LightBlue;
        }

        private void ConfigurePen() {
            pen = new Pen(Brushes.Black, 0.5);
        }
    }
}

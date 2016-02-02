using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using TextEditor.DataStructures;

namespace TextEditor.Views.SelectionView {
    internal class VisualElement : DrawingVisual {
        private Brush brush;

        public VisualElement() {
            ConfigureBrush();
        }

        public void Draw(IEnumerable<PointsPair> boundingBoxes) {
            Rect rectangle;

            using (var drawingContext = RenderOpen()) {
                foreach (var pair in boundingBoxes) {
                    rectangle = new Rect(pair.StartingPoint, pair.EndingPoint);

                    drawingContext.DrawRectangle(brush, null, rectangle);
                }
            }
        }
        
        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters) {
            return null;
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters) {
            return null;
        }

        private void ConfigureBrush() {
            brush = Brushes.Orange;
        }
    }
}

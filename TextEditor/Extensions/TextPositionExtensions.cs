﻿using System.Windows;
using TextEditor.DataStructures;

namespace TextEditor.Extensions {
    public static class TextPositionExtensions {
        public static Point GetPositionRelativeToParent(this TextPosition position) {
            var charSize = StringExtensions.GetCharSize();
            var point = new Point {
                X = position.Column * charSize.Width,
                Y = position.Line * charSize.Height
            };

            return point;
        }
    }
}

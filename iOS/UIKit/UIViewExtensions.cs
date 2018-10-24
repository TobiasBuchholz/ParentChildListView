using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;

namespace UIKit
{
    public static class UIViewExtensions
    {
        public static void SetX(this UIView view, nfloat x)
        {
            var frame = view.Frame;
            frame.X = x;
            view.Frame = frame;
        }

        public static void SetY(this UIView view, nfloat y)
        {
            var frame = view.Frame;
            frame.Y = y;
            view.Frame = frame;
        }

        public static void SetWidth(this UIView view, nfloat width)
        {
            var frame = view.Frame;
            frame.Width = width;
            view.Frame = frame;
        }

        public static void SetHeight(this UIView view, nfloat height)
        {
            var frame = view.Frame;
            frame.Height = height;
            view.Frame = frame;
        }

        public static IEnumerable<NSIndexPath> ToNSIndexPaths(this IEnumerable<int> @this, int section = 0)
        {
            return @this.Select(x => NSIndexPath.FromRowSection(x, section));
        }
    }
}
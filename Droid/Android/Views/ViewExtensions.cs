namespace Android.Views
{
    public static class ViewExtensions
    {
        public static int GetMeasuredHeight(this View view)
        {
            var widthMeasureSpec = View.MeasureSpec.MakeMeasureSpec(view.Width, MeasureSpecMode.AtMost);
            var heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
            view.Measure(widthMeasureSpec, heightMeasureSpec);
            return view.MeasuredHeight;
        }
        
        public static int GetMeasuredWidth(this View view)
        {
            var widthMeasureSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
            var heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(view.Height, MeasureSpecMode.AtMost);
            view.Measure(widthMeasureSpec, heightMeasureSpec);
            return view.MeasuredWidth;
        }

        public static void SetHeight(this View view, int height)
        {
            var layoutParams = view.LayoutParameters;
            layoutParams.Height = height;
            view.LayoutParameters = layoutParams;
        }
    }
}
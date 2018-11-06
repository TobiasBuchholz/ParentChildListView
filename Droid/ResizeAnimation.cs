using Android.Views;
using Android.Views.Animations;

namespace ParentChildListView.UI.Droid
{
    public class ResizeAnimation : Animation
    {
        private readonly View _view;
        private readonly int _startHeight;
        private readonly int _destHeight;

        public ResizeAnimation(View view, int startHeight, int destHeight)
        {
            _view = view;
            _startHeight = startHeight;
            _destHeight = destHeight;
        }

        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            var height = (int) (_startHeight + (_destHeight - _startHeight) * interpolatedTime);
            _view.LayoutParameters.Height = height;
            _view.RequestLayout();
        }

        public override bool WillChangeBounds()
        {
            return true;
        }
    }
}
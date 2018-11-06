using System;
using Android.Support.V7.Widget;
using Android.Views;

namespace ParentChildListView.UI.Droid
{
    public sealed class RecyclerViewOutSideTouchListener : RecyclerView.SimpleOnItemTouchListener
    {
        private readonly Action _onTouchedOutSide;

        public RecyclerViewOutSideTouchListener(Action onTouchedOutSide)
        {
            _onTouchedOutSide = onTouchedOutSide;
        }

        public override bool OnInterceptTouchEvent(RecyclerView rv, MotionEvent e)
        {
            if(e.Action == MotionEventActions.Up && rv.FindChildViewUnder(e.GetX(), e.GetY()) == null) {
                _onTouchedOutSide?.Invoke();
            }
            return base.OnInterceptTouchEvent(rv, e);
        }
    }
}
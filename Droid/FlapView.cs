using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.View.Animation;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ParentChildListView.UI.Droid
{
    public sealed class FlapView : RelativeLayout
    {
        private int _flapHeight;
        private FastOutSlowInInterpolator _interpolator;
        private View _contentView;
        private View _flapTitleBackgroundView;
        private TextView _flapTitleView;
        private View _arrowIcon;
        private View _backgroundView;
        private int _contentHeight;

        public FlapView(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public FlapView(Context context) 
            : base(context)
        {
            Initialize(context);
        }

        public FlapView(Context context, IAttributeSet attrs) 
            : base(context, attrs)
        {
            Initialize(context);
        }

        public FlapView(Context context, IAttributeSet attrs, int defStyleAttr) 
            : base(context, attrs, defStyleAttr)
        {
            Initialize(context);
        }

        public FlapView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) 
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize(context);
        }

        private void Initialize(Context context)
        {
            _flapHeight = context.Resources.GetDimensionPixelSize(Resource.Dimension.height_flap);
            _interpolator = new FastOutSlowInInterpolator();
            Inflate(context, Resource.Layout.flap_view, this);
            Alpha = 0.97f;

            InitFlapTitleBackgroundView();
            InitFlapTitleView();
            InitArrowIcon();
            InitBackgroundView();
        }

        private void InitFlapTitleBackgroundView()
        {
            _flapTitleBackgroundView = FindViewById(Resource.Id.flap_view_flap_title_background);
            _flapTitleBackgroundView.Click += HandleFlapTitleTapped;
        }

        private void HandleFlapTitleTapped(object sender, EventArgs e)
        {
            if(_contentView.TranslationY < 0) {
                Open();
            } else {
                Close();
            }
        }

        private void Open()
        {
            this.SetHeight(_contentHeight + _flapHeight);
            _backgroundView.Animate().Alpha(0.5f);
            _arrowIcon.Animate().Rotation(180f).SetInterpolator(_interpolator);
            Animate().Alpha(1f).SetDuration(100).SetStartDelay(0);
            _contentView
                .Animate()
                .TranslationY(0)
                .SetInterpolator(_interpolator)
                .SetListener(null);
        }

        public void Close()
        {
            _backgroundView.Animate().Alpha(0f);
            _arrowIcon.Animate().Rotation(0f).SetInterpolator(_interpolator);
            Animate().Alpha(0.97f).SetDuration(500).SetStartDelay(300);
            _contentView
                .Animate()
                .TranslationY(-_contentView.Height - _flapHeight)
                .SetInterpolator(_interpolator)
                .SetListener(new AnimatorListener(onEndAction:_ => this.SetHeight(_flapHeight)));
        }

        private void InitFlapTitleView()
        {
            _flapTitleView = FindViewById<TextView>(Resource.Id.flap_view_flap_title);
        }
        
        private void InitArrowIcon()
        {
            _arrowIcon = FindViewById(Resource.Id.flap_view_arrow_icon);
        }
        
        private void InitBackgroundView()
        {
            _backgroundView = FindViewById(Resource.Id.flap_view_background);
            _backgroundView.Click += HandleBackgroundTapped;
        }

        private void HandleBackgroundTapped(object sender, EventArgs e)
        {
            Close();
        }

        public View ContentView {
            set {
                if(_contentView != null) {
                    RemoveView(_contentView);
                }

                _contentView = value;
                var layoutParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                layoutParams.AddRule(LayoutRules.Below, Resource.Id.flap_view_flap_title_background);
                AddView(_contentView, layoutParams);
                BringChildToFront(_flapTitleBackgroundView);
                BringChildToFront(_flapTitleView);
                BringChildToFront(_arrowIcon);
                _contentHeight = _contentView.GetMeasuredHeight();
                _contentView.TranslationY = -_contentHeight - _flapHeight;

                this.SetHeight(_flapHeight);
            }
        }

        public string Title {
            set => _flapTitleView.Text = value;
        }
    }
}
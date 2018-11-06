using System;
using System.Security.Principal;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;

namespace ParentChildListView.UI.iOS
{
    public sealed class FlapView : UIView
    {
        public const float FlapHeight = 60;

        private readonly nfloat _contentHeight;
        private NSLayoutConstraint _heightConstraint;
        private UIButton _backgroundButton;
        private UIButton _flapTitleButton;
        private UILabel _arrowIcon;
        private UIView _contentView;
        
        public FlapView(nfloat contentHeight)
        {
            _contentHeight = contentHeight;
            Alpha = 0.97f;
            
            InitHeightConstraint();
            InitBackgroundButton();
            InitFlapTitleButton();
            InitArrowIcon();
        }

        private void InitHeightConstraint()
        {
            _heightConstraint = NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, FlapHeight);
            AddConstraint(_heightConstraint);
        }

        private void InitBackgroundButton()
        {
            _backgroundButton = new UIButton();
            _backgroundButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _backgroundButton.BackgroundColor = new UIColor(0, 0, 0, 0.5f);
            _backgroundButton.Alpha = 0f;
            AddSubview(_backgroundButton);
            
            _backgroundButton.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            _backgroundButton.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
            _backgroundButton.LeadingAnchor.ConstraintEqualTo(LeadingAnchor).Active = true;
            _backgroundButton.TrailingAnchor.ConstraintEqualTo(TrailingAnchor).Active = true;

            _backgroundButton.TouchUpInside += HandleBackgroundTapped;
        }

        private void HandleBackgroundTapped(object sender, System.EventArgs e)
        {
            CloseAsync().Ignore(); 
        }

        private async Task CloseAsync()
        {
            Animate(0.3f, () => _backgroundButton.Alpha = 0f);
            await AnimateNotifyAsync(0.3f, 0f, UIViewAnimationOptions.CurveEaseOut, () => {
                _contentView.Transform = CGAffineTransform.MakeTranslation(0, -_contentView.Frame.Height - FlapHeight);
                _arrowIcon.Transform = CGAffineTransform.MakeRotation((nfloat) (-Math.PI * 2));
            });
            Animate(0.5f, () => Alpha = 0.97f);
            _heightConstraint.Constant = FlapHeight;
        }

        private void InitFlapTitleButton()
        {
            _flapTitleButton = new UIButton();
            _flapTitleButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _flapTitleButton.BackgroundColor = UIColor.Gray;
            _flapTitleButton.TitleLabel.TextColor = UIColor.White;
            AddSubview(_flapTitleButton);

            _flapTitleButton.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            _flapTitleButton.LeadingAnchor.ConstraintEqualTo(LeadingAnchor).Active = true;
            _flapTitleButton.TrailingAnchor.ConstraintEqualTo(TrailingAnchor).Active = true;
            _flapTitleButton.HeightAnchor.ConstraintEqualTo(FlapHeight).Active = true;
            
            _flapTitleButton.TouchUpInside += HandleFlapTitleTapped;
        }

        private void HandleFlapTitleTapped(object Sender, System.EventArgs e)
        {
            if(_contentView.Transform.y0 < 0) {
                Open();
            } else {
                CloseAsync().Ignore();
            }
        }

        private void Open()
        {
            _heightConstraint.Constant = _contentHeight + FlapHeight;
            
            Animate(0.3f, () => _backgroundButton.Alpha = 1f);
            Animate(0.1f, () => Alpha = 1f);
            Animate(0.3f, 0f, UIViewAnimationOptions.CurveEaseIn ,() => {
                _contentView.Transform = CGAffineTransform.MakeTranslation(0, 0);
                _arrowIcon.Transform = CGAffineTransform.MakeRotation((nfloat) Math.PI);
            }, null);
        }
        
        private void InitArrowIcon()
        {
            _arrowIcon = new UILabel();
            _arrowIcon.TranslatesAutoresizingMaskIntoConstraints = false;
            _arrowIcon.TextColor = UIColor.White;
            _arrowIcon.Text = "\\/";
            AddSubview(_arrowIcon);

            _arrowIcon.CenterYAnchor.ConstraintEqualTo(_flapTitleButton.CenterYAnchor).Active = true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _backgroundButton.TouchUpInside -= HandleBackgroundTapped;
            _flapTitleButton.TouchUpInside -= HandleFlapTitleTapped;
        }

        public UIView ContentView {
            set {
                _contentView?.RemoveFromSuperview();
                _contentView = value;
                _contentView.TranslatesAutoresizingMaskIntoConstraints = false;
                AddSubview(_contentView);
                BringSubviewToFront(_flapTitleButton);
                BringSubviewToFront(_arrowIcon);
                
                _contentView.TopAnchor.ConstraintEqualTo(_flapTitleButton.BottomAnchor).Active = true;
                _contentView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor).Active = true;
                _contentView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor).Active = true;

                _contentView.Transform = CGAffineTransform.MakeTranslation(0, -_contentHeight - FlapHeight);
            }
        }

        public string Title {
            set {
                _flapTitleButton.SetTitle(value, UIControlState.Normal);
                _arrowIcon.CenterXAnchor.ConstraintEqualTo(_flapTitleButton.CenterXAnchor, value.Length * 3 * UIScreen.MainScreen.Scale).Active = true;
            }
        }
    }
}
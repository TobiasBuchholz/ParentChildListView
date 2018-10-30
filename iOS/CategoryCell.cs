using System;
using CoreGraphics;
using Foundation;
using ParentChildListView.Core;
using UIKit;

namespace ParentChildListView.UI.iOS
{
    public sealed class CategoryCell : UICollectionViewCell
    {
        public const string CellIdentifier = nameof(CategoryCell);

        private UILabel _label;
        
        [Export("initWithFrame:")]
        public CategoryCell(CGRect frame) 
            : base (frame)
        {
            BackgroundView = new UIView();

            InitLabel();
            InitBottomDivider();
        }

        private void InitLabel()
        {
            _label = new UILabel();
            _label.TranslatesAutoresizingMaskIntoConstraints = false;

            ContentView.AddSubview(_label);

            _label.CenterXAnchor.ConstraintEqualTo(ContentView.CenterXAnchor).Active = true;
            _label.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;
        }

        private void InitBottomDivider()
        {
            var divider = new UIView();
            divider.TranslatesAutoresizingMaskIntoConstraints = false;
            divider.BackgroundColor = UIColor.Black;
            
            ContentView.AddSubview(divider);

            divider.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
            divider.WidthAnchor.ConstraintEqualTo(ContentView.WidthAnchor).Active = true;
            divider.HeightAnchor.ConstraintEqualTo(1).Active = true;
        }

        public void SetupCell(Category category)
        {
            _label.Text = category.Name;
        }

        public ParentChildItemState State {
            set => BackgroundView.BackgroundColor = GetColorForState(value);
        }

        private UIColor GetColorForState(ParentChildItemState state)
        {
            switch(state) {
                case ParentChildItemState.Root:
                    return UIColor.Blue;
                case ParentChildItemState.Parent:
                    return UIColor.Purple;
                case ParentChildItemState.Child:
                    return UIColor.Orange;
                case ParentChildItemState.Selected:
                    return UIColor.Red;
            }
            throw new ArgumentException($"Can't get color for unknown state {state}");
        }
    }
}
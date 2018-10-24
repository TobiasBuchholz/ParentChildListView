using CoreGraphics;
using Foundation;
using UIKit;

namespace ParentChildListView.UI.iOS
{
    public sealed class CategoryCell : UICollectionViewCell
    {
        public const string CellIdentifier = nameof(CategoryCell);

        private UILabel _label;
        private bool _isSelected;
        
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

        public void SetupRootCell(Category category)
        {
            BackgroundView.BackgroundColor = UIColor.Purple;
            _label.Text = $"{category.Name} (Root)";
        }

        public void SetupParentCell(Category category)
        {
            BackgroundView.BackgroundColor = UIColor.Purple;
            _label.Text = category.Name;
        }

        public void SetupSelectedCell(Category category)
        {
            BackgroundView.BackgroundColor = UIColor.Red;
            _label.Text = category.Name;
        }
        
        public void SetupChildCell(Category category)
        {
            BackgroundView.BackgroundColor = UIColor.Orange;
            _label.Text = category.Name;
        }
    }
}
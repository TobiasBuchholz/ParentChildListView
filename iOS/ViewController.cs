using System;
using System.Linq;
using CoreGraphics;
using ParentChildListView.Core;
using ParentChildListView.Core.TreeNodes;
using ParentChildListView.UI.iOS.EventArgs;
using UIKit;

namespace ParentChildListView.UI.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) 
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            View.BackgroundColor = UIColor.Black;
            
            var topView = new UIView { BackgroundColor = UIColor.DarkGray };
            topView.TranslatesAutoresizingMaskIntoConstraints = false;
            View.AddSubview(topView);
            topView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            topView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            topView.TopAnchor.ConstraintEqualTo(View.TopAnchor, 0).Active = true;
            topView.HeightAnchor.ConstraintEqualTo(150).Active = true;
            
            var flatLayout = CreateCollectionViewLayout();
            var flatCollectionView = new UICollectionView(CGRect.Empty, flatLayout);
            var flatDataSource = new CategoriesDataSource(flatLayout.ItemSize.Height);
            var flatCollectionViewDelegate = new MyCollectionViewDelegate(flatCollectionView);
            flatCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;
            flatCollectionView.RegisterClassForCell(typeof(CategoryCell), CategoryCell.CellIdentifier);
            flatCollectionView.BackgroundColor = UIColor.DarkGray;
            flatCollectionView.DataSource = flatDataSource;
            flatCollectionView.Delegate = flatCollectionViewDelegate;
            flatDataSource.CurrentNode = Category.CreateFlatDummyCategories().ToRootTreeNodes()[0];
            View.AddSubview(flatCollectionView);

            flatCollectionView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            flatCollectionView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            flatCollectionView.TopAnchor.ConstraintEqualTo(topView.BottomAnchor).Active = true;
            flatCollectionView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
            
//            flatCollectionViewDelegate.OnItemSelected += (s, e) => CollectionViewDelegateOnOnItemSelected(e, flatDataSource);
            
            topView.LayoutIfNeeded();
            
            var flapViewHeight = View.Frame.Height - (topView.Bounds.Height + FlapView.FlapHeight);
            var flapView = new FlapView(flapViewHeight);
            flapView.TranslatesAutoresizingMaskIntoConstraints = false;
            flapView.BackgroundColor = UIColor.Clear;
            flapView.Title = "Filter";
            View.AddSubview(flapView);
            
            flapView.LeadingAnchor.ConstraintEqualTo(flatCollectionView.LeadingAnchor).Active = true;
            flapView.TrailingAnchor.ConstraintEqualTo(flatCollectionView.TrailingAnchor).Active = true;
            flapView.TopAnchor.ConstraintEqualTo(flatCollectionView.TopAnchor).Active = true;
            
//            flatCollectionViewDelegate.OnScrolled += (s, e) => flapView.OnScrolled(flatCollectionView.ContentOffset.Y);
            View.BringSubviewToFront(topView);
            
            
            var layout = CreateCollectionViewLayout();
            var collectionView = new UICollectionView(CGRect.Empty, layout);
            var dataSource = new CategoriesDataSource(60);
            var collectionViewDelegate = new MyCollectionViewDelegate(collectionView);
            collectionView.TranslatesAutoresizingMaskIntoConstraints = false;
            collectionView.RegisterClassForCell(typeof(CategoryCell), CategoryCell.CellIdentifier);
            collectionView.BackgroundColor = UIColor.Clear;
            collectionView.DataSource = dataSource;
            collectionView.Delegate = collectionViewDelegate;
            
            var heightConstraint = NSLayoutConstraint.Create(collectionView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, flapViewHeight);
            collectionView.AddConstraint(heightConstraint);
            dataSource.HeightWillChange += (s, e) => UIView.Animate(0.3f, () => {
                heightConstraint.Constant = e > flapViewHeight ? flapViewHeight : e;
                View.LayoutIfNeeded();
            });
            
            dataSource.CurrentNode = Category.CreateDummyCategories().ToRootTreeNodes()[0];
            collectionViewDelegate.OnItemSelected += (s, e) => CollectionViewDelegateOnOnItemSelected(e, dataSource);

            flapView.ContentView = collectionView;
        }

        private void CollectionViewDelegateOnOnItemSelected(ItemSelectedEventArgs e, CategoriesDataSource dataSource)
        {
            dataSource.ItemSelected(e.CollectionView, e.IndexPath);
            System.Diagnostics.Debug.WriteLine($"tap");
        }

        private static UICollectionViewFlowLayout CreateCollectionViewLayout()
        {
            var layout = new ParentChildCollectionViewFlowLayout();
            layout.MinimumLineSpacing = 0;
            layout.ItemSize = new CGSize(UIScreen.MainScreen.Bounds.Width, 60);
            return layout;
        }
    }
}

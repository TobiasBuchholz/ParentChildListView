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

            var layout = CreateCollectionViewLayout();
            var collectionView = new UICollectionView(CGRect.Empty, layout);
            var dataSource = new CategoriesDataSource();
            var collectionViewDelegate = new MyCollectionViewDelegate(collectionView);
            collectionView.TranslatesAutoresizingMaskIntoConstraints = false;
            collectionView.RegisterClassForCell(typeof(CategoryCell), CategoryCell.CellIdentifier);
            collectionView.BackgroundColor = UIColor.DarkGray;
            collectionView.DataSource = dataSource;
            collectionView.Delegate = collectionViewDelegate;
            dataSource.CurrentNode = Category.CreateDummyCategories().ToRootTreeNodes()[0];
            View.AddSubview(collectionView);

            collectionView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            collectionView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            collectionView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            collectionView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;

            collectionViewDelegate.OnItemSelected += (s, e) => CollectionViewDelegateOnOnItemSelected(e, dataSource);
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

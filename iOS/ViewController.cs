using System;
using System.Linq;
using CoreGraphics;
using ParentChildListView.UI.iOS.EventArgs;
using PressMatrix.Utility.TreeNodes;
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
            var dataSource = new ParentChildListDataSource();
            var collectionView = new UICollectionView(CGRect.Empty, layout);
            var collectionViewDelegate = new ParentChildListCollectionViewDelegate(collectionView);
            collectionView.TranslatesAutoresizingMaskIntoConstraints = false;
            collectionView.RegisterClassForCell(typeof(CategoryCell), CategoryCell.CellIdentifier);
            collectionView.BackgroundColor = UIColor.DarkGray;
            collectionView.DataSource = dataSource;
            collectionView.Delegate = collectionViewDelegate;
            dataSource.CurrentNode = Category.CreateDummyCategories().ToRootTreeNodes()[0];
//            dataSource.Items = Category.CreateDummyCategories().ToList();
            
            View.AddSubview(collectionView);

            collectionView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            collectionView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            collectionView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            collectionView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
            
            collectionViewDelegate.OnItemSelected += CollectionViewDelegateOnOnItemSelected;
        }

        private void CollectionViewDelegateOnOnItemSelected(object sender, ItemSelectedEventArgs e)
        {
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

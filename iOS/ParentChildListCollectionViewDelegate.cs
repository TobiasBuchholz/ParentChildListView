using System;
using Foundation;
using ParentChildListView.UI.iOS.EventArgs;
using UIKit;

namespace ParentChildListView.UI.iOS
{
	public class ParentChildListCollectionViewDelegate : UICollectionViewDelegate
	{
		public event EventHandler OnScrolled;
		public event EventHandler OnDecelerationEnded;
        public event EventHandler ScrolledToBottom;
		public event EventHandler<ItemSelectedEventArgs> OnItemSelected;
		public event EventHandler<ItemLongPressedEventArgs> OnItemLongPressed;

		public ParentChildListCollectionViewDelegate(UICollectionView collectionView)
		{
			collectionView.AddItemLongClickListener(cell => OnItemLongPressed?.Invoke(collectionView, new ItemLongPressedEventArgs { Cell = cell }));
		}

		public override void Scrolled(UIScrollView scrollView)
		{
            OnScrolled?.Invoke(scrollView, System.EventArgs.Empty);
		}

		public override void DecelerationEnded(UIScrollView scrollView)
		{
            OnDecelerationEnded?.Invoke(scrollView, System.EventArgs.Empty);
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			OnItemSelected?.Invoke(collectionView, new ItemSelectedEventArgs { IndexPath = indexPath });
			
			var dataSource = (ParentChildListDataSource) collectionView.DataSource;
			dataSource.ItemSelected(collectionView, indexPath);
		}

        public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
        {
            if (indexPath.Item == collectionView.NumberOfItems() - 1) {
                ScrolledToBottom?.Invoke(collectionView, System.EventArgs.Empty);
            }
        }
	}
}

using Foundation;
using UIKit;

namespace ParentChildListView.UI.iOS.EventArgs
{
	public sealed class ItemSelectedEventArgs : System.EventArgs 
	{
		public ItemSelectedEventArgs(UICollectionView collectionView, NSIndexPath indexPath)
		{
			CollectionView = collectionView;
			IndexPath = indexPath;
		}

		public UICollectionView CollectionView { get; }
		public NSIndexPath IndexPath { get; }
	}
}
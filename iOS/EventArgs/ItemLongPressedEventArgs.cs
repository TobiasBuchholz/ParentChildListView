using UIKit;

namespace ParentChildListView.UI.iOS.EventArgs
{
	public sealed class ItemLongPressedEventArgs : System.EventArgs 
	{
		public ItemLongPressedEventArgs(UICollectionViewCell cell)
		{
			Cell = cell;
		}

		public UICollectionViewCell Cell { get; }
	}
}
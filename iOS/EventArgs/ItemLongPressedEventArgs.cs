using UIKit;

namespace ParentChildListView.UI.iOS.EventArgs
{
	public sealed class ItemLongPressedEventArgs : System.EventArgs 
	{
		public UICollectionViewCell Cell { get; set; }
	}
}
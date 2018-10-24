using Foundation;

namespace ParentChildListView.UI.iOS.EventArgs
{
	public sealed class ItemSelectedEventArgs : System.EventArgs 
	{
		public NSIndexPath IndexPath { get; set; }
	}
}
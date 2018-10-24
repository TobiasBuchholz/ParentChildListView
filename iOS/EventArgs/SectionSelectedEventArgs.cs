namespace ParentChildListView.UI.iOS.EventArgs
{
    public sealed class SectionSelectedEventArgs : System.EventArgs
    {
        public SectionSelectedEventArgs(int section)
        {
            Section = section;
        }

        public int Section { get; }
    }
}
using System.Windows.Forms;

namespace PUTSUSPCOM
{
    public class DealListViewItem : ListViewItem
    {

        public Deal DealData { get; set; }

        public DealListViewItem(Deal data)
            : base()
        {
            Text = data.Description;
            DealData = data;
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageSelector.Infrastructure;

namespace ImageSelector
{
    public partial class EventViewer : UserControl
    {
        public EventViewer()
        {
            InitializeComponent();
        }

        public void Push(ImageAndMouseState data)
        {
            bsEvents.Add(data);

            int notDisplayableRowCount = dataGridView1.RowCount - dataGridView1.DisplayedRowCount(false); // false means partial rows are not taken into acount
            if (notDisplayableRowCount > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = notDisplayableRowCount;
        }
    }
}

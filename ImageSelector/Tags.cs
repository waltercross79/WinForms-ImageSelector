using ImageSelector.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageSelector
{
    public partial class Tags : Form
    {
        public Tags()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();

            var t = new EventHandler<CustomEventArgs<string>>(Save);
            
            CustomEventArgs<string> ea = new CustomEventArgs<string>(txtSelectionText.Text);

            if(t != null)
                Save(this, ea);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

            var t = new EventHandler(Cancel);

            
            if (t != null)
                Cancel(this, e);
        }

        public event EventHandler<CustomEventArgs<string>> Save;

        public event EventHandler Cancel;
    }
}

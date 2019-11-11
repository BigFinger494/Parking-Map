using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingMapbyBigFinger
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();


        }
        public event EventHandler IncrementProgress;

        void SomeMethodWhereProgressHappens()
        {
           
            EventHandler handler = IncrementProgress;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }

            // ... then make some more progress, etc.
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}

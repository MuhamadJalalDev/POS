using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class product : Form
    {
        public product()
        {
            InitializeComponent();
        }

        private void product_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Product' table. You can move, or remove it, as needed.
            this.productTableAdapter.Fill(this.database1DataSet.Product);

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace POS
{
    public partial class AddProduct : Form
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(publicVariables.dbcon);
            String query = "insert into Product(ProductName, Description, Price, QuantityAvailable, Barcode)values(@ProductName, @Description, @Price, @QuantityAvailable, @Barcode) ";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProductName", textBox1.Text);
            cmd.Parameters.AddWithValue("@Description", textBox2.Text);
            cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(textBox3.Text));
            cmd.Parameters.AddWithValue("@QuantityAvailable", Convert.ToInt32(textBox4.Text));
            cmd.Parameters.AddWithValue("@Barcode", textBox5.Text);
            conn.Open();
            int rowsadded = cmd.ExecuteNonQuery();
            MessageBox.Show(rowsadded + " row(s) inserted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
        }

        private void AddProduct_Load(object sender, EventArgs e)
        {
            
        }
    }
}

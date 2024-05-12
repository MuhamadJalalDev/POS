using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class SaleForm : Form
    {
        public SaleForm()
        {
            InitializeComponent();
            dataGridView1.Columns.Add("ProductID", "Product ID");
            dataGridView1.Columns.Add("ProductName", "Product Name");
            dataGridView1.Columns.Add("Quantity", "Quantity");
            dataGridView1.Columns.Add("Price", "Price");
            dataGridView1.Columns.Add("Barcode", "Barcode");
            dataGridView1.Columns.Add("Amount", "Amount");

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void CalculateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Amount"].Value != null)
                {
                    totalAmount += Convert.ToDecimal(row.Cells["Amount"].Value);
                }
            }
            label1.Text = totalAmount.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(publicVariables.dbcon);
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            String query = "select ProductID,ProductName,Price,Barcode from Product where Barcode = @Barcode";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Barcode", textBox1.Text);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                bool found = false;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["Barcode"] != null && row.Cells["Barcode"].Value != null && row.Cells["Barcode"].Value.ToString() == textBox1.Text)
                    {
                        int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                        row.Cells["Quantity"].Value = quantity + 1;
                        decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                        row.Cells["Amount"].Value = Convert.ToDecimal(row.Cells["Quantity"].Value) * Convert.ToDecimal(row.Cells["Price"].Value);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    DataRow newRow = dt.Rows[0];
                    object[] rowData = { newRow["ProductID"], newRow["ProductName"], 1, newRow["Price"], textBox1.Text, newRow["Price"] };
                    dataGridView1.Rows.Add(rowData);
                }

            }
            else
            {
                MessageBox.Show("Item not found for the entered barcode.");
            }
            CalculateTotalAmount();
            textBox1.Text = "";
            textBox1.Focus();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(publicVariables.dbcon))
            {
                conn.Open();

                string saleQuery = @"INSERT INTO Sale (UserID, SaleDate, TotalAmount) VALUES (@UserID, @SaleDate, @TotalAmount);
                  SELECT SCOPE_IDENTITY();";
                SqlCommand saleCmd = new SqlCommand(saleQuery, conn);
                saleCmd.Parameters.AddWithValue("@UserID", publicVariables.UserId);
                saleCmd.Parameters.AddWithValue("@SaleDate", DateTime.Now);
                saleCmd.Parameters.AddWithValue("@TotalAmount", Convert.ToDecimal(label1.Text));
                int saleID = Convert.ToInt32(saleCmd.ExecuteScalar());

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int productID = Convert.ToInt32(row.Cells["ProductID"].Value);
                    int quantitySold = Convert.ToInt32(row.Cells["Quantity"].Value);
                    decimal unitPrice = Convert.ToDecimal(row.Cells["Price"].Value);
                    decimal totalPrice = Convert.ToDecimal(row.Cells["Amount"].Value);

                    string saleItemsQuery = @"INSERT INTO SaleItems (SaleID, ProductID, QuantitySold, UnitPrice, TotalPrice) VALUES (@SaleID, @ProductID, @QuantitySold, @UnitPrice, @TotalPrice);";
                    SqlCommand saleItemsCmd = new SqlCommand(saleItemsQuery, conn);
                    saleItemsCmd.Parameters.AddWithValue("@SaleID", saleID);
                    saleItemsCmd.Parameters.AddWithValue("@ProductID", productID);
                    saleItemsCmd.Parameters.AddWithValue("@QuantitySold", quantitySold);
                    saleItemsCmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                    saleItemsCmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                    saleItemsCmd.ExecuteNonQuery();
                }

                conn.Close();

                MessageBox.Show("SaleID: " + saleID + " inserted successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);




            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SaleForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Product' table. You can move, or remove it, as needed.
            this.productTableAdapter.Fill(this.database1DataSet.Product);

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}

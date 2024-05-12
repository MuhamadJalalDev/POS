using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace POS
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(publicVariables.dbcon);
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            String query = "select * from Login where Username = @username and Password = @password";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username",textBox1.Text);
            cmd.Parameters.AddWithValue("@password", textBox2.Text);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                publicVariables.UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                this.Hide();
                Form1 frm = new Form1();
                frm.Show();
            }
            else
            {
                MessageBox.Show("This username or password does not exist", "Wrong Username / Password", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}

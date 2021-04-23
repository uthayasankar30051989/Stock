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

namespace Stock
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

    

        

        private void button2_Click(object sender, EventArgs e)
        {
            //Checking username and password
            string connectingString = @"Data Source=LAPTOP-2\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True";
            SqlConnection con = new SqlConnection(connectingString);
            SqlDataAdapter sda = new SqlDataAdapter(@"SELECT * FROM [dbo].[Login] where UserName = '" + textBox1.Text+"'and Password= '" +textBox2.Text+"'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                this.Hide();
                StockMains main = new StockMains();
                main.Show();
            }
            else
            {
                MessageBox.Show("Invalid username and password", "Error message",MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1_Click(sender, e);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox1.Focus();
        }
    }
}

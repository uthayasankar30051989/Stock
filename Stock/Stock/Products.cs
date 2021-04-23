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
    public partial class Products : Form
    {
        string connectingString = @"Data Source=LAPTOP-2\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True";
        public Products()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //adding logic to add data in grid view
           // string connectingString = @"Data Source=LAPTOP-2\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True";
            SqlConnection con = new SqlConnection(connectingString);
            con.Open();
            bool status = false;
            if (comboBox1.SelectedIndex == 0)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            //updating data in product and adding in grid view 
            var sqlQuery = "";
            if (ifProductExit(con, textBox1.Text))
            {
                sqlQuery = @"UPDATE[dbo].[Product] SET [ProductName] = '" + textBox2.Text + "',[ProductStatus] = '" + status + "' WHERE [ProductCode] = '" + textBox1.Text + "'";

            }
            else
            {
                sqlQuery = @"INSERT INTO [dbo].[Product]
           ([ProductCode]
           ,[ProductName]
           ,[ProductStatus])
     VALUES
           ('" + textBox1.Text + "','" + textBox2.Text + "','" + status + "')";
            }
           
            SqlCommand cmd = new SqlCommand(sqlQuery, con);

            cmd.ExecuteNonQuery();
            con.Close();

            //reading a data form data base
            LoadData();
        }

        private bool ifProductExit(SqlConnection con, string ProductCode)
        {

            SqlDataAdapter sda = new SqlDataAdapter(@"select 1 from [dbo].[Product] WHERE [ProductCode] = '"+ ProductCode+ "' ", con) ;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if(dt.Rows.Count>0)
            return true;
            else 
                return false;

        }

        public void LoadData()
        {
            //string connectingString = @"Data Source=LAPTOP-2\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True";
            SqlConnection con = new SqlConnection(connectingString);
            SqlDataAdapter sda = new SqlDataAdapter(@"select * from [dbo].[Product]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {

                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();

                if ((bool)item["ProductStatus"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Deactive";
                }
               
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadData();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if ((dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Active"))
            {
                comboBox1.SelectedIndex = 0;

            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string connectingString = @"Data Source=LAPTOP-2\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True";
            SqlConnection con = new SqlConnection(connectingString);

            //check and delete the product data or record if avialable
            var sqlQuery = "";
            if (ifProductExit(con, textBox1.Text))
            {
                con.Open();
                sqlQuery = @"DELETE  FROM [dbo].[Product] WHERE [ProductCode] = '" + textBox1.Text + "'";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();
                
            }
            else
            {
                MessageBox.Show("Product Data is not available", "Error message", MessageBoxButtons.OK, MessageBoxIcon.Error);
              
            }

            //reading a data form data base
            LoadData();
        }
    }
}

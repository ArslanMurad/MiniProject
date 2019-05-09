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

namespace Assets
{
    public partial class Project : Form
    {
        public Project()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            AddProject proj = new AddProject();
            if (e.ColumnIndex == 2)
            {
                SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
                con.Open();
                string cmd = string.Format("Select Id from project where Title='{0}'", dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

                SqlCommand query = new SqlCommand(cmd, con);

                
                int ide = (int)query.ExecuteScalar();
                //conn.Close();

                proj.setpara("Update",
                    ide,
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), 
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()
                    );
                
                proj.Show();
            }

            if (e.ColumnIndex == 3)
            {
                var Result = MessageBox.Show("Delete This Item ?",
                                     "Delete",
                                     MessageBoxButtons.YesNo);

                if (Result == DialogResult.Yes)
                {
                    SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
                    con.Open();

                    String cmd2 = string.Format("Delete from GroupProject where ProjectId=(Select Id from Project where Title='{0}');Delete from ProjectAdvisor where ProjectId=(Select Id from Project where Title='{0}');Delete from Project where Id=(Select Id from Project where Title='{0}');", dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

                    SqlCommand query1 = new SqlCommand(cmd2, con);

                    SqlDataReader reader2 = query1.ExecuteReader();
                    int stdid = 0;
                    while (reader2.Read())
                    {
                        stdid = Convert.ToInt32(reader2["Id"]);
                    }
                    con.Close();
                    

                    MessageBox.Show("Deleted");

                    object sende = null;

                    EventArgs er = null;
                    dataGridView1.Rows.Clear();
                    this.Project_Load(sende, er);


                }
                else
                {
                    MessageBox.Show("Not Deleted");
                }
            }
        }

        public void Project_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
            con.Open();

            string query = "Select Title,Description from Project";
            SqlCommand cmd = new SqlCommand(query, con);


            SqlDataReader reader = cmd.ExecuteReader();

            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Title";
            dataGridView1.Columns[1].Name = "Description";

            while (reader.Read())
            {
                ArrayList row = new ArrayList();

                row.Add(reader["Title"].ToString());
                row.Add(reader["Description"].ToString());

                dataGridView1.Rows.Add(row.ToArray());


            }

            DataGridViewButtonColumn button = new DataGridViewButtonColumn();

            button.UseColumnTextForButtonValue = true;
            button.Name = "Update";
            button.Text = "Update";
            button.HeaderText = "Update";

            DataGridViewButtonColumn button1 = new DataGridViewButtonColumn();

            button1.UseColumnTextForButtonValue = true;
            button1.Name = "Delete";
            button1.Text = "Delete";
            button1.HeaderText = "Delete";

            
            dataGridView1.Columns.Add(button);
            dataGridView1.Columns.Add(button1);
            




            con.Close();
        }

        private void add_Click(object sender, EventArgs e)
        {
            AddProject proj = new AddProject();
            proj.Show();
        }
    }
}

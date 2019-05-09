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
    public partial class GroupProjects : Form
    {

        public int Id { get; set; }

        SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");



        public GroupProjects()
        {
            InitializeComponent();
        }

        private void GroupProjects_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear(); //clear the grid
            comboBox1.Items.Clear();

            con.Open();

            string cmd66 = string.Format("Select Title, AssignmentDate FROM Project INNER JOIN ([ProjectA].[dbo].[Group] INNER JOIN GroupProject ON [ProjectA].[dbo].[Group].Id = GroupProject.GroupId) On Project.Id = GroupProject.ProjectId WHERE [ProjectA].[dbo].[Group].Id={0}", Id);
            
            SqlCommand query66 = new SqlCommand(cmd66, con);

            SqlDataReader reader = query66.ExecuteReader();

            List<string> list = new List<string>();

            while (reader.Read())
            {
                
                ArrayList row = new ArrayList();

                row.Add(reader["Title"].ToString());
                row.Add(reader["AssignmentDate"].ToString());
                list.Add(reader["Title"].ToString());
                

                dataGridView1.Rows.Add(row.ToArray());
            }
            con.Close();
            con.Open();

            string cmd37 = string.Format("Select Title from Project");
            
            SqlCommand query37 = new SqlCommand(cmd37, con);
            SqlDataReader reader1 = query37.ExecuteReader();
            
            while (reader1.Read())
            {

                if ( list.Count == 0 )
                {

                    comboBox1.Items.Add(reader1["Title"]);
                }


            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text.ToString() == "")
            {


                MessageBox.Show("you must select a project");
            }


            else
            {
                con.Open();

                string cmd28 = string.Format("INSERT INTO GroupProject values((Select Id FROM Project WHERE Title = '{0}'),{1},'{2}')", comboBox1.Text.ToString(), Id, DateTime.Now);
                SqlCommand query28 = new SqlCommand(cmd28, con);
                query28.ExecuteNonQuery();

                con.Close();
                object ob = null;
                EventArgs Event = null;
                //create a new instance
                this.GroupProjects_Load(ob, Event);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //for deleting items
            if (e.ColumnIndex == 2)
            {
                var ConfirmResult = MessageBox.Show("Delete this item ?",
                                     "Confirm Delete!",
                                     MessageBoxButtons.YesNo);

                if (ConfirmResult == DialogResult.Yes) //yes delete this
                {
                    con.Open();

                    string cmd19 = string.Format("Delete from GroupProject where GroupId={0} and ProjectId=(Select Id from Project where Title='{1}')", Id, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    SqlCommand query19 = new SqlCommand(cmd19, con);
                    query19.ExecuteNonQuery();

                    con.Close();


                    object ob = null;
                    EventArgs er = null;
                    this.GroupProjects_Load(ob, er);
                }
            }
        }
    }
}

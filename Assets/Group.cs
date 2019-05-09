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
    public partial class Group : Form
    {
        public Group()
        {
            InitializeComponent();
        }

        public void Group_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            

            SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
            con.Open();

            string cmd11 = string.Format("Select ID,Date=Created_On from [ProjectA].[dbo].[Group]");
            SqlCommand query11 = new SqlCommand(cmd11, con);
            SqlDataReader reader = query11.ExecuteReader();


            while (reader.Read())
            {
                ArrayList row = new ArrayList();

                row.Add(reader["ID"].ToString());
                row.Add(reader["Date"].ToString());

                dataGridView1.Rows.Add(row.ToArray());


            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var ConfirmResult = MessageBox.Show("Want to add a new group",
                                     "Confirm Group!!",
                                     MessageBoxButtons.YesNo);

            if (ConfirmResult == DialogResult.Yes)
            {
                SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");

                con.Open();

                string cmd23 = string.Format("insert into [ProjectA].[dbo].[Group] values('{0}')", DateTime.Now);
                SqlCommand query23 = new SqlCommand(cmd23, con);
                query23.ExecuteNonQuery();

                con.Close();

                object OB = null;
                EventArgs er = null;
                this.Group_Load(OB, er);
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");



            if (e.ColumnIndex == 2)
            {

                ViewGroupStudents sdr = new ViewGroupStudents();
                sdr.Id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                sdr.Show();

            }
            if (e.ColumnIndex == 3)
            {

                GroupEvaluation grp = new GroupEvaluation();
                grp.Id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                grp.Show();


            }
            if (e.ColumnIndex == 4)
            {

                GroupProjects prg = new GroupProjects();
                prg.Id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                prg.Show();

            }
            //for deleting stuff
            if (e.ColumnIndex == 5)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this item ??",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                
                    con.Open();

                    String cmd45 = string.Format("Delete FROM GroupEvaluation WHERE GroupId = {0};Delete FROM GroupStudent WHERE GroupId = {0};Delete FROM GroupProject WHERE GroupId = {0};Delete FROM [Group] WHERE Id = {0}",
                        Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()));
                    SqlCommand query45 = new SqlCommand(cmd45, con);
                    query45.ExecuteNonQuery();
                    
                    con.Close();


                    MessageBox.Show("Deleted");

                    object sende = null;
                    EventArgs er = null;
                    dataGridView1.Rows.Clear();
                    this.Group_Load(sende, er);


                }
            }
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assets
{
    public partial class ProjectAdvisor : Form
    {
        public ProjectAdvisor()
        {
            InitializeComponent();
        }


        //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ProjectA"].ToString());

        SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");

        public void ProjectAdvisor_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            con.Open();

            string cmd1 = string.Format("Select Name = FirstName+' '+LastName, Title, Role = Lookup.Value, ASSDate = ProjectAdvisor.AssignmentDate from Lookup INNER join (Project INNER Join (ProjectAdvisor INNER join (Advisor INNER join Person ON Person.Id = Advisor.Id) ON ProjectAdvisor.AdvisorId=Advisor.Id) ON Project.Id = ProjectAdvisor.ProjectId) ON ProjectAdvisor.AdvisorRole = Lookup.Id");
            SqlCommand query1 = new SqlCommand(cmd1, con);
            
            SqlDataReader reader = query1.ExecuteReader();

            //read data form SQL 

            while (reader.Read())
            {
                ArrayList row = new ArrayList();

                row.Add(reader["Name"].ToString());
                row.Add(reader["Title"].ToString());
                row.Add(reader["Role"].ToString());
                row.Add(reader["ASSDate"].ToString());
                



               
                dataGridView1.Rows.Add(row.ToArray());


            }
            con.Close();
        }

        private void addbutton_Click(object sender, EventArgs e)
        {
            //open the addprojectadvisore page
            AddProjectAdvisor a = new AddProjectAdvisor();
            a.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            AddProjectAdvisor a = new AddProjectAdvisor();

            
            if (e.ColumnIndex == 4) // if we want to update

            {
                a.setpara("Update",
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(),
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()
                    );

                a.Show();
            }

            if (e.ColumnIndex == 5) //deleting stuff
            {
                var ConfirmResult = MessageBox.Show("really want to delete this",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);


                if (ConfirmResult == DialogResult.Yes)
                {
                    
                    con.Open();

                    String cmd2 = string.Format("DELETE From ProjectAdvisor WHERE ProjectId = ANY(Select Id from Project where Title='{0}') and AdvisorId = ANY(Select Advisor.Id from Advisor INNER join Person On Advisor.Id = Person.Id WHERE FirstName+' '+LastName='{1}' )",
                        dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(),
                        dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    SqlCommand query2 = new SqlCommand(cmd2, con);

                    query2.ExecuteNonQuery();

                    con.Close();

                    //after deletion 
                    MessageBox.Show("Deleted");
                    object sende = null;
                    EventArgs er = null;
                    dataGridView1.Rows.Clear();
                    //load the project advisor again
                    this.ProjectAdvisor_Load(sende, er);


                }
                else
                {
                    MessageBox.Show("Not Deleted");
                }
            }
        }
    }
}

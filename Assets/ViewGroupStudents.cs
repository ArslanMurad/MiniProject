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
    public partial class ViewGroupStudents : Form
    {


        SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");


        public int Id { get; set; }




        public ViewGroupStudents()
        {
            InitializeComponent();
        }

        private void ViewGroupStudents_Load(object sender, EventArgs e)
        {
            //setting up the cleared page
            dataGridView1.Rows.Clear();
            comboBox1.Items.Clear();

            con.Open();

            string cmd45 = String.Format("Select RegistrationNo, FirstName, LastName, Status = Lookup.Value FROM Person INNER JOIN (Student INNER JOIN (GroupStudent INNER JOIN Lookup ON Lookup.Id = GroupStudent.Status) ON Student.Id = GroupStudent.StudentId)  ON Student.Id = Person.Id WHERE GroupId={0}", Id);
            SqlCommand query45 = new SqlCommand(cmd45, con);
            SqlDataReader reader = query45.ExecuteReader();
            List<string> list = new List<string>();

            while (reader.Read())
            {
                
                ArrayList row = new ArrayList(); //create the array

                row.Add(reader["FirstName"].ToString());
                row.Add(reader["LastName"].ToString());
                row.Add(reader["RegistrationNo"].ToString());
                list.Add(reader["RegistrationNo"].ToString()); //checking the combo box
                row.Add(reader["Status"].ToString());
                //adding stuff to the forum

                dataGridView1.Rows.Add(row.ToArray());
                //displaying it on the forum

            }

            con.Close();

            con.Open();
            
            string cmd44 = "Select RegistrationNo from Student Except Select RegistrationNo from Student INNER join GroupStudent ON Student.Id = GroupStudent.StudentId";
            SqlCommand query44 = new SqlCommand(cmd44, con);
            SqlDataReader reader007 = query44.ExecuteReader();


            while (reader007.Read())
            {



                if (!list.Contains(reader007["RegistrationNo"]))
                {
                    comboBox1.Items.Add(reader007["RegistrationNo"]);
                }




            }

            con.Close();


            

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text.ToString() != "")
            {
                con.Open();

                string cmd22 = string.Format("INSERT INTO GroupStudent values({0},(SELECT Id FROM Student WHERE RegistrationNo = '{1}'),(Select Id FROM Lookup WHERE Value = '{2}'),'{3}')", Id, comboBox1.Text.ToString(), "Active", DateTime.Now);
                SqlCommand query22 = new SqlCommand(cmd22, con);
                query22.ExecuteNonQuery();


                con.Close();


                object ob = null;
                EventArgs er = null;
                this.ViewGroupStudents_Load(ob, er);

            }
            else
            {
                MessageBox.Show("Select any student");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you want to change the group for any student then delete the student from the existing group and add him into new group");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                con.Open();
                
                string cmdgg;
                string msg;

                if (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() == "Active")
                {
                    msg = "InActive";
                    cmdgg = string.Format("Update GroupStudent SET Status = (Select Id FROM Lookup WHERE Value = 'InActive') WHERE GroupId = {0} and StudentId = (Select Id FROM Student WHERE RegistrationNo = '{1}')", Id, dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                }
                else
                {
                    msg = "Active";
                    cmdgg = string.Format("Update GroupStudent SET Status = (Select Id FROM Lookup WHERE Value = 'Active') WHERE GroupId = {0} and StudentId = (Select Id FROM Student WHERE RegistrationNo = '{1}')", Id, dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());

                }

                var confirmResult = MessageBox.Show("Are you sure you want to change the status to" + " " + msg,
                                     msg,
                                     MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    SqlCommand querygg = new SqlCommand(cmdgg, con);
                    querygg.ExecuteNonQuery();
                    con.Close();

                    object ob = null;
                    EventArgs er = null;
                    this.ViewGroupStudents_Load(ob, er);
                }

                con.Close();

            }
            if (e.ColumnIndex == 5)
            {

                var ConfirmResult = MessageBox.Show("Confirm Deletion ??",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);

                if (ConfirmResult == DialogResult.Yes)
                {
                    con.Open();

                    string cmd88 = string.Format("Delete FROM GroupStudent WHERE GroupId = {0} and StudentId = (Select Id FROM Student WHERE RegistrationNo = '{1}')", Id, dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    SqlCommand query88 = new SqlCommand(cmd88, con);
                    query88.ExecuteNonQuery();

                    con.Close();



                    object ob = null;
                    EventArgs er = null;
                    this.ViewGroupStudents_Load(ob, er);

                }
            }
        }
    }
}

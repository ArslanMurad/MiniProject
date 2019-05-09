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
    public partial class Advisors : Form
    {
        public Advisors()
        {
            InitializeComponent();
        }

        public void Advisors_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
            con.Open();

            string query = string.Format("Select Id=s.Id,FirstName=s.FirstName,LastName=s.LastName,Contact=s.Contact,Email=s.Email,DateOfBirth=s.DateOfBirth,Gender=one.Gender,Designation=s.DValue,Salary=s.Salary from(Select Id = Person.Id, FirstName = Person.FirstName, LastName = Person.LastName, Contact = Person.Contact, Email = Person.Email, DateOfBirth = Person.DateOfBirth, Gender = Person.Gender, Designation = Advisor.Designation, DValue = Lookup.Value, Salary = Advisor.Salary from Person INNER Join(Advisor INNER Join Lookup ON Lookup.Id = Advisor.Designation) ON Advisor.Id = Person.Id) as s INNer join (Select Id = Person.Id, FirstName = Person.FirstName, LastName = Person.LastName, Contact = Person.Contact, Email = Person.Email, DateOfBirth = Person.DateOfBirth, Gender = Lookup.Value, Designation = Advisor.Designation, DValue = Advisor.Designation, Salary = Advisor.Salary from Advisor INNER Join(Person INNER Join Lookup ON Lookup.Id = Person.Gender) ON Person.Id = Advisor.Id) as one On one.Id = s.id");











            SqlCommand cmd = new SqlCommand(query, con);


            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                ArrayList row = new ArrayList();

                row.Add(reader["FirstName"].ToString());
                row.Add(reader["LastName"].ToString());
                row.Add(reader["Contact"].ToString());
                row.Add(reader["Email"].ToString());
                row.Add(reader["Gender"].ToString());
                row.Add(reader["DateOfBirth"].ToString());
                row.Add(reader["Designation"].ToString());
                row.Add(reader["Salary"].ToString());

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

        private void addbutton_Click(object sender, EventArgs e)
        {
            AddAdvisor advisor = new AddAdvisor();
            advisor.Show();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            AddAdvisor change = new AddAdvisor();


            if (e.ColumnIndex == 8)
            {
                DateTime? date;
                int? pay;


                if (String.IsNullOrEmpty(dataGridView1.Rows[e.RowIndex].Cells[5].Value as String)) // checking if it is null
                {
                    
                    date = null;

                }
                else //if not then continue as you would
                {
                    date = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                }

                //will check the same for these
                string gender = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                string desig = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();


                if (String.IsNullOrEmpty(dataGridView1.Rows[e.RowIndex].Cells[7].Value as String))
                {
                    MessageBox.Show("no salary");

                    pay = null;
                }

                else //if salary is not empty
                {
                    pay = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[7].Value);


                }

                //calling setpara
                change.setpara("Update",
                    (int)e.RowIndex,
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(),
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(),
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(),
                    date,
                    gender,
                    desig,
                    pay);
                change.Show();
            }

            if (e.ColumnIndex == 9)
            {
                var Result = MessageBox.Show("delete this item ??",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);

                if (Result == DialogResult.Yes)
                {
                    SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
                    con.Open();



                    String cmD1 = string.Format("Delete FROM ProjectAdvisor WHERE AdvisorId = ANY(Select Id FROM Person WHERE Email='{0}');Delete From Advisor WHERE Id = ANY(Select Id FROM Person WHERE Email = '{0}');Delete From Person WHERE Id = ANY(Select Id FROM Person WHERE Email = '{0}')", dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                    SqlCommand comm1 = new SqlCommand(cmD1, con);
                    comm1.ExecuteNonQuery();

                    //String query = string.Format("Delete From Person where Id=ANY(Select Id from Person where Email='{0}')", dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                    //SqlCommand comm = new SqlCommand(query, con);
                    //var row = comm.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Deleted");
                    object sende = null;
                    EventArgs er = null;

                    dataGridView1.Rows.Clear();
                    this.Advisors_Load(sende, er);


                }

                else
                {
                    MessageBox.Show("Not Deleted");
                }

            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Collections;

namespace Assets
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
            
        }

        public void Form1_Load(object sender, EventArgs e)
        {

         
            dataview.Rows.Clear();
            SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
            
            con.Open();

            string query = "Select FirstName,LastName,Contact,Email,DateOfBirth,Gender=Lookup.Value,RegistrationNo from Student INNER Join (Person Right Join Lookup ON Lookup.Id=Person.Gender ) ON Student.Id=Person.Id Union Select FirstName,LastName,Contact,Email,DateOfBirth,Gender=CAST(NULL AS VARCHAR(MAX)),RegistrationNo from Student  INNer join Person ON Person.Id=Student.Id where Gender is null";
            SqlCommand cmd = new SqlCommand(query, con);
            

            SqlDataReader reader = cmd.ExecuteReader();
            List<Person> list = new List<Person>();

            //total number of options

            dataview.ColumnCount = 7;
            dataview.Columns[0].Name = "FisrtName";
            dataview.Columns[1].Name = "LastName";
            dataview.Columns[2].Name = "Contact";
            dataview.Columns[2].Width = 80;
            dataview.Columns[3].Name = "Email";
            dataview.Columns[3].Width = 80;
            dataview.Columns[4].Name = "DateOfBirth";
            dataview.Columns[5].Name = "Gender";
            dataview.Columns[5].Width = 50;
            dataview.Columns[6].Name = "RegistraionNo";


            while (reader.Read())
            {

                ArrayList row = new ArrayList();


                row.Add(reader["FirstName"].ToString());
                row.Add(reader["LAstName"].ToString());
                row.Add(reader["Contact"].ToString());
                row.Add(reader["Email"].ToString());
                row.Add(reader["DateOfBirth"].ToString());
                row.Add(reader["Gender"]);
                row.Add(reader["RegistrationNo"]);




                dataview.Rows.Add(row.ToArray());


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

            
            dataview.Columns.Add(button);
            dataview.Columns.Add(button1);
             




            con.Close();
        }



        private void dataview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdatePerson form5 = new UpdatePerson();

            //when edit button clicked
            if (e.ColumnIndex == 7)
            {
                //MessageBox.Show(e.RowIndex.ToString());
                DateTime? date; //nullable 

                if (String.IsNullOrEmpty(dataview.Rows[e.RowIndex].Cells[4].Value as String)) //check if date is a null
                {
                    MessageBox.Show("Empty");
                    date = null;

                }


                else
                {
                    date = Convert.ToDateTime(dataview.Rows[e.RowIndex].Cells[4].Value.ToString());
                }

                    form5.change((int)e.RowIndex + 1, dataview.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    dataview.Rows[e.RowIndex].Cells[1].Value.ToString(),
                    dataview.Rows[e.RowIndex].Cells[2].Value.ToString(),
                    dataview.Rows[e.RowIndex].Cells[3].Value.ToString(),

                    date,
                    dataview.Rows[e.RowIndex].Cells[5].Value.ToString(), dataview.Rows[e.RowIndex].Cells[6].Value.ToString());
                
                form5.Show();
            }

            if (e.ColumnIndex == 8)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this item ??",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
                    con.Open();

                    String cmde = string.Format("Select Id from Person where Email='{0}'", dataview.Rows[e.RowIndex].Cells[3].Value.ToString());
                    SqlCommand queryt = new SqlCommand(cmde, con);
                    SqlDataReader readerz = queryt.ExecuteReader();

                    int stid = 0;

                    while (readerz.Read())
                    {
                        stid = Convert.ToInt32(readerz["Id"]);
                    }

                    con.Close();

                    con.Open();

                    String cmdh1 = string.Format("Delete From GroupStudent where StudentId={0};Delete From Student where Id={0};Delete From Person where Id={0}", stid);
                    SqlCommand comdh1 = new SqlCommand(cmdh1, con);
                    comdh1.ExecuteNonQuery();

                    
                    con.Close();

                    MessageBox.Show("Deleted");
                    object sende = null;
                    EventArgs er = null;
                    dataview.Rows.Clear();
                    this.Form1_Load(sende, er);


                }
                else
                {
                    MessageBox.Show("Not Deleted");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddStudent Student = new AddStudent();
            Student.Show();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Advisors ad = new Advisors();
            ad.Show();
        }

        

        private void button4_Click_1(object sender, EventArgs e)
        {
            Show_Evaluation ev = new Show_Evaluation();
            ev.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Project pro = new Project();
            pro.Show();
        }
    }
}

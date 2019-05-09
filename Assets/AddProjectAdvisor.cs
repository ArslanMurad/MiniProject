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

namespace Assets
{
    public partial class AddProjectAdvisor : Form
    {
        public AddProjectAdvisor()
        {
            InitializeComponent();
        }

        //create a projectadvisor object so that objects wont get changed

        ProjectAdvisor obj = (ProjectAdvisor)Application.OpenForms["ProjectAdvisor"];

        SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");


        public string Type { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Role { get; set; }

        public DateTime AssignmentDate { get; set; }

        private void AddProjectAdvisor_Load(object sender, EventArgs e)
        {
            con.Open();

            string cmd22 = "Select Name = FirstName+' '+LastName from Advisor INNER Join Person on Person.Id = Advisor.Id";
            SqlCommand query22 = new SqlCommand(cmd22, con);

            SqlDataReader reader = query22.ExecuteReader();

            while (reader.Read())
            {
                //put data in the database
                comboBox2.Items.Add(reader["Name"]);

            }
            con.Close();

            con.Open();

            string cmd33 = "Select Name=Title from Project ";
            SqlCommand query33 = new SqlCommand(cmd33, con);

            SqlDataReader reader1 = query33.ExecuteReader();

            while (reader1.Read())
            {

                comboBox1.Items.Add(reader1["Name"]);

            }
            con.Close();

            con.Open();

            string cmd44 = "Select Name = Value from Lookup where Category = 'ADVISOR_ROLE' ";
            SqlCommand query44 = new SqlCommand(cmd44, con);

            SqlDataReader reader2 = query44.ExecuteReader();

            while (reader2.Read())
            {

                comboBox3.Items.Add(reader2["Name"]);

            }

            con.Close();

            //update the values in the forum to show what is being updates
            if (Type == "Update")
            {

                comboBox1.Text = Title;
                comboBox2.Text = Name;
                comboBox3.Text = Role;

            }
        }

        public void setpara(string type,
            string name,
            string title,
            string role)
        {
            Type = type;

            Name = name;
            Title = title;
            Role = role;

        }
        public void Ref()
        {
            this.Close();
            Object obje = null;
            EventArgs Event = null;

            obj.ProjectAdvisor_Load(obje, Event);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int pID;
            int aID;
            int lID;

            try
            {
                if (Type == "Update")
                {
                    if ( comboBox1.Text == ""|| comboBox2.Text == "" || comboBox3.Text == "" )
                    {

                        throw new Exception("Values  should not be empty");

                    }

                    con.Open();

                    if ( comboBox1.Text.ToString() != Title || comboBox2.Text.ToString() != Name )
                    {

                        String cmd11 = string.Format("Select ProjectId from ProjectAdvisor WHERE ProjectId = (Select Id from Project WHERE Title='{0}') and AdvisorId = (Select Advisor.Id from Advisor INNER Join Person ON Person.Id = Advisor.Id WHERE FirstName+' '+LastName='{1}')",
                            comboBox1.Text.ToString(),
                            comboBox2.Text.ToString());

                        SqlCommand query11 = new SqlCommand(cmd11, con);


                        if (query11.ExecuteScalar() != null)
                        {

                            con.Close();
                            throw new Exception("Advisor already exist");

                        }


                    }


                    string cmd55 = string.Format("Update ProjectAdvisor set ProjectId = (Select Id from Project WHERE Title = '{0}'),AdvisorId = (Select Advisor.Id from Advisor INNER join Person On Person.Id=Advisor.Id WHERE FirstName+' '+LastName = '{1}'), AdvisorRole = (Select Lookup.Id FROM Lookup WHERE Lookup.Value = '{2}') WHERE ProjectId = (Select Id FROM Project WHERE Title = '{3}') and AdvisorId = (Select Advisor.Id FROM Advisor INNER join Person On Person.Id=Advisor.Id WHERE FirstName+' '+LastName = '{4}')",
                        comboBox1.Text.ToString(),
                        comboBox2.Text.ToString(),
                        comboBox3.Text.ToString(),
                        Title,
                        Name);

                    SqlCommand query55 = new SqlCommand(cmd55, con);

                    query55.ExecuteNonQuery();
                    con.Close();


                }
                if (Type != "Update")
                {

                    if (comboBox1.Text == "" || comboBox2.Text == "" || comboBox3.Text == "")
                    {

                        throw new Exception("values left empty");

                    }

                    con.Open();

                    String cmd66 = string.Format("Select ProjectId FROM ProjectAdvisor WHERE ProjectId = (Select Id FROM Project WHERE Title = '{0}') and AdvisorId = (Select Advisor.Id from Advisor INNER Join Person ON Person.Id = Advisor.Id WHERE FirstName+' '+LastName = '{1}')",
                        comboBox1.Text.ToString(),
                        comboBox2.Text.ToString());

                    SqlCommand query66 = new SqlCommand(cmd66, con);

                    if (query66.ExecuteScalar() != null) // check if it already exists
                    {
                        con.Close();

                        throw new Exception("Advisor already exist");
                    }



                    string cmd12 = string.Format("Select Id FROM Project WHERE Title='{0}'", 
                        comboBox1.Text.ToString());

                    SqlCommand query12 = new SqlCommand(cmd12, con);
                    pID = (int)query12.ExecuteScalar();

                    string cmd13 = string.Format("Select Advisor.Id FROM Advisor INNER join Person ON Person.Id = advisor.Id WHERE FirstName+' '+LastName = '{0}'",
                        comboBox2.Text.ToString());

                    SqlCommand query13 = new SqlCommand(cmd13, con);
                    aID = (int)query13.ExecuteScalar();

                    string cmd14 = string.Format("Select Id FROM Lookup WHERE Value = '{0}'"
                    , comboBox3.Text.ToString());

                    SqlCommand query14 = new SqlCommand(cmd14, con);
                    lID = (int)query14.ExecuteScalar();

                    string query3 = string.Format("INSERT Into ProjectAdvisor values ({0},{1},{2},'{3}')"
                      , aID, pID, lID, DateTime.Now);
                    SqlCommand cmd3 = new SqlCommand(query3, con);

                    cmd3.ExecuteNonQuery();
                    con.Close();
                }
                

                this.Ref();
            }



            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }






        }
    }
}

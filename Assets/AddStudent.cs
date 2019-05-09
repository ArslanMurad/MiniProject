using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assets
{
    public partial class AddStudent : Form
    {

        Form1 obje = (Form1)Application.OpenForms["Form1"];

        public int ID { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        //public DateTime DateOfBirth { get; set; }
        //public int Gender { get; set; }

        public Nullable<DateTime> DateOfBirth { get; set; }
        public Nullable<int> Gender { get; set; }

        public string RegistrationNo { get; set; }

        public void Ref()
        {
            this.Close();
            Object obj = null;
            EventArgs env = null;
            obje.Form1_Load(obj, env);
        }

        public bool valid(Regex str, string txt)
        {
            return str.IsMatch(txt);
        }


        public AddStudent()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            FirstName = firstname.Text;
            LastName = lastname.Text;
            Contact = contact.Text;
            Email = email.Text;
            DateOfBirth = Convert.ToDateTime(dateofbirth.Text);
            string gen = GenderList.Text;
            RegistrationNo = registraionnumber.Text;


            SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
            try
            {


                //the regular expressions for validating stuff

                Regex first = new Regex(@"^[a-zA-Z\s]{1,20}$");
                Regex last = new Regex(@"^[a-zA-Z\s]{0,20}$");
                Regex contac = new Regex(@"^[0-9]{0,20}$");
                Regex mail = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                Regex Reg = new Regex(@"^[0-9]{4}[a-zA-Z]{2,5}[0-9]{1,3}$");



                if (!valid(first, FirstName))
                {
                    throw new Exception("FirstName should only have alphabets");
                }


                if (!valid(last, LastName))
                {
                    throw new Exception("LastName should only have alphabets");
                }


                if (!valid(contac, Contact))
                {
                    throw new Exception("Conatct number should have numbers and wshould be 11 ");
                }


                if (!valid(mail, Email))
                {
                    throw new Exception("Email should be of the form abcd@something.com");
                }

                if (!valid(Reg, RegistrationNo))
                {
                    throw new Exception("RegistrationNo should be like 2016ce5");
                }

                
                con.Open();

                //chesking if the email entered is already there
                //we must make sure that email is not repeated
                String cmdg = string.Format("Select Id from Person where Email = '{0}'", email.Text.ToString());
                //find out if the database already has this email or not
                SqlCommand query78 = new SqlCommand(cmdg, con);

                if (query78.ExecuteScalar() != null) // email is already there
                {
                    throw new Exception("Email already exist");
                }

                String cmd77 = string.Format("Select Id from Student where RegistrationNo = '{0}'", registraionnumber.Text.ToString());
                SqlCommand query77 = new SqlCommand(cmd77, con);

                if (query77.ExecuteScalar() != null)
                {
                    throw new Exception("RegistrationNo already exist");
                }

                con.Close();
                
                con.Open();

                if (!String.IsNullOrEmpty(gen as String)) // some gender was added
                {
                    String cmhk = string.Format("Select Id from Lookup where Category = 'Gender' and Value = '{0}'", gen.ToString());
                    SqlCommand queryhk = new SqlCommand(cmhk, con);


                    SqlDataReader reader = queryhk.ExecuteReader();

                    while (reader.Read())//in that case read the gender value form the database lookup 
                    {
                        Gender = Convert.ToInt32(reader["Id"]);
                    }
                }
                else
                {
                
                    Gender = null; //else in case no gender was added we must consider it to be null which is weird why wouldnt anyone tell us their gender are they emmbarrassed?
                }

                con.Close();


                con.Open();


                string cmdt = string.Format("Insert into Person values(@FirstName, @lastName, @Contact, @Email, @DateOfBirth,@Gender)");


                SqlCommand qr = new SqlCommand(cmdt, con);

                qr.Parameters.AddWithValue("@FirstName", FirstName);
                qr.Parameters.AddWithValue("@LastName", LastName);
                qr.Parameters.AddWithValue("@Contact", Contact);
                qr.Parameters.AddWithValue("@Email", Email);
                qr.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);


                if (Gender == null) // again this is very weird if someone didnt enter its gender
                {
                    qr.Parameters.AddWithValue("@Gender", DBNull.Value);
                }
                else  //for all of us normal folks
                {
                    qr.Parameters.AddWithValue("@Gender", Gender);

                }

                
                int row = qr.ExecuteNonQuery(); // enter to database with the value for Gender which can be null

                if (row > 0)
                {
                    String cmdj = string.Format("Select Id from Person where Email='{0}'", Email);
                    SqlCommand qk = new SqlCommand(cmdj, con);
                    SqlDataReader reader2 = qk.ExecuteReader();


                    int stid = 0;
                    while (reader2.Read())
                    {
                        stid = Convert.ToInt32(reader2["Id"]);
                    }
                    con.Close();
                    con.Open();

                    string cmd3 = string.Format("Insert into Student values(@Id,@RegistrationNo)");
                    SqlCommand q5 = new SqlCommand(cmd3, con);

                    q5.Parameters.AddWithValue("@Id", stid);
                    q5.Parameters.AddWithValue("@RegistrationNo", RegistrationNo);
                    q5.ExecuteNonQuery();
                    this.Ref();
                    con.Close();

                }

                             }
            catch (Exception exc)
            {
                string line1 = exc.ToString().Split(new[] { '\r', '\n' }).FirstOrDefault();
                 
                MessageBox.Show(line1);
            }
        }

        private void AddStudent_Load(object sender, EventArgs e)
        {

        }

        private void GenderList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void genderr_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

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
using System.Configuration;

namespace Assets
{
    public partial class AddAdvisor : Form
    {
        public AddAdvisor()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");

        Advisors obje = (Advisors)Application.OpenForms["Advisors"];

        public int Id { get; set; }

        public string Type { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        //public DateTime DateOfBirth { get; set; }


        public Nullable<DateTime> DateOfBirth { get; set; }


        public string Gender { get; set; }

        public string Designation { get; set; }

        //public int Salary { get; set; }

        public Nullable<int> Salary { get; set; }

        public void Ref()
        {
            this.Close();

            Object obj = null;
            EventArgs env = null;

            obje.Advisors_Load(obj, env);
        }


        public bool valid(Regex str, string txt)
        {
            return str.IsMatch(txt);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //FirstName = firstnbox.Text;
            //LastName = lastnbox.Text;
            //Contact = contactbox.Text;
            string emaile = Email;

            //Email = emailbox.Text;

            //DateOfBirth = Convert.ToDateTime(dateofbirth.Text);

            //string gen = GenderList.Text;

            //Salary = Convert.ToInt32(salbox.Text);

            try
            {
                //adding regular expressions for the validation checks

                Regex first = new Regex(@"^[a-zA-Z\s]{1,20}$");

                Regex last = new Regex(@"^[a-zA-Z\s]{1,20}$");

                Regex contact = new Regex(@"^\d{4}-\d{7}$");

                Regex pay = new Regex(@"^[0-9]{0,8}$");

                Regex mail = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");

                if (!valid(first, firstnbox.Text.ToString()))
                {
                    throw new Exception("The First should only have alphabets");
                }
                if (!valid(last, lastnbox.Text.ToString()))
                {
                    throw new Exception("LastName should only contain letters");
                }
                if (!valid(contact, contactbox.Text.ToString()))
                {
                    throw new Exception("Conatct only contain numbers");
                }
                if (!valid(mail, emailbox.Text.ToString()))
                {
                    throw new Exception("Email format is abcd@something.com");
                }

                if (designation.Text.ToString() == "")
                {
                    throw new Exception("Select designation");
                }

                if (!valid(pay, salbox.Text))
                {
                    throw new Exception("salary must not be more than 99999999");
                }
                SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");

                if (Type != "Update")
                {

                    //adding extra checks to keep data form being corrupted
                    //incase there was a nullable data entered for slaary




                    con.Open();

                    String cmd6 = string.Format("Select Advisor.Id from Advisor INNER Join Person ON Advisor.Id = Person.Id where Email ='{0}'", emailbox.Text.ToString());
                    SqlCommand querya = new SqlCommand(cmd6, con);

                    if (querya.ExecuteScalar() != null)
                    {
                        throw new Exception("already registered");
                    }

                    int income = 0;

                    if (!String.IsNullOrEmpty(salbox.Text as String))
                    {
                        income = Convert.ToInt32(salbox.Text);
                    }

                    string empty = GenderList.Text;

                    bool check = String.IsNullOrEmpty(empty as String);






                    //con.Open();

                    if (check != true)
                    {
                        string cmd2 = string.Format("Insert into Person values(@FirstName, @lastName, @Contact, @Email, @DateOfBirth,( Select Id from Lookup where Category ='Gender' and Value ='{0}'))", GenderList.Text.ToString());

                        SqlCommand queryb = new SqlCommand(cmd2, con);
                        queryb.Parameters.AddWithValue("@FirstName", firstnbox.Text);
                        queryb.Parameters.AddWithValue("@LastName", lastnbox.Text);
                        queryb.Parameters.AddWithValue("@Contact", contactbox.Text);
                        queryb.Parameters.AddWithValue("@Email", emailbox.Text);
                        queryb.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(dateofbirth.Text));

                        int row = queryb.ExecuteNonQuery();
                        con.Close();


                        if (row > 0)
                        {


                            con.Open();

                            string cmd3 = string.Format("Insert into Advisor values((Select Id from Person where Email = '{0}'),(select Id from Lookup where Value=@Designation),@Salary)", emailbox.Text);
                            SqlCommand queryc = new SqlCommand(cmd3, con);

                            queryc.Parameters.AddWithValue("@Designation", designation.Text);
                            queryc.Parameters.AddWithValue("@Salary", Convert.ToInt32(salbox.Text));
                            queryc.ExecuteNonQuery();

                            this.Ref();
                            con.Close();

                        }
                    }
                    else
                    {
                        string cm2 = string.Format("Insert into Person values(@FirstName,@lastName,@Contact,@Email,@DateOfBirth,@Gender)");
                        SqlCommand comm2 = new SqlCommand(cm2, con);
                        comm2.Parameters.AddWithValue("@FirstName", firstnbox.Text);
                        comm2.Parameters.AddWithValue("@LastName", lastnbox.Text);
                        comm2.Parameters.AddWithValue("@Contact", contactbox.Text);
                        comm2.Parameters.AddWithValue("@Email", emailbox.Text);
                        comm2.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(dateofbirth.Text));
                        comm2.Parameters.AddWithValue("@Gender", DBNull.Value);
                        int row = comm2.ExecuteNonQuery();
                        con.Close();
                        if (row > 0)
                        {

                            con.Open();

                            string cmda = string.Format("Insert into Advisor values((Select Id from Person where Email ='{0}'),(Select Id from Lookup where Category ='DESIGNATION' and Value ='{1}'),@Salary)", emailbox.Text, designation.Text.ToString());

                            SqlCommand queryq = new SqlCommand(cmda, con);

                            queryq.Parameters.AddWithValue("@Salary", income);
                            queryq.ExecuteNonQuery();

                            this.Ref();
                            con.Close();

                        }
                    }
                }



                else
                {


                    con.Open();
                    

                    String cmdd = string.Format("Select Id from Person where Email = '{0}' and Id = ANY(Select Id from Person where Id != (Select Id from Person where Email='{1}'))", emailbox.Text.ToString(), emaile);
                    SqlCommand queryy = new SqlCommand(cmdd, con);
                    
                    if (queryy.ExecuteScalar() != null)
                    {
                        throw new Exception("Email already exist");
                    }

                    int income = 0;

                    if (!String.IsNullOrEmpty(salbox.Text as String))
                    {
                        income = Convert.ToInt32(salbox.Text);
                    }

                    if (!String.IsNullOrEmpty(GenderList.Text as String))
                    {
                        String cmdf = string.Format("Update Person set FirstName ='{0}', LastName ='{1}',Contact ='{2}',Email='{3}',DateOfBirth='{4}',Gender=(Select Id from Lookup where Value='{5}') where Id=(Select Id from Person where Email='{6}')", firstnbox.Text.ToString(), lastnbox.Text.ToString(), contactbox.Text.ToString(), emailbox.Text.ToString(), Convert.ToDateTime(dateofbirth.Text.ToString()), GenderList.Text.ToString(), Email);

                        SqlCommand queryd = new SqlCommand(cmdf, con);
                        var row = queryd.ExecuteNonQuery();



                        String cmdn = string.Format("Select Id from Lookup where Value ='{0}'", designation.Text.ToString());
                        SqlCommand queryk = new SqlCommand(cmdn, con);

                        int ide = (int)queryk.ExecuteScalar(); // typecast
                        string cmdj = string.Format("Update Advisor set Salary={0}, Designation={1} where Id=(Select Id from Person where Email='{2}')", income, ide, emailbox.Text.ToString());


                        SqlCommand queryi = new SqlCommand(cmdj, con);


                        queryi.ExecuteNonQuery();
                    }
                    else
                    {
                        String cmh = string.Format("Update Person set FirstName ='{0}', LastName ='{1}',Contact ='{2}',Email ='{3}',DateOfBirth ='{4}',Gender = @Gender where Id = (Select Id from Person where Email='{5}')", firstnbox.Text.ToString(), lastnbox.Text.ToString(), contactbox.Text.ToString(), emailbox.Text.ToString(), Convert.ToDateTime(dateofbirth.Text.ToString()), Email);
                        SqlCommand queryh = new SqlCommand(cmh, con);
                        queryh.Parameters.AddWithValue("@Gender", DBNull.Value);


                        var row = queryh.ExecuteNonQuery();
                        String cmdo = string.Format("Select Id from Lookup where Value='{0}'", designation.Text.ToString());
                        SqlCommand queryt = new SqlCommand(cmdo, con);


                        int ide = (int)queryt.ExecuteScalar();


                        string cmzd = string.Format("Update Advisor set Salary = {0}, Designation = {1} where Id=(Select Id from Person where Email='{2}')", income, ide, emailbox.Text.ToString());
                        SqlCommand queryzd = new SqlCommand(cmzd, con);

                        queryzd.ExecuteNonQuery();
                    }

                    this.Ref();
                    con.Close();

                }



            }

            catch (Exception exec)
            {
                string line = exec.ToString().Split(new[] { '\r', '\n' }).FirstOrDefault();
                
                MessageBox.Show(line);
            }




        }



        public void setpara
            (
           string type,
           int id,
           string first,
           string last,
           string conatct,
           string mail,
           DateTime? dateofbirth,
           string gender,
           string designation,
           int? sal
            )
        {
            Type = type;
            Id = id;
            FirstName = first;
            LastName = last;
            Contact = conatct;
            Email = mail;
            Designation = designation;
            Salary = sal;
            Gender = gender;
            DateOfBirth = dateofbirth;

        }

        private void AddAdvisor_Load(object sender, EventArgs e)
        {

            if (Type == "Update")
            {
                
                firstnbox.Text = FirstName;
                lastnbox.Text = LastName;
                contactbox.Text = Contact;
                emailbox.Text = Email;
                dateofbirth.Text = Convert.ToString(DateOfBirth);
                GenderList.Text = Gender;
                designation.Text = Designation;
                salbox.Text = Salary.ToString();

            }

        }

        
    }
}

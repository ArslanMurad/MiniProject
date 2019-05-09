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
    public partial class UpdatePerson : Form
    {
        Form1 obje = (Form1)Application.OpenForms["Form1"];

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string RegistrationNo { get; set; }

        public UpdatePerson()
        {
            InitializeComponent();
        }

        public void Ref()
        {
            
            this.Close();
            Object obj = null;
            EventArgs env = null;
            obje.Form1_Load(obj, env);
        }

        public bool valid (Regex str, string txt)
        {
            return str.IsMatch(txt);
        }

        public void change(int id,
            string First,
            string Last,
            string Cont,
            string mail,
            DateTime? date,
            string gend,
            string RegNO)
        {

            ID = id;

            FirstName = First;
            firstname.Text = First;

            LastName = Last;
            lastname.Text = Last;

            Contact = Cont;
            contact.Text = Cont;

            Email = mail;
            email.Text = mail;

            DateOfBirth = date;
            dateofbirth.Text = Convert.ToString(date);

            Gender = gend;
            genderr.Text = Convert.ToString(gend);



            RegistrationNo = RegNO;
            registraionnumber.Text = RegNO;




        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                //setting in regular expressions for the purpose  of validations
                Regex first = new Regex(@"^[a-zA-Z\s]{1,20}$");
                Regex last = new Regex(@"^[a-zA-Z\s]{0,20}$");
                Regex contac = new Regex(@"^[0-9]{0,20}$");
                Regex mail = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                Regex Reg = new Regex(@"^[0-9]{4}[a-zA-Z]{2,5}[0-9]{1,3}$");

                if (!valid(first, firstname.Text.ToString())) //so that  wrong data doesnt get saved mistakenly
                {
                    throw new Exception("FirstName only contain letters");
                }

                if (!valid (last, lastname.Text.ToString()))
                {
                    throw new Exception("LastName only contain letters");
                }

                if (!valid(contac, contact.Text.ToString()))
                {
                    throw new Exception("Conatct only contain numbers [0-20]");
                }

                if (!valid(mail, email.Text.ToString()))
                {
                    throw new Exception("Email should be like ali123@gmail.com");
                }

                if (!valid(Reg, registraionnumber.Text.ToString()))
                {
                    throw new Exception("RegistrationNo should be like 2016ce5");
                }

                SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");


                con.Open();

                String cmd = string.Format("Select Id from Person where Email='{0}' and Id=ANY(Select Id from Person where Id != (Select Id from Person where Email='{1}'))", email.Text.ToString(), Email);
                SqlCommand querya = new SqlCommand(cmd, con);
                 
                if (querya.ExecuteScalar() != null)
                {
                    throw new Exception("Email already exist");
                }

                String cmd1 = string.Format("Select Id from Student where RegistrationNo='{0}' and Id=ANY(Select Id from Student where Id != (Select Id from Student where RegistrationNo='{1}'))", registraionnumber.Text.ToString(), RegistrationNo);
                SqlCommand queryb = new SqlCommand(cmd1, con);

                if (queryb.ExecuteScalar() != null)
                {
                    throw new Exception("RegistrationNo already exist");
                }

                int Gend;

                if (!String.IsNullOrEmpty(genderr.Text as String)) //if no gender was entered
                {
                    String cmd2 = string.Format("Select Id from Lookup where Category = 'Gender' and Value = '{0}'", genderr.Text);
                    SqlCommand comm2 = new SqlCommand(cmd2, con);

                    Gend = (int)comm2.ExecuteScalar();

                    String cmda = string.Format("Update Person set FirstName='{0}', LastName='{1}',Contact='{2}',Email='{3}',DateOfBirth='{4}',Gender={5} where Id=(Select Id from Person where Email='{6}')", firstname.Text.ToString(), lastname.Text.ToString(), contact.Text.ToString(), email.Text.ToString(), Convert.ToDateTime(dateofbirth.Text.ToString()), Gend, Email);
                    SqlCommand queryc = new SqlCommand(cmda, con);

                    var row = queryc.ExecuteNonQuery();
                    string cm1 = string.Format("Update Student set RegistrationNo='{0}' where Id=(Select Id from Person where Email='{1}')", registraionnumber.Text.ToString(), email.Text.ToString());
                    SqlCommand comm1 = new SqlCommand(cm1, con);
                    comm1.ExecuteNonQuery();

                    
                }

                else
                {
                    String cm = string.Format("Update Person set FirstName='{0}', LastName='{1}',Contact='{2}',Email='{3}',DateOfBirth='{4}',Gender=@Gender where Id=(Select Id from Person where Email='{5}')", firstname.Text.ToString(), lastname.Text.ToString(), contact.Text.ToString(), email.Text.ToString(), Convert.ToDateTime(dateofbirth.Text.ToString()), Email);
                    SqlCommand comm = new SqlCommand(cm, con);
                    comm.Parameters.AddWithValue("@Gender", DBNull.Value);

                    var row = comm.ExecuteNonQuery();

                    string cmdh = string.Format("Update Student set RegistrationNo='{0}' where Id=(Select Id from Person where Email='{1}')", registraionnumber.Text.ToString(), email.Text.ToString());
                    SqlCommand queryt = new SqlCommand(cmdh, con);
                    queryt.ExecuteNonQuery();

                }
                 
                con.Close();
                this.Ref();

            }
            catch (Exception exc)
            {
                string line1 = exc.ToString().Split(new[] { '\r', '\n' }).FirstOrDefault();
                //button1.DialogResult = DialogResult.Cancel;
                MessageBox.Show(exc.ToString());
            }

        }
    }
}

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
    public partial class AddProject : Form
    {
        public AddProject()
        {
            InitializeComponent();
        }

        Project obje = (Project)Application.OpenForms["Project"];





        private void label1_Click(object sender, EventArgs e)
        {

        }

        public string type { get; set; }
        public int Id { get; set; }

        public string title { get; set; }
        public string description { get; set; }

        

        public void Ref()
        {
            this.Close();

            Object obj = null;
            EventArgs event1 = null;
            obje.Project_Load(obj, event1);
        }

        public bool valid(Regex str, string txt)
        {
            return str.IsMatch(txt);
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                Regex titlever = new Regex(@"^[a-zA-Z\s]*$");


                if (!valid(titlever, titlebox.Text.ToString()))
                {

                    throw new Exception("Title can only contain letters");

                }
                if (type != "Update")
                {
                    SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
                    con.Open();

                    String cmq = string.Format("Select Id from Project where Title = '{0}'", titlebox.Text.ToString());
                    SqlCommand qm = new SqlCommand(cmq, con);

                    if (qm.ExecuteScalar() != null)
                    {
                        throw new Exception("Title already exist");
                    }

                    string cmd3 = string.Format("Insert into Project values(@Description,@Title)");
                    SqlCommand query3 = new SqlCommand(cmd3, con);
                    //inserts values into the table
                    query3.Parameters.AddWithValue("@Title", titlebox.Text);
                    query3.Parameters.AddWithValue("@Description", desbox.Text);

                    int row = query3.ExecuteNonQuery();
                    this.Ref();
                    con.Close();
                }
                else
                {
                    SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
                    con.Open();

                    String cmgh = string.Format("Select Id from Project where Title = '{0}' and Id = ANY(Select Id from Project where Id != (Select Id from Project where Title='{1}'))", titlebox.Text.ToString(), title);
                    SqlCommand queryt = new SqlCommand(cmgh, con);
                     
                    if (queryt.ExecuteScalar() != null)
                    {
                        throw new Exception("Title already exist");
                    }


                    string cmd4 = string.Format("Update Project set Title='{0}', Description='{1}' where Id={2}", titlebox.Text, desbox.Text, Id);
                    SqlCommand query4 = new SqlCommand(cmd4, con);


                    int row = query4.ExecuteNonQuery();
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

        private void AddProject_Load(object sender, EventArgs e)
        {
            if (type == "Update")
            {
                label1.Text = "Update Project"; // automatically chnages the text

                titlebox.Text = title;

                desbox.Text = description;
            }
        }

        public void setpara(string type1, int id1, string title1, string descr)
        {
            type = type1;
            Id = id1;
            title = title1;
            description = descr;

        }

    }
    }



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
    public partial class AddEvaluation : Form
    {
        Show_Evaluation obj = (Show_Evaluation)Application.OpenForms["Show_Evaluation"];
        private string Id { get; set; }

        private string Type { get; set; }

        private string Title { get; set; }

        private int TotalMarks { get; set; }

        private float TotalWeightage { get; set; }


        public AddEvaluation()
        {
            InitializeComponent();
        }


        public void Ref()
        {
            this.Close();
            Object obje = null;
            EventArgs event1 = null;
            obj.Show_Evaluation_Load(obje , event1);


        }

        public bool valid(Regex str , string txt)
        {
            return str.IsMatch(txt);
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                Regex titleregex = new Regex(@"^[a-zA-Z]$");
                Regex marksregex = new Regex(@"^[0,9]{0,3}$");
                Regex weightregex = new Regex(@"^[0-9]{0,3}$");

                if(!valid(titleregex, name.Text.ToString()))
                {
                    throw new Exception("Name should only have letters");
                }

                if (!valid(marksregex, textmarks.Text.ToString()) || (float)Convert.ToDouble(textmarks.Text) > 100)
                {
                    throw new Exception("marks should be digits within the range of [0,100]");
                }

                if (!valid(titleregex, totalweight.Text.ToString()) || (float)Convert.ToDouble(totalweight.Text) > 100)
                {
                    throw new Exception("weightage should be digits within the range of [0,100]");
                }

                if(Type != "Update")
                {
                    SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
                    con.Open();


                    //cheakcing whether name already exists

                    String cmdk = string.Format("Select Id from Evaluation where Name='{0}'", name.Text.ToString());
                    SqlCommand queryk = new SqlCommand(cmdk, con);

                    if (queryk.ExecuteScalar() != null)
                    {
                        throw new Exception("Name already exist");
                    }


                    string query = string.Format("insert into Evaluation values(@Name, @TotalMarks, @TotalWeight)");
                    SqlCommand cmd = new SqlCommand(query, con);

                    
                    
                    cmd.Parameters.AddWithValue("@Name", name.Text);
                    cmd.Parameters.AddWithValue("@TotalMarks", (float)Convert.ToDouble(textmarks.Text));
                    cmd.Parameters.AddWithValue("@TotalWeightage", (float)Convert.ToDouble(totalweight.Text));

                    int row = cmd.ExecuteNonQuery();

                    this.Ref();
                    con.Close();


                }
                else
                {
                    SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
                    con.Open();

                    //again checking whether name already exists

                    String cmdd = string.Format("Select Id from Evaluation where Name='{0}' and Id=ANY(Select Id from Evaluation where Id != (Select Id from Evaluation where Name='{1}'))", name.Text.ToString(), Title);
                    SqlCommand queryd = new SqlCommand(cmdd, con);
                    


                    if (queryd.ExecuteScalar() != null)
                    {
                        throw new Exception("Name already exist");
                    }



                    string cmd2 = string.Format("Update Evaluation set Name='{0}', TotalMarks={1},TotalWeightage={2} where Name='{3}' and TotalMarks={4} and TotalWeightage={5} ",
                        name.Text, (float)Convert.ToDouble(textmarks.Text),
                        (float)Convert.ToDouble(totalweight.Text), Title,
                        TotalMarks, TotalWeightage);

                    SqlCommand query2 = new SqlCommand(cmd2, con);
                    
                    int row = query2.ExecuteNonQuery();


                    this.Ref();
                    con.Close();
                }

            }
            catch(Exception exec)
            {
                string line = exec.ToString().Split(new[] { '\r', '\n' }).FirstOrDefault();
                
                MessageBox.Show(line);
            }
        }

        public void setpara(string type,  string title, int totalmarks , float weightage )
        {
            Type = type;
            
            Title = title;
            TotalMarks = totalmarks;
            TotalWeightage = weightage;
        }

        private void AddEvaluation_Load(object sender, EventArgs e)
        {
            if (Type == "Update")
            {
                name.Text = Name;
                textmarks.Text = Convert.ToString(TotalMarks);
                totalweight.Text = Convert.ToString(TotalWeightage);
            }
            
        }
    }
}

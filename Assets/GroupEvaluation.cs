using System;
using System.Collections;
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
    public partial class GroupEvaluation : Form
    {
        public GroupEvaluation()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");


        public int Id { get; set; }

        private void GroupEvaluation_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear(); // clear all the rows for this situation
            comboBox1.Items.Clear(); //enter they type of evaluation
            con.Open();

            string cmd34 = string.Format("Select Name,TotalMarks,ObtainedMarks,EvaluationDate FROM Evaluation INNER JOIN GroupEvaluation ON Evaluation.Id = GroupEvaluation.EvaluationId WHERE GroupId = {0}", Id);
            
            SqlCommand query34 = new SqlCommand(cmd34, con);
            SqlDataReader reader = query34.ExecuteReader();


            List<string> list = new List<string>();

            while (reader.Read())
            {
                
                ArrayList row = new ArrayList();


                row.Add(reader["Name"].ToString());
                row.Add(reader["TotalMarks"].ToString());

                row.Add(reader["ObtainedMarks"].ToString());
                list.Add(reader["Name"].ToString());
                row.Add(reader["EvaluationDate"].ToString());

                dataGridView1.Rows.Add(row.ToArray());
            }

            con.Close();


            con.Open();

            string cmd56 = string.Format("Select Name FROM Evaluation ");
            SqlCommand query56 = new SqlCommand(cmd56, con);
            SqlDataReader reader1 = query56.ExecuteReader();


            while (reader1.Read())
            {

                if (!list.Contains(reader1["Name"].ToString()))
                {

                    
                    comboBox1.Items.Add(reader1["Name"].ToString());
                }

            }
            con.Close();
        }



        public bool valid(Regex str, string txt)
        {

            return str.IsMatch(txt);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Regex mark = new Regex(@"^[0-9]{1,3}$"); //condition of the type of marks to be entered


                if (comboBox1.Text.ToString() == "")
                {
                      throw new Exception("Select Evalaution from list");
                }

                con.Open();
                string cmd67 = string.Format("Select TotalMarks FROM Evaluation WHERE Name = '{0}'"
                    , comboBox1.Text.ToString());
                
                SqlCommand query67 = new SqlCommand(cmd67, con);

                int marks = (int)query67.ExecuteScalar();
                
                con.Close();

                if (!(valid(mark, textBox1.Text.ToString())))
                {
                    throw new Exception("Marks should be in numbers and 3 digits each between 0-9 and less than total marks");
                }

                else if (int.Parse(textBox1.Text.ToString()) > marks)
                {
                    throw new Exception("the entered marks should be less than total marks");
                }

                con.Open();

                string cmd46 = string.Format("INSERT Into GroupEvaluation values ({0}, (Select Id from Evaluation where Name = '{1}'), {2},'{3}')", Id, comboBox1.Text.ToString(), int.Parse(textBox1.Text.ToString()), DateTime.Now);
                SqlCommand query46 = new SqlCommand(cmd46, con);

                query46.ExecuteNonQuery();

                con.Close();

                object ob = null;
                EventArgs er = null;
                this.GroupEvaluation_Load(ob, er); //make new object
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //mechanism for deletion
            if (e.ColumnIndex == 5)
            {
                var ConfirmResult = MessageBox.Show("DELETE THIS ?",
                                     "Confirm Delete!",
                                     MessageBoxButtons.YesNo);

                if (ConfirmResult == DialogResult.Yes)
                {
                    con.Open();

                    string cmd99 = string.Format("DELETE FROM GroupEvaluation WHERE GroupId={0} and EvaluationId = (Select Id FROM Evaluation WHERE Name = '{1}')", Id, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    SqlCommand query99 = new SqlCommand(cmd99, con);
                    query99.ExecuteNonQuery();

                    con.Close();



                    object ob = null;
                    EventArgs er = null;
                    this.GroupEvaluation_Load(ob, er);
                    //reload the Group Evaluation forum

                }
            }
            





            if (e.ColumnIndex == 4)
            {

                MessageBox.Show("update this ? -by deleting this and adding new entry");
            }
        }
    }
}

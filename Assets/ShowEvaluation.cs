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
    public partial class Show_Evaluation : Form
    {
        public Show_Evaluation()
        {
            InitializeComponent();
        }

        public void Show_Evaluation_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'assetsDataSet.IT_Assets' table. You can move, or remove it, as needed.
            //this.iT_AssetsTableAdapter.Fill(this.assetsDataSet.IT_Assets);
            dataGridView1.Rows.Clear();
            SqlConnection conn = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
            conn.Open();
            string query = "Select Name,TotalMarks, TotalWeightage from Evaluation";
            SqlCommand cmd = new SqlCommand(query, conn);
            

            SqlDataReader reader = cmd.ExecuteReader();
            List<Person> list = new List<Person>();

            int i = 0;
            while (reader.Read())
            {
                ArrayList row = new ArrayList();
                row.Add(reader["Name"].ToString());
                row.Add(reader["TotalMarks"].ToString());
                row.Add(reader["TotalWeightage"].ToString());
                dataGridView1.Rows.Add(row.ToArray());
                ++i;

            }
            



            conn.Close();
        }

        private void Addbutton_Click(object sender, EventArgs e)
        {
            AddEvaluation Add = new AddEvaluation(); // no dialog box and no stuff  would be returned

            Add.Show();
            //if the form gets bad input it would still run which is bad for us and we dont want that to happen
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            AddEvaluation ev = new AddEvaluation();

            if(e.ColumnIndex == 3)
            {
                ev.setpara("Update", dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    (int)dataGridView1.Rows[e.RowIndex].Cells[1].Value,
                    (float)Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[0].Value));

                ev.Show();
            }

            if (e.ColumnIndex == 4)
            {
                var Result = MessageBox.Show("Are you sure to delete this item ??",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);//asking for confirmation to deletion

                if (Result == DialogResult.Yes) // approving deletion
                {
                    SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
                    con.Open();

                    String cmd = string.Format("Delete from GroupEvaluation where EvaluationId=(Select Id from Evaluation where Name='{0}');Delete from Evaluation where Name='{0}' and TotalMarks={1} and TotalWeightage={2};", dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[1].Value), dataGridView1.Rows[e.RowIndex].Cells[2].Value);
                    SqlCommand query = new SqlCommand(cmd, con);

                    query.ExecuteNonQuery();
                    
                    con.Close();
                    

                    MessageBox.Show("Deleted");

                    object sende = null;
                    EventArgs er = null;

                    dataGridView1.Rows.Clear();
                    this.Show_Evaluation_Load(sende, er);

                }
                else
                {

                    MessageBox.Show("Not Deleted");

                }
            }

































        }
    }
}

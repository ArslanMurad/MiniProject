using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assets
{
    public partial class Mainpage : Form
    {
        public Mainpage()
        {
            InitializeComponent();
        }

        private void Mainpage_Load(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            Form1 student = new Form1();
            student.Show();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Project project = new Project();
            project.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Advisors Advisor = new Advisors();
            Advisor.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Show_Evaluation Evaluation = new Show_Evaluation();
            Evaluation.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Group Groups = new Group();
            Groups.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Reports report = new Reports();
            report.Show();
        }
    }
}

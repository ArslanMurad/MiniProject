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
    public partial class UpdateProject : Form
    {
        public UpdateProject()
        {
            InitializeComponent();
        }

        public int id { get; set; }

        public string title { get; set; }

        public string description { get; set; }
        

        private void UpdateProject_Load(object sender, EventArgs e)
        {
            titlebox.Text = title;
            desbox.Text = description;
        }

        public void setpara(int id, string title, string descr)
        {
            id = id;
            title = title;
            description = descr;

        }
    }
}

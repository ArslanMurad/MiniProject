using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;//requires you to download the iTextSharp Library
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
namespace Assets

    //I HAVE COMMENTED OUT ALOT OF STUFF THAT CAUSES  ERRORS BECAUSE THE RIGHT LIBRARY IS NOT AVAILABE TO BE SENT ALONG
{
    public partial class Reports : Form
    {

        SqlConnection con = new SqlConnection("Data Source = HAIER-PC; Initial Catalog = ProjectA; Integrated Security = True; MultipleActiveResultSets = True");
        //setting up the connection

        


        public Reports()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //setting up the datagridview for the report
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Project";
            dataGridView1.Columns[1].Name = "Advisor";
            dataGridView1.Columns[2].Name = "AdvisorDesignation";
            dataGridView1.Columns[3].Name = "AdvisorRole";

            con.Open();

            

            Document document = new Document();
            PdfPTable table = new PdfPTable(5);
            Paragraph paragraph = new Paragraph( "Projects and their Advisors\n" + Environment.NewLine );

            table.AddCell("No#");
            table.AddCell("Project Title");
            table.AddCell("Advisor Name");
            table.AddCell("Advisor Designation");
            table.AddCell("Advisor Role");
            
            //exception handling

            try
            {
                PdfWriter.GetInstance( document, new FileStream( "Report#1.pdf", FileMode.Create ));
                //this will create a new pdf document on its own and write to this pdf document the results
                document.Open();
                //we have noe opened this newly created docuemnt

                string cmdepic = string.Format("Select Title = one.Title, Name = seo.Name, Designation = seo.Designation, Role = one.Role FROM (Select ProjectID ,Title,Name = FirstName + ' ' + LastName,Role = AdvisorRole, Designation = Lookup.Value FROM Lookup INNER JOIN ( Project INNER Join( ProjectAdvisor INNER Join( Advisor INNER JOIN Person ON Person.Id = Advisor.Id ) ON Advisor.Id = ProjectAdvisor.AdvisorId ) ON Project.Id = ProjectAdvisor.ProjectId ) ON Lookup.Id = Advisor.Designation ) AS seo INNER join (Select ProjectId, Title, Name = FirstName + ' ' + LastName, Role = Lookup.Value, Designation FROM Lookup INNER Join( Project INNER Join( ProjectAdvisor INNER JOIN ( Advisor INNER Join Person ON Person.Id = Advisor.Id ) On Advisor.Id = ProjectAdvisor.AdvisorId ) ON Project.Id = ProjectAdvisor.ProjectId) ON  Lookup.Id = ProjectAdvisor.AdvisorRole ) AS one ON seo.ProjectId = one.ProjectId");
                SqlCommand queryepic = new SqlCommand(cmdepic, con);
                SqlDataReader reader = queryepic.ExecuteReader();

                int i;
                i = 1;

                while (reader.Read())
                {
                    ArrayList row = new ArrayList();
                    
                    //adding elements to this pdf file we have created

                    table.AddCell(i.ToString());
                    //add  the number
                    row.Add(reader["Title"].ToString());
                    //add a row to the Title
                    table.AddCell(reader["Title"].ToString());
                    //write in the cell
                    row.Add(reader["Name"].ToString());
                    //add the Name cell
                    table.AddCell(reader["Name"].ToString());
                    //add data and similarly the rest
                    row.Add(reader["Designation"].ToString());
                    table.AddCell(reader["Designation"].ToString());
                    row.Add(reader["Role"].ToString());
                    table.AddCell(reader["Role"].ToString());
                    dataGridView1.Rows.Add(row.ToArray());
                    i = i + 1;

                }
                


                document.Add(paragraph);
                //adding the heading
                document.Add(table);
                //adding the table with the data
                document.Close();

                MessageBox.Show("the File has successfully been created in the project folder");
            }

            catch (Exception exc)
            {
                MessageBox.Show("cannot access file becasue it is already open");
            }

            con.Close();

           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //creating the colums for the local view of the data on the forum
            dataGridView1.Rows.Clear(); //clear the view
            dataGridView1.ColumnCount = 5; //add the colums to it
            dataGridView1.Columns[0].Name = "Project"; // add projects column
            dataGridView1.Columns[1].Name = "Evaluation"; //add the evaluation column
            dataGridView1.Columns[2].Name = "TotalMarks"; //add the total marks column
            dataGridView1.Columns[3].Name = "ObtainedMarks"; //add the marks obtained column
            dataGridView1.Columns[4].Name = "TotalWeightage"; // add the total wirghtage column

            con.Open();
            
            Document document = new Document(); // the file for creation
            PdfPTable table = new PdfPTable(6); // the table that goes into the file
            Paragraph paragraph = new Paragraph("Projects and Evaluations\n" + Environment.NewLine);



            table.AddCell("No#");
            table.AddCell("Project Title");
            table.AddCell("Evaluation Name");
            table.AddCell("TotalMarks");
            table.AddCell("ObtainedMarks");
            table.AddCell("TotalWeightage");

            try
            {
                PdfWriter.GetInstance( document, new FileStream("Report#2.pdf", FileMode.Create));
                //creata a new pdf document and create it
                document.Open();

                string cmdlong = string.Format("Select Project = Project.Title,Evaluation = Evaluation.Name, TotalMarks, ObtainedMarks, TotalWeightage FROM Project INNER JOIN (GroupProject INNER JOIN (GroupEvaluation INNER JOIN Evaluation ON GroupEvaluation.EvaluationId = Evaluation.Id) ON GroupEvaluation.GroupId = GroupProject.GroupId) ON Project.Id = GroupProject.ProjectId");
                SqlCommand querylong = new SqlCommand(cmdlong, con);
                SqlDataReader reader = querylong.ExecuteReader();

                int i; ///for indexing the report
                i = 1;

                while (reader.Read())
                {
                    ArrayList row = new ArrayList();

                    table.AddCell(i.ToString()); // add to docuemnt

                    row.Add(reader["Project"].ToString());// add to forum
                    table.AddCell(reader["Project"].ToString());//add to docuemnt 

                    row.Add(reader["Evaluation"].ToString()); // add to the forum
                    table.AddCell(reader["Evaluation"].ToString());
                    
                    //similar to above

                    row.Add(reader["TotalMarks"].ToString());
                    table.AddCell(reader["TotalMarks"].ToString());

                    row.Add(reader["ObtainedMarks"].ToString());
                    table.AddCell(reader["ObtainedMarks"].ToString());

                    row.Add(reader["TotalWeightage"].ToString());
                    table.AddCell(reader["TotalWeightage"].ToString());

                    dataGridView1.Rows.Add(row.ToArray());

                    i++;

                }



                document.Add(paragraph);
                document.Add(table);
                document.Close();

                MessageBox.Show("Your File is Created Successfull in your Project folder");
            }

            catch (Exception exc)
            {
                MessageBox.Show("Can not access file because already open");
            }

            con.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "GroupID";
            dataGridView1.Columns[1].Name = "StudentName";
            dataGridView1.Columns[2].Name = "Email";
            dataGridView1.Columns[3].Name = "Status";
            dataGridView1.Columns[4].Name = "RegistrationNo";

            con.Open();
            
            
            Document document = new Document();
            PdfPTable table = new PdfPTable(6);
            Paragraph paragraph = new Paragraph("Student Groups And Their Status\n" + Environment.NewLine);

            table.AddCell("No#");
            table.AddCell("GroupID");
            table.AddCell("Student Name");
            table.AddCell("Email");
            table.AddCell("Status");
            table.AddCell("RegistrationNo");

            try
            {
                PdfWriter.GetInstance( document, new FileStream( "Report#3.pdf", FileMode.Create ) );
                document.Open();

                string cmdgood = string.Format("Select GroupId = GroupStudent.GroupId, StudentName = FirstName+' '+LastName, Email, Status = Lookup.Value, RegistrationNo FROM Person INNER JOIN (Student INNER JOIN(GroupStudent INNER JOIN Lookup ON Lookup.Id = GroupStudent.Status) ON GroupStudent.StudentId = Student.Id) ON Student.Id = Person.Id");
                SqlCommand querygood = new SqlCommand(cmdgood, con);
                SqlDataReader reader = querygood.ExecuteReader();

                int i;
                i = 1;

                while (reader.Read())
                {
                    ArrayList row = new ArrayList();
                    //adding to the forum
                    //addding to the document

                    table.AddCell(i.ToString());

                    row.Add(reader["GroupId"].ToString());
                    table.AddCell(reader["GroupId"].ToString());

                    row.Add(reader["StudentName"].ToString());
                    table.AddCell(reader["StudentName"].ToString());

                    row.Add(reader["Email"].ToString());
                    table.AddCell(reader["Email"].ToString());

                    row.Add(reader["Status"].ToString());
                    table.AddCell(reader["Status"].ToString());

                    row.Add(reader["RegistrationNo"].ToString());
                    table.AddCell(reader["RegistrationNo"].ToString());


                    dataGridView1.Rows.Add(row.ToArray());
                    i++;
                }



                document.Add(paragraph); //adding header to the project
                document.Add(table) ; //adding the actual paragraph
                document.Close();

                MessageBox.Show("Your file has been successfully created in the folder of your project");
            }

            catch (Exception exc)
            {
                MessageBox.Show("cannot acccess the file becasue it is already open");
            }

            con.Close();
        }
    }
}

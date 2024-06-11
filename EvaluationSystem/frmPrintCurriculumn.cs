using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace EvaluationSystem
{
    public partial class frmPrintCurriculumn : Form
    {
        private SQLConfig SC = new SQLConfig();
        private string sql;
        private int maxrow;
        private int courseid;
        private string idno;
        private PrintDocument printDocument1 = new PrintDocument();
        private PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();

        public frmPrintCurriculumn(int courseid, string idno)
        {
            InitializeComponent();
            this.courseid = courseid;
            this.idno = idno;

            // Initialize PrintDocument and its event handler
            printDocument1.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
        }

        private void loadReports(int courseid, string idno)
        {
            sql = "Select IdNo, Firstname, Lastname, HomeAddress, Course, s.YearLevel, StudentPhoto From tblstudent s, tblcourse c Where IdNo = '" + idno + "' AND s.CourseId = c.CourseId";
            maxrow = SC.maxrow(sql);
            if (maxrow > 0)
            {
                foreach (DataRow r in SC.dt.Rows)
                {
                    lblName.Text = r.Field<string>("Firstname") + " " + r.Field<string>("Lastname");
                    lblAddress.Text = r.Field<string>("IdNo");
                    lblCourse.Text = r.Field<string>("Course");
                    lblYearLevel.Text = r.Field<string>("YearLevel");
                    PictureBox1.ImageLocation = Application.StartupPath + "\\StudentPhoto\\" + r.Field<string>("StudentPhoto");
                }
            }

            sql = "SELECT Subject, DescriptiveTitle, LecUnit, LabUnit, s.YearLevel, Semester, Grades FROM tblsubject s, tblgrades g, tblstudent st, tblcourse c " +
                  "WHERE s.SubjectId = g.SubjectId AND g.IdNo = st.IdNo AND g.CourseId = c.CourseId " +
                  "AND c.CourseId = " + courseid + " AND g.IdNo = '" + idno + "'";
            Console.WriteLine(sql);
            try
            {
                SC.Load_DTG(sql, dataGridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error loading data.");
            }
        }

        private void frmPrintCurriculumn_Load(object sender, EventArgs e)
        {
            loadReports(courseid, idno);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();

            printDialog.Document = printDocument;
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDocument.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while trying to print: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Define fonts and brushes
            Font headerFont = new Font("Arial", 24, FontStyle.Bold);
            Font subHeaderFont = new Font("Arial", 14, FontStyle.Bold);
            Font normalFont = new Font("Arial", 10, FontStyle.Regular);
            Font columnHeaderFont = new Font("Arial", 12, FontStyle.Bold);
            Brush brush = Brushes.Black;

            // Define margins
            int leftMargin = 50;
            int rightMargin = e.PageBounds.Width - 50;
            int topMargin = 50;

            // Draw header (center aligned)
            string headerText = "Detailed Marks Certificate";
            SizeF headerSize = e.Graphics.MeasureString(headerText, headerFont);
            e.Graphics.DrawString(headerText, headerFont, brush, (e.PageBounds.Width - headerSize.Width) / 2, topMargin);

            // Draw student information
            int infoTopMargin = topMargin + 70;
            e.Graphics.DrawString("Name: " + lblName.Text, subHeaderFont, brush, leftMargin, infoTopMargin);
            e.Graphics.DrawString("Roll No: " + lblAddress.Text, subHeaderFont, brush, leftMargin, infoTopMargin + 30);
            e.Graphics.DrawString("Course: " + lblCourse.Text, subHeaderFont, brush, leftMargin, infoTopMargin + 60);
            e.Graphics.DrawString("Year Level: " + lblYearLevel.Text, subHeaderFont, brush, leftMargin, infoTopMargin + 90);

            // Draw student photo
            if (!string.IsNullOrEmpty(PictureBox1.ImageLocation))
            {
                Image studentImage = Image.FromFile(PictureBox1.ImageLocation);
                e.Graphics.DrawImage(studentImage, rightMargin - 150, infoTopMargin, 100, 100);
            }

            // Draw column headers
            int yPos = infoTopMargin + 150;
            e.Graphics.DrawString("Crs. Code", columnHeaderFont, brush, leftMargin, yPos);
            e.Graphics.DrawString("Crs. Title", columnHeaderFont, brush, leftMargin + 100, yPos);
            e.Graphics.DrawString("Credit Hrs", columnHeaderFont, brush, leftMargin + 350, yPos);
            e.Graphics.DrawString("Year", columnHeaderFont, brush, leftMargin + 450, yPos);
            e.Graphics.DrawString("Semester", columnHeaderFont, brush, leftMargin + 550, yPos);
            e.Graphics.DrawString("Grades", columnHeaderFont, brush, leftMargin + 650, yPos);

            // Draw DataGridView content
            yPos += 30;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the row is not the last empty row
                if (!row.IsNewRow)
                {
                    int xPos = leftMargin;
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        string cellValue = row.Cells[i].Value?.ToString();
                        Rectangle cellBounds = new Rectangle(xPos, yPos, i == 1 ? 250 : 100, 30);
                        StringFormat format = new StringFormat
                        {
                            Alignment = StringAlignment.Near,
                            LineAlignment = StringAlignment.Center,
                            Trimming = StringTrimming.EllipsisCharacter
                        };

                        if (i == 1) // Apply word wrap for second column
                        {
                            format.FormatFlags = StringFormatFlags.LineLimit;
                        }

                        if (i == 2) // Combine 3rd and 4th column values
                        {
                            string combinedValue = "(" + cellValue + ", " + row.Cells[3].Value?.ToString() + ")";
                            e.Graphics.DrawString(combinedValue, normalFont, brush, cellBounds, format);
                            xPos += 100; // Increment xPos only once for the combined value
                            i++; // Skip the next column as it's combined
                        }
                        else
                        {
                            e.Graphics.DrawString(cellValue, normalFont, brush, cellBounds, format);
                            xPos += i == 1 ? 250 : 100;
                        }
                    }
                    yPos += 30;
                }
            }
        }
    }
}

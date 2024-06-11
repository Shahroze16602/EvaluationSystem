using System;
using System.Windows.Forms;

namespace EvaluationSystem
{
    public partial class frmReports : Form
    {
        SQLConfig config = new SQLConfig();

        public frmReports()
        {
            InitializeComponent();
        }

        private void btnCurriculum_Click(object sender, EventArgs e)
        {
            string sql = "Select * From tblsubject s, tblcourse c WHERE s.CourseId=c.CourseId AND Course LIKE '%" + txtCourse.Text + "%'";
            config.Load_DTG(sql, DataGridView1);
        }

        private void btnListStudents_Click(object sender, EventArgs e)
        {
            string sql = "SELECT  IdNo, Firstname, Lastname, MI, HomeAddress, Gender,Course,YearLevel FROM tblstudent s, tblcourse c WHERE s.CourseId=c.CourseId";
            config.Load_DTG(sql, DataGridView1);
        }

        private void btnSubjPre_Click(object sender, EventArgs e)
        {
            // Add code to preview subject here
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // Add code to handle student evaluation preview here
        }

        private void cboSubjId_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Add code to handle ComboBox selected index change here
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            // Load ComboBox with subject IDs
            string sql = "SELECT SubjectId, Subject FROM tblsubject";
            config.fiil_CBO(sql, cboSubjId);
        }
    }
}

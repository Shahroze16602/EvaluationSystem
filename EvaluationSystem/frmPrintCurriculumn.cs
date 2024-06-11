using System;
using System.Data;
using System.Windows.Forms;

namespace EvaluationSystem
{
    public partial class frmPrintCurriculumn : Form
    {
        private SQLConfig SC = new SQLConfig();
        private string sql;
        private int courseid, idno;

        public frmPrintCurriculumn(int courseid, int idno)
        {
            InitializeComponent();
            this.courseid = courseid;
            this.idno = idno;
        }

        private void loadReports(int courseid, int idno)
        {
            sql = "SELECT * FROM tblsubject s, tblgrades g, tblstudent st,tblcourse c " +
               " WHERE s.SubjectId=g.SubjectId AND g.IdNo=st.IdNo AND g.CourseId  = c.CourseId " +
               " AND c.CourseId=" + courseid + " AND g.IdNo='" + idno + "'";

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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EvaluationSystem
{
    public partial class frmFinstudents : Form
    {
        public frmFinstudents()
        {
            InitializeComponent();
        }
        SQLConfig SC = new SQLConfig();
        usableFunction UF = new usableFunction();
        string sql, idno;
        int maxrow, inc, courseid;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmFinstudents_Load(object sender, EventArgs e)
        {
            TextBox1_TextChanged(sender, e);
        }

        private void btnAddGrades_Click(object sender, EventArgs e)
        {
            idno = dtgList.CurrentRow.Cells[0].Value.ToString();
            courseid = int.Parse(dtgList.CurrentRow.Cells[7].Value.ToString());

            Form frm = new frmPrintCurriculumn(courseid,idno);
            frm.ShowDialog();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            sql = " SELECT IdNo, Firstname, Lastname, HomeAddress, Gender,Course, YearLevel,c.CourseId FROM tblstudent s, tblcourse c WHERE s.CourseId=c.CourseId AND (Firstname LIKE '%" + TextBox1.Text + "%' Or Lastname LIKE '%" + TextBox1.Text + "%' OR IdNo Like '%" + TextBox1.Text + "%')";
            SC.Load_DTG(sql, dtgList);
            dtgList.Columns[7].Visible = false;
        }
    }
}

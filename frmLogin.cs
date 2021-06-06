using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballClubManagementSystem
{
	public partial class frmLogin : Form
	{
		public frmLogin()
		{
			InitializeComponent();
		}

		SqlConnection conn;
		SqlCommand cmd;
		SqlDataReader dr;

		private void login_btnLogin_Click(object sender, EventArgs e)
		{
			string phoneNumber = login_txtPhoneNumber.Text;
			string pw = login_txtPw.Text;
			string sql = "select * from club where phoneNumber='"
				+ phoneNumber + "' and password='" + pw + "'";

			cmd = new SqlCommand(sql, conn);
			dr = cmd.ExecuteReader();
			if (dr.Read())
			{
				dr.Close();
				frmHome fHome = new frmHome();
				fHome.phoneNumber = phoneNumber;
				showAnotherForm(fHome);
			}
			else
			{
				dr.Close();
				MessageBox.Show("No account avilable with this phone number and password",
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/*show another form and close current form*/
		public void showAnotherForm(Form frm) {
			this.Hide();
			frm.Closed += (s, args) => this.Close();
			frm.Show();
		}

		void clearDataInput() {
			login_txtPhoneNumber.Text = "";
			login_txtPw.Text = "";
		}

		private void login_btnRegister_Click(object sender, EventArgs e)
		{
			frmRegister frmRe = new frmRegister();
			displayModalForm(frmRe);
			clearDataInput();
			login_txtPhoneNumber.Focus();
		}

		/*display modal form*/
		public void displayModalForm(Form frm) {
			frm.ShowDialog(this);
			frm.Dispose();
		}

		private void frmLogin_Load(object sender, EventArgs e)
		{
			conn = new SqlConnection(@"Data Source=DESKTOP-L0U2IJ8\SQLEXPRESS;
                Initial Catalog=FootballClubManagementDB;
                Persist Security Info=True;User ID=sa;
                Password=sa");
			conn.Open();
		}
	}
}

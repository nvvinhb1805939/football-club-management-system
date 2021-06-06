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
	public partial class frmHome : Form
	{
		public frmHome()
		{
			InitializeComponent();
		}

		SqlConnection conn;
		SqlCommand cmd;
		SqlDataReader dr;

		public string phoneNumber;

		int countUpdated = 0;

		/*show another form and close current form*/
		public void showAnotherForm(Form frm)
		{
			this.Hide();
			frm.Closed += (s, args) => this.Close();
			frm.Show();
		}

		private void home_btnClub_Click(object sender, EventArgs e)
		{
			frmClub frm = new frmClub();
			frm.phoneNumber = phoneNumber;
			showAnotherForm(frm);
		}

		private void home_btnPlayer_Click(object sender, EventArgs e)
		{
			frmPlayer frm = new frmPlayer();
			frm.player_phoneNumber = phoneNumber;
			showAnotherForm(frm);
		}

		private void home_btnCoach_Click(object sender, EventArgs e)
		{
			frmCoach frm = new frmCoach();
			frm.coach_phoneNumber = phoneNumber;
			showAnotherForm(frm);
		}

		

		private void home_btnLogOut_Click(object sender, EventArgs e)
		{
			frmLogin frm = new frmLogin();
			showAnotherForm(frm);
		}

		private void home_btnManager_Click(object sender, EventArgs e)
		{
			frmManager frm = new frmManager();
			frm.manager_phoneNumber = phoneNumber;
			showAnotherForm(frm);
		}

		private void frmHome_Load(object sender, EventArgs e)
		{
			conn = new SqlConnection(@"Data Source=DESKTOP-L0U2IJ8\SQLEXPRESS;
                Initial Catalog=FootballClubManagementDB;MultipleActiveResultSets=true;
				Persist Security Info=True;User ID=sa; Password=sa");
			conn.Open();
			string query = "select logopath, countupdate from club " +
				"where phoneNumber = @phoneNumber";
			cmd = new SqlCommand(query, conn);
			cmd.Parameters.Add("@phoneNumber", phoneNumber);
			dr = cmd.ExecuteReader();
			if (dr.Read()) {
				countUpdated = int.Parse(dr["countUpdate"].ToString());
				if (countUpdated != 0)
				{
					string filePath = dr["logoPath"].ToString();
					home_btnClub.Image = System.Drawing.Image.FromFile(@filePath);
				}
				else {
					home_btnClub.Image = System.Drawing.Image.FromFile(
						@"D:\Programs\C#\FootballClubManagementSystem\img\icon_team.png");
				}
			}
		}
	}
}

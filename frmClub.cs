using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace FootballClubManagementSystem
{
	public partial class frmClub : Form
	{
		public frmClub()
		{
			InitializeComponent();
		}

		SqlConnection conn;
		SqlCommand cmd;
		SqlDataReader dr;

		public string phoneNumber;
		string logoFilePath;
		string homeJerseyFilePath;
		string awayJerseyFilePath;

		int countUpdated = 0;

		public void loadDataIntoForm() 
		{
			string query = "select phoneNumber, name, email, " +
				"address, logo, logoPath, homeJersey, homeJerseyPath," +
				" awayJersey, awayJerseyPath, countUpdate from club " +
				"where phoneNumber = @phoneNumber";
			cmd = new SqlCommand(query, conn);
			cmd.Parameters.Add("@phoneNumber", phoneNumber);
			dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				club_txtPhoneNumber.Text = phoneNumber;
				club_txtNameClub.Text = dr["name"].ToString();
				club_txtEmail.Text = dr["email"].ToString(); ;
				club_txtAddress.Text = dr["address"].ToString();
				logoFilePath = dr["logoPath"].ToString();
				homeJerseyFilePath = dr["homeJerseyPath"].ToString();
				awayJerseyFilePath = dr["awayJerseyPath"].ToString();
				countUpdated = int.Parse(dr["countUpdate"].ToString());

				if (countUpdated != 0)
				{
					SqlDataAdapter dataAdapter = new SqlDataAdapter(
					new SqlCommand("SELECT logo FROM club WHERE " +
					"phoneNumber = '" + phoneNumber + "'", conn));
					DataSet dataSet = new DataSet();
					dataAdapter.Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count == 1)
					{
						Byte[] data = new Byte[0];
						data = (Byte[])(dataSet.Tables[0].Rows[0]["logo"]);
						MemoryStream mem = new MemoryStream(data);
						club_ptbLogo.Image = Image.FromStream(mem);
					}

					SqlDataAdapter dataAdapter2 = new SqlDataAdapter(
						new SqlCommand("SELECT homeJersey FROM club " +
						"WHERE phoneNumber = '" + phoneNumber + "'", conn));
					DataSet dataSet2 = new DataSet();
					dataAdapter2.Fill(dataSet2);
					if (dataSet2.Tables[0].Rows.Count == 1)
					{
						Byte[] data = new Byte[0];
						data = (Byte[])(dataSet2.Tables[0].Rows[0]["homeJersey"]);
						MemoryStream mem = new MemoryStream(data);
						club_ptbHomeJersey.Image = Image.FromStream(mem);
					}

					SqlDataAdapter dataAdapter3 = new SqlDataAdapter(
						new SqlCommand("SELECT awayJersey FROM club " +
						"WHERE phoneNumber = '" + phoneNumber + "'", conn));
					DataSet dataSet3 = new DataSet();
					dataAdapter3.Fill(dataSet3);
					if (dataSet3.Tables[0].Rows.Count == 1)
					{
						Byte[] data = new Byte[0];
						data = (Byte[])(dataSet3.Tables[0].Rows[0]["awayJersey"]);
						MemoryStream mem = new MemoryStream(data);
						club_ptbAwayJersey.Image = Image.FromStream(mem);
					}
				}
			}
		}

		private void frmClub_Load(object sender, EventArgs e)
		{
			conn = new SqlConnection(@"Data Source=DESKTOP-L0U2IJ8\SQLEXPRESS;
                Initial Catalog=FootballClubManagementDB;MultipleActiveResultSets=true;
				Persist Security Info=True;User ID=sa; Password=sa");
			conn.Open();
			loadDataIntoForm();
			club_txtNameClub.Focus();
		}

		private void club_btnUploadLogo_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)" +
				"|*.jpg; *.jpeg; *.gif; *.bmp";
			if (open.ShowDialog() == DialogResult.OK)
			{
				club_ptbLogo.Image = new Bitmap(open.FileName);
				logoFilePath = open.FileName;
			}
		}

		public bool isDigit(String str)
		{
			bool result = false;
			Regex regex = new Regex(@"^[0-9]+$");
			if (regex.IsMatch(str))
				result = true;
			return result;
		}

		public bool IsValidEmail(string emailAddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailAddress);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		public void insertDataIntoDB()
		{
			string name = club_txtNameClub.Text.Trim();
			string phoneNumber = club_txtPhoneNumber.Text.Trim();
			string email = club_txtEmail.Text.Trim();
			string address = club_txtAddress.Text.Trim();

			/*check valid input data*/
			if (name.Length == 0)
			{
				MessageBox.Show("Name must be not empty!!", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				club_txtNameClub.Focus();
				return;
			}
			if (email.Length == 0 || !IsValidEmail(email))
			{
				MessageBox.Show("Email must be not empty and contains sign '@' !!",
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				club_txtEmail.Focus();
				return;
			}
			if (address.Length == 0)
			{
				MessageBox.Show("Address must be not empty!!",
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				club_txtAddress.Focus();
				return;
			}
			if (logoFilePath.Length == 0)
			{
				MessageBox.Show("Please choose a logo!!",
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				club_btnUploadLogo.Focus();
				return;
			}
			if (homeJerseyFilePath.Length == 0)
			{
				MessageBox.Show("Please choose a home jersey!!",
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				club_btnUploadHomeJersey.Focus();
				return;
			}
			if (awayJerseyFilePath.Length == 0)
			{
				MessageBox.Show("Please choose an away jersey!!",
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				club_btnUploadAwayJersey.Focus();
				return;
			}

			countUpdated++;

			byte[] logo = File.ReadAllBytes(logoFilePath);
			byte[] homeJersey = File.ReadAllBytes(homeJerseyFilePath);
			byte[] awayJersey = File.ReadAllBytes(awayJerseyFilePath);

			cmd = new SqlCommand("update club set name = @name, " +
				"email = @email, address = @address, logo = @logo, " +
				"logoPath = @logoPath, homeJersey = @homeJersey, " +
				"homeJerseyPath = @homeJerseyPath, awayJersey = @awayJersey," +
				"awayJerseyPath = @awayJerseyPath, countUpdate = @countUpdate" +
				" where " + "phoneNumber = @phoneNumber", conn);
			cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
			cmd.Parameters.AddWithValue("@name", club_txtNameClub.Text);
			cmd.Parameters.AddWithValue("@email", club_txtEmail.Text);
			cmd.Parameters.AddWithValue("@address", club_txtAddress.Text);
			cmd.Parameters.AddWithValue("@logo", logo);
			cmd.Parameters.AddWithValue("@logoPath", logoFilePath);
			cmd.Parameters.AddWithValue("@homeJersey", homeJersey);
			cmd.Parameters.AddWithValue("@homeJerseyPath", homeJerseyFilePath);
			cmd.Parameters.AddWithValue("@awayJersey", awayJersey);
			cmd.Parameters.AddWithValue("@awayJerseyPath", awayJerseyFilePath);
			cmd.Parameters.AddWithValue("@countUpdate", countUpdated);


			cmd.ExecuteNonQuery();

			MessageBox.Show("Update info success!",
				   "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void club_btnOK_Click(object sender, EventArgs e)
		{
			insertDataIntoDB();
		}

		private void club_btnUploadHomeJersey_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)" +
				"|*.jpg; *.jpeg; *.gif; *.bmp";
			if (open.ShowDialog() == DialogResult.OK)
			{
				club_ptbHomeJersey.Image = new Bitmap(open.FileName);
				homeJerseyFilePath = open.FileName;
			}
		}

		private void club_btnUploadAwayJersey_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)" +
				"|*.jpg; *.jpeg; *.gif; *.bmp";
			if (open.ShowDialog() == DialogResult.OK)
			{
				club_ptbAwayJersey.Image = new Bitmap(open.FileName);
				awayJerseyFilePath = open.FileName;
			}
		}

		private void club_btnCancel_Click(object sender, EventArgs e)
		{
			loadDataIntoForm();
		}

		/*show another form and close current form*/
		public void showAnotherForm(Form frm) {
			this.Hide();
			frm.Closed += (s, args) => this.Close();
			frm.Show();
		}

		private void club_btnBackHome_Click(object sender, EventArgs e)
		{
			frmHome frm = new frmHome();
			frm.phoneNumber = phoneNumber;
			showAnotherForm(frm);
		}

		private void club_btnLogOut_Click(object sender, EventArgs e)
		{
			frmLogin frm = new frmLogin();
			showAnotherForm(frm);
		}
	}
}

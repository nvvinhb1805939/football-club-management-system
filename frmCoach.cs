using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballClubManagementSystem
{
	public partial class frmCoach : Form
	{
		public frmCoach()
		{
			InitializeComponent();
		}

		string avtFilePath = "";
		SqlConnection conn;
		SqlCommand cmd;
		SqlDataReader dr;
		int canADD = 0;
		int type = 0;
		int countUpdated = 0;
		String idCoach = "";

		public string coach_phoneNumber;


		void clearDataInput() {
			coach_txtPhonenumber.Text = "";
			coach_txtName.Text = "";
			coach_dpkerDOB.Text = "";
			coach_txtPOB.Text = "";
			coach_txtAge.Text = "";
			coach_txtHeight.Text = "";
			coach_txtCitizenship.Text = "";
			coach_dpJoined.Text = "";
			coach_txtRole.Text = "";
			coach_txtSalary.Text = "";
			avtFilePath = "";
			coach_pbAvt.Image = System.Drawing.Bitmap.FromFile(
				"D:\\Programs\\C#\\FootballClubManagementSystem\\img\\user.png");
		}

		private void coach_btnAdd_Click(object sender, EventArgs e)
		{
			coach_txtPhonenumber.Enabled = true;
			coach_txtPhonenumber.Focus();
			clearDataInput();
			canADD = 1;
		}

		void loadDataToTable() {
			conn = new SqlConnection(@"Data Source=DESKTOP-L0U2IJ8\SQLEXPRESS;
                Initial Catalog=FootballClubManagementDB;MultipleActiveResultSets=true;
				Persist Security Info=True;User ID=sa; Password=sa");
			conn.Open();
			SqlDataAdapter da = new SqlDataAdapter(
					"SELECT phoneNumber, name, dob, citizenship FROM coach",
					"server = DESKTOP-L0U2IJ8\\SQLEXPRESS; " +
					"database = FootballClubManagementDB; " +
					"UID = sa; password = sa");

			DataSet ds = new DataSet();
			da.Fill(ds, "coach");

			coach_dtgCoachList.Columns[0].Name = "phoneNumber";
			coach_dtgCoachList.Columns[1].Name = "name";
			coach_dtgCoachList.Columns[2].Name = "dob";
			coach_dtgCoachList.Columns[3].Name = "citizenship";

			coach_dtgCoachList.Columns[0].DataPropertyName = "phoneNumber";
			coach_dtgCoachList.Columns[1].DataPropertyName = "name";
			coach_dtgCoachList.Columns[2].DataPropertyName = "dob";
			coach_dtgCoachList.Columns[3].DataPropertyName = "citizenship";
			coach_dtgCoachList.DataSource = ds.Tables["coach"].DefaultView;
		}

		private void coach_btnDel_Click(object sender, EventArgs e)
		{
			cmd = new SqlCommand("delete from coach where phoneNumber=@idCoach", conn);
			cmd.Parameters.AddWithValue("@idCoach", idCoach);
			cmd.ExecuteNonQuery();
			conn.Close();
			MessageBox.Show("Deleted data successfully!");

			loadDataToTable();
			clearDataInput();

			coach_txtPhonenumber.Enabled = false;
			coach_btnDel.Enabled = false;
		}

		private void coach_btnLogOut_Click(object sender, EventArgs e)
		{
			frmLogin frm = new frmLogin();
			showAnotherForm(frm);
		}

		void displayDataToTextBox() {
			string query = "select *" +
						" from coach " +
						"where phoneNumber = @phoneNumber";
			cmd = new SqlCommand(query, conn);
			cmd.Parameters.Add("@phoneNumber", idCoach);
			dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				coach_txtPhonenumber.Text = idCoach;
				coach_txtName.Text = dr["name"].ToString();
				coach_dpkerDOB.Text = dr["dob"].ToString(); ;
				coach_txtPOB.Text = dr["pob"].ToString();
				coach_txtAge.Text = dr["age"].ToString();
				coach_txtHeight.Text = dr["height"].ToString();
				coach_txtCitizenship.Text = dr["citizenship"].ToString();
				coach_dpJoined.Text = dr["joined"].ToString();
				coach_txtRole.Text = dr["role"].ToString();
				coach_txtSalary.Text = dr["salary"].ToString();
				avtFilePath = dr["avtPath"].ToString();
				countUpdated = int.Parse(dr["countUpdate"].ToString());

				if (countUpdated != 0)
				{
					SqlDataAdapter dataAdapter = new SqlDataAdapter(
					new SqlCommand("SELECT avt FROM coach WHERE " +
					"phoneNumber = '" + idCoach + "'", conn));
					DataSet dataSet = new DataSet();
					dataAdapter.Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count == 1)
					{
						Byte[] data = new Byte[0];
						data = (Byte[])(dataSet.Tables[0].Rows[0]["avt"]);
						MemoryStream mem = new MemoryStream(data);
						coach_pbAvt.Image = Image.FromStream(mem);
					}
				}
			}
		}

		private void coach_btnCancel_Click(object sender, EventArgs e)
		{
			if (canADD == 0)
				displayDataToTextBox();
			else
			{
				clearDataInput();
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

		public bool isFloat(String str)
		{
			bool result = false;
			Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
			if (regex.IsMatch(str))
				result = true;
			return result;
		}


		private void coach_btnOK_Click(object sender, EventArgs e)
		{
			string phoneNumber = coach_txtPhonenumber.Text.Trim();
			string name = coach_txtName.Text.Trim();
			string DoB = coach_dpkerDOB.Value.ToString();
			string PoB = coach_txtPOB.Text.Trim();
			string age = coach_txtAge.Text.Trim();
			string height = coach_txtHeight.Text.Trim();
			string citizenship = coach_txtCitizenship.Text.Trim();
			string joined = coach_dpJoined.Value.ToString();
			string role = coach_txtRole.Text.Trim();
			string salary = coach_txtSalary.Text.Trim();

			if (phoneNumber.Length == 0 || phoneNumber.Length > 15 || !isDigit(phoneNumber))
			{
				MessageBox.Show("Phone number must be " +
					"digit less than or equal to 15 characters" +
					" and not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				coach_txtPhonenumber.Focus();
				return;
			}
			if (name.Length == 0)
			{
				MessageBox.Show("Name must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				coach_txtName.Focus();
				return;
			}
			if (PoB.Length == 0)
			{
				MessageBox.Show("Place of birth must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				coach_txtPOB.Focus();
				return;
			}
			if (age.Length == 0 || !isDigit(age))
			{
				MessageBox.Show("Age must be a digit and not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				coach_txtAge.Focus();
				return;
			}
			if (height.Length == 0 || !isFloat(height))
			{
				MessageBox.Show("Heigth must be a digit and not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				coach_txtHeight.Focus();
				return;
			}
			if (citizenship.Length == 0)
			{
				MessageBox.Show("Citizenship must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				coach_txtCitizenship.Focus();
				return;
			}
			if (role.Length == 0)
			{
				MessageBox.Show("Role must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				coach_txtRole.Focus();
				return;
			}
			if (salary.Length == 0 || !isDigit(salary))
			{
				MessageBox.Show("Salary must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				coach_txtSalary.Focus();
				return;
			}
			if (avtFilePath.Length == 0) {
				MessageBox.Show("Please choose an avatar!", "Error", MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				coach_btnUploadAvt.Focus();
				return;
			}

			try {
				countUpdated++;

				byte[] avt = File.ReadAllBytes(avtFilePath);

				string query;
				if (canADD == 0)
				{
					query = "update coach set name = @name, " +
						"dob = @dob, pob = @pob, age = @age, " +
						"height = @height, citizenship = @citizenship, " +
						"joined = @joined, role = @role," +
						"salary = @salary, avt = @avt, avtpath = @avtpath" +
						" where phoneNumber = @phoneNumber";
					type = 0;
				}
				else
				{
					query = "insert into coach values (@phoneNumber, @name, " +
					"@dob, @pob, @age, @height, @citizenship, @joined, @role," +
					"@salary,@avt, @avtpath, @countUpdate)";
					type = 1;
				}

				cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@phoneNumber", coach_txtPhonenumber.Text);
				cmd.Parameters.AddWithValue("@name", coach_txtName.Text);
				cmd.Parameters.AddWithValue("@dob", coach_dpkerDOB.Value.ToString());
				cmd.Parameters.AddWithValue("@pob", coach_txtPOB.Text);
				cmd.Parameters.AddWithValue("@age", coach_txtAge.Text);
				cmd.Parameters.AddWithValue("@height", coach_txtHeight.Text);
				cmd.Parameters.AddWithValue("@citizenship", coach_txtCitizenship.Text);
				cmd.Parameters.AddWithValue("@joined", coach_dpJoined.Value.ToString());
				cmd.Parameters.AddWithValue("@role", coach_txtRole.Text);
				cmd.Parameters.AddWithValue("@salary", coach_txtSalary.Text);
				cmd.Parameters.AddWithValue("@avt", avt);
				cmd.Parameters.AddWithValue("@avtPath", avtFilePath);
				cmd.Parameters.AddWithValue("@countUpdate", countUpdated);

				cmd.ExecuteNonQuery();
			} catch (Exception ex) {
				MessageBox.Show("This coach already exists!",
						   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				type = -9999;
			}

			if (type == 0)
				MessageBox.Show("Update info success!",
				   "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else if (type == 1)
			{
				MessageBox.Show("Add info success!",
				   "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
				canADD = 0;
				clearDataInput();
			}

			loadDataToTable();

			coach_txtPhonenumber.Enabled = false;
		}

		private void coach_btnUploadAvt_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)" +
				"|*.jpg; *.jpeg; *.gif; *.bmp";
			if (open.ShowDialog() == DialogResult.OK)
			{
				coach_pbAvt.Image = new Bitmap(open.FileName);
				avtFilePath = open.FileName;
			}
		}

		private void frmCoach_Load(object sender, EventArgs e)
		{
			loadDataToTable();
			coach_txtPhonenumber.Enabled = false;
		}

		private void coach_dtgCoachList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			coach_btnDel.Enabled = true;
			if (e.RowIndex >= 0)
			{
				DataGridViewRow row = this.coach_dtgCoachList.Rows[e.RowIndex];
				idCoach = row.Cells[0].Value.ToString();

				displayDataToTextBox();
			}
		}

		public void showAnotherForm(Form frm)
		{
			this.Hide();
			frm.Closed += (s, args) => this.Close();
			frm.Show();
		}

		private void coach_btnBackHome_Click(object sender, EventArgs e)
		{
			frmHome frm = new frmHome();
			frm.phoneNumber = coach_phoneNumber;
			showAnotherForm(frm);
		}
	}
}

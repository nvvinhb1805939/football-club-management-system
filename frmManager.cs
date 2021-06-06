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
	public partial class frmManager : Form
	{
		public frmManager()
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

		public string manager_phoneNumber;


		void clearDataInput()
		{
			manager_txtPhonenumber.Text = "";
			manager_txtName.Text = "";
			manager_dpkerDOB.Text = "";
			manager_txtPOB.Text = "";
			manager_txtAge.Text = "";
			manager_txtHeight.Text = "";
			manager_txtCitizenship.Text = "";
			manager_dpJoined.Text = "";
			manager_txtRole.Text = "";
			manager_txtSalary.Text = "";
			avtFilePath = "";
			manager_pbAvt.Image = System.Drawing.Bitmap.FromFile(
				"D:\\Programs\\C#\\FootballClubManagementSystem\\img\\user.png");
		}

		void loadDataToTable()
		{
			conn = new SqlConnection(@"Data Source=DESKTOP-L0U2IJ8\SQLEXPRESS;
                Initial Catalog=FootballClubManagementDB;MultipleActiveResultSets=true;
				Persist Security Info=True;User ID=sa; Password=sa");
			conn.Open();
			SqlDataAdapter da = new SqlDataAdapter(
					"SELECT phoneNumber, name, dob, citizenship FROM  manager",
					"server = DESKTOP-L0U2IJ8\\SQLEXPRESS; " +
					"database = FootballClubManagementDB; " +
					"UID = sa; password = sa");

			DataSet ds = new DataSet();
			da.Fill(ds, "manager");

			manager_dtgCoachList.Columns[0].Name = "phoneNumber";
			manager_dtgCoachList.Columns[1].Name = "name";
			manager_dtgCoachList.Columns[2].Name = "dob";
			manager_dtgCoachList.Columns[3].Name = "citizenship";

			manager_dtgCoachList.Columns[0].DataPropertyName = "phoneNumber";
			manager_dtgCoachList.Columns[1].DataPropertyName = "name";
			manager_dtgCoachList.Columns[2].DataPropertyName = "dob";
			manager_dtgCoachList.Columns[3].DataPropertyName = "citizenship";
			manager_dtgCoachList.DataSource = ds.Tables["manager"].DefaultView;

		}

		private void frmManager_Load(object sender, EventArgs e)
		{
			loadDataToTable();
			manager_txtPhonenumber.Enabled = false;

		}

		private void manager_btnAdd_Click(object sender, EventArgs e)
		{
			manager_txtPhonenumber.Enabled = true;
			manager_txtPhonenumber.Focus();
			clearDataInput();
			canADD = 1;
		}

		private void manager_btnDel_Click(object sender, EventArgs e)
		{
			cmd = new SqlCommand("delete from  manager where phoneNumber=@idCoach", conn);
			cmd.Parameters.AddWithValue("@idCoach", idCoach);
			cmd.ExecuteNonQuery();
			conn.Close();
			MessageBox.Show("Deleted data successfully!");

			loadDataToTable();
			clearDataInput();

			manager_txtPhonenumber.Enabled = false;
			manager_btnDel.Enabled = false;
		}

		public void showAnotherForm(Form frm)
		{
			this.Hide();
			frm.Closed += (s, args) => this.Close();
			frm.Show();
		}

		private void manager_btnLogOut_Click(object sender, EventArgs e)
		{
			frmLogin frm = new frmLogin();
			showAnotherForm(frm);
		}

		void displayDataToTextBox()
		{
			string query = "select *" +
						" from  manager " +
						"where phoneNumber = @phoneNumber";
			cmd = new SqlCommand(query, conn);
			cmd.Parameters.Add("@phoneNumber", idCoach);
			dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				manager_txtPhonenumber.Text = idCoach;
				manager_txtName.Text = dr["name"].ToString();
				manager_dpkerDOB.Text = dr["dob"].ToString(); ;
				manager_txtPOB.Text = dr["pob"].ToString();
				manager_txtAge.Text = dr["age"].ToString();
				manager_txtHeight.Text = dr["height"].ToString();
				manager_txtCitizenship.Text = dr["citizenship"].ToString();
				manager_dpJoined.Text = dr["joined"].ToString();
				manager_txtRole.Text = dr["role"].ToString();
				manager_txtSalary.Text = dr["salary"].ToString();
				avtFilePath = dr["avtPath"].ToString();
				countUpdated = int.Parse(dr["countUpdate"].ToString());

				if (countUpdated != 0)
				{
					SqlDataAdapter dataAdapter = new SqlDataAdapter(
					new SqlCommand("SELECT avt FROM  manager WHERE " +
					"phoneNumber = '" + idCoach + "'", conn));
					DataSet dataSet = new DataSet();
					dataAdapter.Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count == 1)
					{
						Byte[] data = new Byte[0];
						data = (Byte[])(dataSet.Tables[0].Rows[0]["avt"]);
						MemoryStream mem = new MemoryStream(data);
						manager_pbAvt.Image = Image.FromStream(mem);
					}
				}
			}
		}

		private void manager_btnCancel_Click(object sender, EventArgs e)
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

		private void manager_btnOK_Click(object sender, EventArgs e)
		{
			string phoneNumber = manager_txtPhonenumber.Text.Trim();
			string name = manager_txtName.Text.Trim();
			string DoB = manager_dpkerDOB.Value.ToString();
			string PoB = manager_txtPOB.Text.Trim();
			string age = manager_txtAge.Text.Trim();
			string height = manager_txtHeight.Text.Trim();
			string citizenship = manager_txtCitizenship.Text.Trim();
			string joined = manager_dpJoined.Value.ToString();
			string role = manager_txtRole.Text.Trim();
			string salary = manager_txtSalary.Text.Trim();

			if (phoneNumber.Length == 0 || phoneNumber.Length > 15 || !isDigit(phoneNumber))
			{
				MessageBox.Show("Phone number must be " +
					"digit less than or equal to 15 characters" +
					" and not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				manager_txtPhonenumber.Focus();
				return;
			}
			if (name.Length == 0)
			{
				MessageBox.Show("Name must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				manager_txtName.Focus();
				return;
			}
			if (PoB.Length == 0)
			{
				MessageBox.Show("Place of birth must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				manager_txtPOB.Focus();
				return;
			}
			if (age.Length == 0 || !isDigit(age))
			{
				MessageBox.Show("Age must be a digit and not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				manager_txtAge.Focus();
				return;
			}
			if (height.Length == 0 || !isFloat(height))
			{
				MessageBox.Show("Heigth must be a digit and not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				manager_txtHeight.Focus();
				return;
			}
			if (citizenship.Length == 0)
			{
				MessageBox.Show("Citizenship must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				manager_txtCitizenship.Focus();
				return;
			}
			if (role.Length == 0)
			{
				MessageBox.Show("Role must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				manager_txtRole.Focus();
				return;
			}
			if (salary.Length == 0 || !isDigit(salary))
			{
				MessageBox.Show("Salary must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				manager_txtSalary.Focus();
				return;
			}
			if (avtFilePath.Length == 0)
			{
				MessageBox.Show("Please choose an avatar!", "Error", MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				manager_btnUploadAvt.Focus();
				return;
			}

			try
			{
				countUpdated++;

				byte[] avt = File.ReadAllBytes(avtFilePath);

				string query;
				if (canADD == 0)
				{
					query = "update  manager set name = @name, " +
					"dob = @dob, pob = @pob, age = @age, " +
					"height = @height, citizenship = @citizenship, " +
					"joined = @joined, role = @role," +
					"salary = @salary, avt = @avt, avtpath = @avtpath" +
					" where phoneNumber = @phoneNumber";
					type = 0;
				}
				else
				{
					query = "insert into  manager values (@phoneNumber, @name, " +
					"@dob, @pob, @age, @height, @citizenship, @joined, @role," +
					"@salary,@avt, @avtpath, @countUpdate)";
					type = 1;
				}
				cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@phoneNumber", manager_txtPhonenumber.Text);
				cmd.Parameters.AddWithValue("@name", manager_txtName.Text);
				cmd.Parameters.AddWithValue("@dob", manager_dpkerDOB.Value.ToString());
				cmd.Parameters.AddWithValue("@pob", manager_txtPOB.Text);
				cmd.Parameters.AddWithValue("@age", manager_txtAge.Text);
				cmd.Parameters.AddWithValue("@height", manager_txtHeight.Text);
				cmd.Parameters.AddWithValue("@citizenship", manager_txtCitizenship.Text);
				cmd.Parameters.AddWithValue("@joined", manager_dpJoined.Value.ToString());
				cmd.Parameters.AddWithValue("@role", manager_txtRole.Text);
				cmd.Parameters.AddWithValue("@salary", manager_txtSalary.Text);
				cmd.Parameters.AddWithValue("@avt", avt);
				cmd.Parameters.AddWithValue("@avtPath", avtFilePath);
				cmd.Parameters.AddWithValue("@countUpdate", countUpdated);

				cmd.ExecuteNonQuery();
			}
			catch (Exception ex) {
				MessageBox.Show("This manager already exists!",
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

			manager_txtPhonenumber.Enabled = false;
		}

		private void manager_btnUploadAvt_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)" +
				"|*.jpg; *.jpeg; *.gif; *.bmp";
			if (open.ShowDialog() == DialogResult.OK)
			{
				manager_pbAvt.Image = new Bitmap(open.FileName);
				avtFilePath = open.FileName;
			}
		}

		private void manager_dtgCoachList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			manager_btnDel.Enabled = true;
			if (e.RowIndex >= 0)
			{
				DataGridViewRow row = this.manager_dtgCoachList.Rows[e.RowIndex];
				idCoach = row.Cells[0].Value.ToString();

				displayDataToTextBox();
			}
		}

		private void manager_btnBackHome_Click(object sender, EventArgs e)
		{
			frmHome frm = new frmHome();
			frm.phoneNumber = manager_phoneNumber;
			showAnotherForm(frm);
		}
	}
}

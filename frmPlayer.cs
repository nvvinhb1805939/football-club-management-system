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
	public partial class frmPlayer : Form
	{
		public frmPlayer()
		{
			InitializeComponent();
		}

		public string player_phoneNumber;

		string avtFilePath = "";
		SqlConnection conn;
		SqlCommand cmd;
		SqlDataReader dr;
		int canADD = 0;
		int type = 0;
		int countUpdated = 0;
		String idCoach = "";

		void clearDataInput()
		{
			player_txtPhonenumber.Text = "";
			player_txtName.Text = "";
			player_dpkerDOB.Text = "";
			player_txtPOB.Text = "";
			player_txtAge.Text = "";
			player_txtHeight.Text = "";
			player_txtCitizenship.Text = "";
			player_dpJoined.Text = "";
			player_txtRole.Text = "";
			player_txtSalary.Text = "";
			player_txtFoot.Text = "";
			player_txtPosition.Text = "";
			avtFilePath = "";
			player_pbAvt.Image = System.Drawing.Bitmap.FromFile(
				"D:\\Programs\\C#\\FootballClubManagementSystem\\img\\user.png");
		}

		void loadDataToTable()
		{
			conn = new SqlConnection(@"Data Source=DESKTOP-L0U2IJ8\SQLEXPRESS;
                Initial Catalog=FootballClubManagementDB;MultipleActiveResultSets=true;
				Persist Security Info=True;User ID=sa; Password=sa");
			conn.Open();
			SqlDataAdapter da = new SqlDataAdapter(
					"SELECT phoneNumber, name, dob, citizenship, avtPath FROM  player",
					"server = DESKTOP-L0U2IJ8\\SQLEXPRESS; " +
					"database = FootballClubManagementDB; " +
					"UID = sa; password = sa");

			DataSet ds = new DataSet();
			da.Fill(ds, "player");

			player_dtgPlayerList.Columns[0].Name = "phoneNumber";
			player_dtgPlayerList.Columns[1].Name = "name";
			player_dtgPlayerList.Columns[2].Name = "dob";
			player_dtgPlayerList.Columns[3].Name = "citizenship";

			player_dtgPlayerList.Columns[0].DataPropertyName = "phoneNumber";
			player_dtgPlayerList.Columns[1].DataPropertyName = "name";
			player_dtgPlayerList.Columns[2].DataPropertyName = "dob";
			player_dtgPlayerList.Columns[3].DataPropertyName = "citizenship";
			player_dtgPlayerList.DataSource = ds.Tables["player"].DefaultView;

		}

		private void frmPlayer_Load(object sender, EventArgs e)
		{
			loadDataToTable();
			player_txtPhonenumber.Enabled = false;
		}

		public void showAnotherForm(Form frm)
		{
			this.Hide();
			frm.Closed += (s, args) => this.Close();
			frm.Show();
		}

		private void player_btnLogOut_Click(object sender, EventArgs e)
		{
			frmLogin frm = new frmLogin();
			showAnotherForm(frm);
		}

		void displayDataToTextBox()
		{
			string query = "select * from  player " +
						"where phoneNumber = @phoneNumber";
			cmd = new SqlCommand(query, conn);
			cmd.Parameters.Add("@phoneNumber", idCoach);
			dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				player_txtPhonenumber.Text = idCoach;
				player_txtName.Text = dr["name"].ToString();
				player_dpkerDOB.Text = dr["dob"].ToString(); ;
				player_txtPOB.Text = dr["pob"].ToString();
				player_txtAge.Text = dr["age"].ToString();
				player_txtHeight.Text = dr["height"].ToString();
				player_txtCitizenship.Text = dr["citizenship"].ToString();
				player_dpJoined.Text = dr["joined"].ToString();
				player_txtRole.Text = dr["role"].ToString();
				player_txtSalary.Text = dr["salary"].ToString();
				avtFilePath = dr["avtPath"].ToString();
				player_txtPosition.Text = dr["position"].ToString();
				player_txtFoot.Text = dr["foot"].ToString();
				countUpdated = int.Parse(dr["countUpdate"].ToString());

				if (countUpdated != 0)
				{
					SqlDataAdapter dataAdapter = new SqlDataAdapter(
					new SqlCommand("SELECT avt FROM  player WHERE " +
					"phoneNumber = '" + idCoach + "'", conn));
					DataSet dataSet = new DataSet();
					dataAdapter.Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count == 1)
					{
						Byte[] data = new Byte[0];
						data = (Byte[])(dataSet.Tables[0].Rows[0]["avt"]);
						MemoryStream mem = new MemoryStream(data);
						player_pbAvt.Image = Image.FromStream(mem);
					}
				}
			}
		}

		private void player_btnCancel_Click(object sender, EventArgs e)
		{
			if (canADD == 0)
				displayDataToTextBox();
			else {
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

		private void player_btnOK_Click_1(object sender, EventArgs e)
		{
			string phoneNumber = player_txtPhonenumber.Text.Trim();
			string name = player_txtName.Text.Trim();
			string DoB = player_dpkerDOB.Value.ToString();
			string PoB = player_txtPOB.Text.Trim();
			string age = player_txtAge.Text.Trim();
			string height = player_txtHeight.Text.Trim();
			string citizenship = player_txtCitizenship.Text.Trim();
			string joined = player_dpJoined.Value.ToString();
			string role = player_txtRole.Text.Trim();
			string salary = player_txtSalary.Text.Trim();
			string position = player_txtPosition.Text.Trim();
			string foot = player_txtFoot.Text.Trim();

			if (phoneNumber.Length == 0 || phoneNumber.Length > 15 || !isDigit(phoneNumber))
			{
				MessageBox.Show("Phone number must be " +
					"digit less than or equal to 15 characters" +
					" and not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				player_txtPhonenumber.Focus();
				return;
			}
			if (name.Length == 0)
			{
				MessageBox.Show("Name must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				player_txtName.Focus();
				return;
			}
			if (PoB.Length == 0)
			{
				MessageBox.Show("Place of birth must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				player_txtPOB.Focus();
				return;
			}
			if (age.Length == 0 || !isDigit(age))
			{
				MessageBox.Show("Age must be a digit and not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				player_txtAge.Focus();
				return;
			}
			if (height.Length == 0 || !isFloat(height))
			{
				MessageBox.Show("Heigth must be a digit and not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				player_txtHeight.Focus();
				return;
			}
			if (position.Length == 0)
			{
				MessageBox.Show("Position must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				player_txtPosition.Focus();
				return;
			}
			if (foot.Length == 0)
			{
				MessageBox.Show("Foot must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				player_txtFoot.Focus();
				return;
			}
			if (citizenship.Length == 0)
			{
				MessageBox.Show("Citizenship must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				player_txtCitizenship.Focus();
				return;
			}
			if (role.Length == 0)
			{
				MessageBox.Show("Role must be not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				player_txtRole.Focus();
				return;
			}
			if (salary.Length == 0 || !isDigit(salary))
			{
				MessageBox.Show("Salary must be a digit and not empty!", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				player_txtSalary.Focus();
				return;
			}
			if (avtFilePath.Length == 0)
			{
				MessageBox.Show("Please choose an avatar!", "Error", MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				player_btnUploadAvt.Focus();
				return;
			}

			try
			{
				countUpdated++;

				byte[] avt = File.ReadAllBytes(avtFilePath);

				string query;
				if (canADD == 0)
				{
					query = "update  player set name = @name, " +
					"dob = @dob, pob = @pob, age = @age, " +
					"height = @height, position = @position," +
					"foot = @foot, citizenship = @citizenship, " +
					"joined = @joined, role = @role," +
					"salary = @salary, avt = @avt, avtpath = @avtpath" +
					" where phoneNumber = @phoneNumber";
					type = 0;
				}

				else {
					query = "insert into  player values (@phoneNumber, " +
						"@name, @dob, @pob, @age, @height, @citizenship, " +
						"@joined, @role, @salary,@avt, @avtpath, @position," +
						" @foot, @countUpdate)";
					type = 1;
				}
					
				cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@phoneNumber", player_txtPhonenumber.Text);
				cmd.Parameters.AddWithValue("@name", player_txtName.Text);
				cmd.Parameters.AddWithValue("@dob", player_dpkerDOB.Value.ToString());
				cmd.Parameters.AddWithValue("@pob", player_txtPOB.Text);
				cmd.Parameters.AddWithValue("@age", player_txtAge.Text);
				cmd.Parameters.AddWithValue("@height", player_txtHeight.Text);
				cmd.Parameters.AddWithValue("@citizenship", player_txtCitizenship.Text);
				cmd.Parameters.AddWithValue("@joined", player_dpJoined.Value.ToString());
				cmd.Parameters.AddWithValue("@role", player_txtRole.Text);
				cmd.Parameters.AddWithValue("@salary", player_txtSalary.Text);
				cmd.Parameters.AddWithValue("@avt", avt);
				cmd.Parameters.AddWithValue("@avtPath", avtFilePath);
				cmd.Parameters.AddWithValue("@position", player_txtPosition.Text);
				cmd.Parameters.AddWithValue("@foot", player_txtFoot.Text);
				cmd.Parameters.AddWithValue("@countUpdate", countUpdated);

				cmd.ExecuteNonQuery();
			} catch (Exception ex) {
				MessageBox.Show("This player already exists!",
					   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				type = -9999;
			}

			if (type == 0)
				MessageBox.Show("Update info success!",
				   "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else if(type == 1)
			{
				MessageBox.Show("Add info success!",
				   "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
				canADD = 0;
				clearDataInput();
			}

			loadDataToTable();

			player_txtPhonenumber.Enabled = false;
		}

		private void player_btnUploadAvt_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)" +
				"|*.jpg; *.jpeg; *.gif; *.bmp";
			if (open.ShowDialog() == DialogResult.OK)
			{
				player_pbAvt.Image = new Bitmap(open.FileName);
				avtFilePath = open.FileName;
			}
		}

		private void player_dtgCoachList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			player_btnDel.Enabled = true;
			if (e.RowIndex >= 0)
			{
				DataGridViewRow row = this.player_dtgPlayerList.Rows[e.RowIndex];
				idCoach = row.Cells[0].Value.ToString();

				displayDataToTextBox();
			}
		}

		private void player_btnBackHome_Click(object sender, EventArgs e)
		{
			frmHome frm = new frmHome();
			frm.phoneNumber = player_phoneNumber;
			showAnotherForm(frm);
		}

		private void player_btnAdd_Click_1(object sender, EventArgs e)
		{
			player_txtPhonenumber.Enabled = true;
			player_txtPhonenumber.Focus();
			clearDataInput();
			canADD = 1;
		}

		private void player_btnDel_Click_1(object sender, EventArgs e)
		{
			cmd = new SqlCommand("delete from  player where phoneNumber=@idCoach", conn);
			cmd.Parameters.AddWithValue("@idCoach", idCoach);
			cmd.ExecuteNonQuery();
			conn.Close();
			MessageBox.Show("Deleted data successfully!");

			loadDataToTable();
			clearDataInput();

			player_txtPhonenumber.Enabled = false;
			player_btnDel.Enabled = false;
		}
	}
}

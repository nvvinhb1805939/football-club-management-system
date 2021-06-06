using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballClubManagementSystem
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;

        /*isDigit regular expression*/
        public bool isDigit(String str) {
            bool result = false;
            Regex regex = new Regex(@"^[0-9]+$");
            if (regex.IsMatch(str))
                result = true;
            return result;
        }

        /*insert account data into db*/
        public bool insertDataIntoDB(string phoneNumber, string pw) {
            bool result = false;
            string sql = "select * from club " +
                "where phoneNumber='" + phoneNumber + "'";
            cmd = new SqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close();
                MessageBox.Show("Username Already exist!" +
                    "\nPlease try another!", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dr.Close();
                sql = "insert into club(phoneNumber, password) " +
                    "values(@phoneNumber, @pw)";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("phoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("pw", pw);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Your account is created. \nPlease login now!",
                    "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                result = true;
            }
            return result;
        }
        private void register_btnSubmit_Click(object sender, EventArgs e)
        {
            String phoneNumber = register_txtPhoneNumber.Text.Trim(),
                pw = register_txtPw.Text.Trim(),
                confirmPw = register_txtConfirmPw.Text.Trim();
            /*check valid input data*/
            if (!isDigit(phoneNumber) || phoneNumber.Length == 0
                    || phoneNumber.Length > 15)
            {
                MessageBox.Show("Phone number must be " +
                    "digit less than or equal to 15 characters" +
                    " and not empty!", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                register_txtPhoneNumber.Focus();
                return;
            }
            if (pw.Length == 0 || pw.Length > 15)
            {
                MessageBox.Show("Password must be less than" +
                    " or equal to 15 characters and not empty!",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                register_txtPw.Focus();
                return;
            }
            if (!confirmPw.Equals(pw))
            {
                MessageBox.Show("Incorrect password!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                register_txtConfirmPw.Focus();
                return;
            }

            bool result = insertDataIntoDB(phoneNumber, pw);
            if(result)
                this.Close();
            
        }

		private void frmRegister_Load(object sender, EventArgs e)
		{
            conn = new SqlConnection(@"Data Source=DESKTOP-L0U2IJ8\SQLEXPRESS;
                Initial Catalog=FootballClubManagementDB;
                Persist Security Info=True;User ID=sa;
                Password=sa");
            conn.Open();
		}
	}
}

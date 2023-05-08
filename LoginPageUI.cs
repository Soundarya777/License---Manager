using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LicenseManager
{
    /// <summary>
    /// Login form
    /// </summary>
    public partial class LoginPageUI : Form
    {
        public LoginPageUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Login button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = txtbxUser.Text;
            string password = txtbxPassword.Text;


            if (username == "wcadmin" && password == "wcadmin")
            {
                this.Hide();
                LicenceManagementUI licenceManageUI = new LicenceManagementUI();
                licenceManageUI.Show();

            }
            else if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                MessageBox.Show("Please enter username and password", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show("Incorrect username or password", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

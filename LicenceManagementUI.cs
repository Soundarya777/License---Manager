using System;
using System.IO;
using System.Windows.Forms;

namespace LicenseManager
{
    public partial class LicenceManagementUI : Form
    {
        public LicenceManagementUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// License click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bttnlicenceManage_Click(object sender, EventArgs e)
        {
            ManageLicenceUI manageLicenceUI = new ManageLicenceUI();
            if(File.Exists(manageLicenceUI.filePath) && File.Exists(manageLicenceUI.keyPath))
            manageLicenceUI.ShowDialog();
            else
                MessageBox.Show("licence.dat and key file does not exists in the path", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void bttnLicencehistory_Click_1(object sender, EventArgs e)
        {
            LicenceHistoryUI licenceHistoryUI = new LicenceHistoryUI();
            licenceHistoryUI.ShowDialog();
        }

        private void LicenceManagementUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            int processId = System.Diagnostics.Process.GetCurrentProcess().Id;
            System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(processId);
            if (process != null)
                process.Kill();
        }
    }
}

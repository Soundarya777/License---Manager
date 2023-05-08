using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace LicenseManager
{
    /// <summary>
    /// License Management UI
    /// </summary>
    public partial class ManageLicenceUI : Form
    {
        // private string filepath = @"D:\WGM Files\licence.dat";
        // private string keyFilepath = @"D:\WGM Files\key";

        private string key = "SH2sbUS7r/pFvW2Z0LhzRA==";
        public string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "licence.dat");
        public string keyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "key");

        public ManageLicenceUI()
        {
            InitializeComponent();
        }

        private void ManageLicenceUI_Load(object sender, EventArgs e)
        {
            UpdateDatagridview();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Remove")
            {
                if (MessageBox.Show("Are you sure want to delete this record ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dataGridView2.Rows.Remove(dataGridView2.Rows[e.RowIndex]);
                    UpdateLicenceFile();
                }
            }
        }

        private void UpdateLicenceFile()
        {
            try
            {
                FileInfos content = new FileInfos();
                content.users = new List<User>();

                List<DataGridViewRow> rowCollection = new List<DataGridViewRow>();

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    User user = new User();
                    user.userName = row.Cells[1].Value.ToString();
                    user.lastLoggedIn = row.Cells[2].Value.ToString();
                    content.users.Add(user);
                }
                content.count = content.users.Count;
                ManageLicence.Encrypt(key, content, filePath);
                UpdateDatagridview();
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateDatagridview()
        {
            try
            {
                dataGridView2.Rows.Clear();
                dataGridView2.Refresh();

                FileInfos content = new FileInfos();
                content = ManageLicence.Decrypt(key, filePath);

                DataGridViewButtonColumn btnRemove = new DataGridViewButtonColumn();
                btnRemove.Text = "Remove";
                btnRemove.HeaderText = "Remove";
                btnRemove.Name = "Remove";
                btnRemove.UseColumnTextForButtonValue = true;
                btnRemove.Width = 100;


                dataGridView2.ColumnCount = 3;
                dataGridView2.Columns[0].Name = "S.No";
                dataGridView2.Columns[1].Name = "User Name";
                dataGridView2.Columns[2].Name = "Last Login Time";
                DataGridViewColumn snoColumn = dataGridView2.Columns[0];
                snoColumn.Width = 50;
                DataGridViewColumn loginColumn = dataGridView2.Columns[2];
                loginColumn.Width = 200;


                int i = 1;
                foreach (var item in content.users)
                {
                    dataGridView2.Rows.Add(i, item.userName, item.lastLoggedIn);
                    i++;
                }
                dataGridView2.Columns.Add(btnRemove);

                KeyFileInfo keyInfo = new KeyFileInfo();
                keyInfo = ManageLicence.DecryptKey(key, keyPath);

                lbMaxUser.Text = keyInfo.count.ToString();
                lbLicenceValid.Text = keyInfo.validity.ToString();
                lbActiveUser.Text = content.count.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateDatagridview();
        }
    }
}

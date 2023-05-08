using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace LicenseManager
{
    public partial class LicenceHistoryUI : Form
    {
        private LicenceHistory licenceHistory = new LicenceHistory();
        private List<Tuple<string, string, string>> fileData = LicenceHistoryData.ReadFile();

        public LicenceHistoryUI()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            LicenceHistory.status = comboBxStatus.SelectedItem = null;
            LicenceHistory.userName = comboBxUsername.SelectedItem = null;
            LicenceHistory.date = comboBxDate.SelectedItem = null;

            gboxStartdate.Visible = gboxEnddate.Visible = false;
            dateTimePicker1.Visible = dateTimePicker2.Visible = false;
        }

        private static byte[] DecryptFiles(string secKey, string fileName)
        {
            try
            {
                secKey = "SH2sbUS7r/pFvW2ZOLhzRA==";
                byte[] key = Encoding.ASCII.GetBytes(secKey);
                byte[] iv = new byte[16];
                string IV = "1234567891234567";
                for (int l = 0; l < 16; l++)
                {
                    iv[l] = Convert.ToByte(IV[l]);
                }
                int chunkSize = 50 * 1024 * 1024;
                fileName = @"licence.dat";
                string filefullName = @"D:\WGM Files\licence.dat";
                var sourceFile = new FileInfo(fileName);
                var buffer = new byte[chunkSize];

                using (var fileStream = File.Open(filefullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fileStream.Seek(chunkSize, SeekOrigin.Begin);
                    using (var binaryReader = new BinaryReader(fileStream))
                    {
                        binaryReader.Read(buffer, 0, buffer.Length);
                    }
                }

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (MemoryStream dataOut = new MemoryStream())
                    {
                        using (RijndaelManaged rijAlg = new RijndaelManaged())
                        {
                            rijAlg.Padding = PaddingMode.None;
                            ICryptoTransform decryptor = rijAlg.CreateDecryptor(key, iv);

                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                using (BinaryReader decryptedData = new BinaryReader(cryptoStream))
                                {
                                    byte[] buffer1 = new byte[chunkSize];
                                    int count;
                                    while ((count = decryptedData.Read(buffer1, 0, buffer1.Length)) != 0)
                                    {
                                        dataOut.Write(buffer1, 0, count);
                                    }
                                    return dataOut.ToArray();
                                }
                            }
                        }
                    }

                }
            }

            catch (Exception ex) { return null; }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            LicenceHistory.status = comboBxStatus.SelectedItem;
            LicenceHistory.userName = comboBxUsername.SelectedItem;
            LicenceHistory.date = comboBxDate.SelectedItem;
            LicenceHistory.startDate = dateTimePicker1.Value;
            LicenceHistory.endDate = dateTimePicker2.Value;
            
            List<object> comboBoxs = new List<object> { comboBxDate.SelectedItem, comboBxStatus.SelectedItem, comboBxUsername.SelectedItem };

            if (comboBoxs.All(v => v == null))
                MessageBox.Show("Please choose any value");
            else
                Selection();
        }

        /// <summary>
        /// On History Selection method
        /// </summary>
        private void Selection()
        {
            try
            {
                LicenceHistoryData licenceHistoryData = new LicenceHistoryData();
                if (comboBxStatus.SelectedItem != null)
                {
                    if (comboBxStatus.SelectedItem.ToString().Equals("Success", StringComparison.InvariantCultureIgnoreCase))
                        LicenceHistory.status = "LicenceValid";
                    else if (comboBxStatus.SelectedItem.ToString().Equals("Failed", StringComparison.InvariantCultureIgnoreCase))
                        LicenceHistory.status = "LicenceNotValid";
                    
                }
                else
                    LicenceHistory.status = string.Empty;

                List<Tuple<string, DateTime, string>> fileDatas = licenceHistoryData.DataFilter();
                if(fileDatas.Count!=0)
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("S.No", typeof(string));
                    table.Columns.Add("User Name", typeof(string));
                    table.Columns.Add("Last Login Time", typeof(DateTime));
                    table.Columns.Add("Status", typeof(string));


                    int i = 1;
                    foreach (var item in fileDatas)
                    {
                        table.Rows.Add(i, item.Item1, item.Item2, item.Item3);
                        i++;
                    }

                    dataGridView1.DataSource = table;
                }
                else
                {
                    if (dataGridView1.Rows.Count != 0)
                        dataGridView1.DataSource = null;

                    dataGridView1.Refresh();
                    MessageBox.Show("No records found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void comboBxUsername_Click(object sender, EventArgs e)
        {
            List<string> userName = new List<string>();

            foreach (var item in fileData)
            {
                if (!userName.Contains(item.Item1))
                    userName.Add(item.Item1);
            }

            if (userName.Contains(string.Empty))
                userName.Remove(string.Empty);

            foreach (var item in userName)
            {
                if (!comboBxUsername.Items.Contains(item))
                    comboBxUsername.Items.Add(item);
            }
        }

        private void comboBxDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LicenceHistory.date = comboBxDate.SelectedItem;

            if (LicenceHistory.date.ToString() == "Period")
            {
                LicenceHistory.startDate = dateTimePicker1.Value;
                LicenceHistory.endDate = dateTimePicker2.Value;
                gboxStartdate.Visible = gboxEnddate.Visible = true;
                dateTimePicker1.Visible = dateTimePicker2.Visible = true;
            }
            else
            {
                gboxStartdate.Visible = gboxEnddate.Visible = false;
                dateTimePicker1.Visible = dateTimePicker2.Visible = false;
            }
        }
    }
}

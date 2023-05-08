using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace LicenseManager
{
    internal class ManageLicence
    {
        /// Sample data
        //private string filepath = @"D:\Workshop\EncryptDecrypt\licence1.dat";
        //private string key = "SH2sbUS7r/pFvW2Z0LhzRA==";
        //private User user1 = new User { userName = "SSTRevitWGM", lastLoggedIn = "2018-12-12 12:25:08+0530" };
        //private User user2 = new User { userName = "vignesh", lastLoggedIn = "2021-07-15 13:25:43+0000" };


        public static KeyFileInfo DecryptKey(string keyValue, string filepath)
        {
            KeyFileInfo content = new KeyFileInfo();
            try
            {
                byte[] plain = File.ReadAllBytes(filepath);
                AesManaged tdes = new AesManaged();
                tdes.Key = Convert.FromBase64String(keyValue);
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decrypt = tdes.CreateDecryptor();
                byte[] cipher = decrypt.TransformFinalBlock(plain, 0, plain.Length);
                String encryptedText = Encoding.UTF8.GetString(cipher);
                content.keyData = encryptedText.Split('|');
                content.count = Convert.ToInt32(content.keyData[0]);

                string dateString;
                dateString = DateTime.ParseExact(content.keyData[1], "yyyy-MM-dd HH:mm:ss'+0000'", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy HH:mm:ss");

                DateTime dateTime = DateTime.ParseExact(dateString, "dd-MM-yyyy HH:mm:ss", null);

                content.validity = dateTime;
            }
            catch (Exception ex)
            {

            }
            return content;
        }

        public static bool EncryptKey(string keyValue, KeyFileInfo content, string fileToWrite)
        {
            bool isEncrypted = false;
            try
            {
                AesManaged tdes = new AesManaged();
                tdes.Key = Convert.FromBase64String(keyValue);
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                string cont = JsonConvert.SerializeObject(content);
                byte[] plain = Encoding.UTF8.GetBytes(cont);
                ICryptoTransform decrypt = tdes.CreateEncryptor();
                byte[] cipher = decrypt.TransformFinalBlock(plain, 0, plain.Length);
                File.WriteAllBytes(fileToWrite, cipher);
                isEncrypted = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isEncrypted;
        }

        /// <summary>
        /// To endecrypt & retrieve the contents
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>

        public static FileInfos Decrypt(string keyValue, string filepath)
        {
            FileInfos content = new FileInfos();
            try
            {
                byte[] plain = File.ReadAllBytes(filepath);
                AesManaged tdes = new AesManaged();
                tdes.Key = Convert.FromBase64String(keyValue);
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decrypt = tdes.CreateDecryptor();
                byte[] cipher = decrypt.TransformFinalBlock(plain, 0, plain.Length);
                String encryptedText = Encoding.UTF8.GetString(cipher);
                content = JsonConvert.DeserializeObject<FileInfos>(encryptedText);
            }
            catch (Exception ex)
            {
                
            }
            return content;
        }
        /// <summary>
        /// To encrypt the content in a file
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="content"></param>
        /// <param name="fileToWrite"></param>
        /// <returns></returns>
        public static bool Encrypt(string keyValue, FileInfos content, string fileToWrite)
        {
            bool isEncrypted = false;
            try
            {
                AesManaged tdes = new AesManaged();
                tdes.Key = Convert.FromBase64String(keyValue);
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                string cont = JsonConvert.SerializeObject(content);
                byte[] plain = Encoding.UTF8.GetBytes(cont);
                ICryptoTransform decrypt = tdes.CreateEncryptor();
                byte[] cipher = decrypt.TransformFinalBlock(plain, 0, plain.Length);
                File.WriteAllBytes(fileToWrite, cipher);
                isEncrypted = true;
            }
            catch (Exception ex)
            {
                
            }
            return isEncrypted;
        }
    }

    [Serializable]
    public class User
    {
        public  string userName { get; set; }
        public  string lastLoggedIn { get; set; }
    }

    [Serializable]
    public class FileInfos
    {
        public int count { get; set; }
        public List<User> users { get; set; }
    }

    [Serializable]
    public class KeyFileInfo
    {
        public int count { get; set; }
        public DateTime validity { get; set; }
        public string[] keyData { get; set; }
    }
}

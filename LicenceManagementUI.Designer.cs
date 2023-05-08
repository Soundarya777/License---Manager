namespace LicenseManager
{
    partial class LicenceManagementUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bttnlicenceManage = new System.Windows.Forms.Button();
            this.bttnLicencehistory = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bttnlicenceManage
            // 
            this.bttnlicenceManage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttnlicenceManage.Location = new System.Drawing.Point(37, 28);
            this.bttnlicenceManage.Name = "bttnlicenceManage";
            this.bttnlicenceManage.Size = new System.Drawing.Size(213, 57);
            this.bttnlicenceManage.TabIndex = 2;
            this.bttnlicenceManage.Text = "Manage Licence";
            this.bttnlicenceManage.UseVisualStyleBackColor = true;
            this.bttnlicenceManage.Click += new System.EventHandler(this.bttnlicenceManage_Click);
            // 
            // bttnLicencehistory
            // 
            this.bttnLicencehistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttnLicencehistory.Location = new System.Drawing.Point(37, 118);
            this.bttnLicencehistory.Name = "bttnLicencehistory";
            this.bttnLicencehistory.Size = new System.Drawing.Size(213, 57);
            this.bttnLicencehistory.TabIndex = 3;
            this.bttnLicencehistory.Text = "Licence History";
            this.bttnLicencehistory.UseVisualStyleBackColor = true;
            this.bttnLicencehistory.Click += new System.EventHandler(this.bttnLicencehistory_Click_1);
            // 
            // LicenceManagementUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 209);
            this.Controls.Add(this.bttnLicencehistory);
            this.Controls.Add(this.bttnlicenceManage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LicenceManagementUI";
            this.ShowIcon = false;
            this.Text = "Licence Management";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LicenceManagementUI_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button bttnlicenceManage;
        private System.Windows.Forms.Button bttnLicencehistory;
    }
}
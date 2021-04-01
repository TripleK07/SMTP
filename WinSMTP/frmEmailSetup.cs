using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinSMTP
{
    public partial class frmEmailSetup : Form
    {
        public frmEmailSetup()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                txtAttachPath.Text = folderBrowser.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsHelper.sender = txtFrom.Text;
                clsHelper.senderPassword = txtPassword.Text;
                clsHelper.recipients = txtTo.Text;
                clsHelper.subject = txtSubject.Text;
                clsHelper.body = txtBody.Text;
                clsHelper.attachmentPath = txtAttachPath.Text;

                clsHelper.writeConfigVariables();
                MessageBox.Show("Save successfully");

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                clsHelper.WriteErrorLog(ex);
                MessageBox.Show("Save failed");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmEmailSetup_Load(object sender, EventArgs e)
        {
            clsHelper.readConfigVariables();

            txtFrom.Text = clsHelper.sender;
            txtPassword.Text = clsHelper.senderPassword;
            txtTo.Text = clsHelper.recipients;
            txtSubject.Text = clsHelper.subject;
            txtBody.Text = clsHelper.body;
            txtAttachPath.Text = clsHelper.attachmentPath;
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
    }
}

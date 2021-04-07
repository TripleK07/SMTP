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
    public partial class frmExportAndSend : Form
    {
        public frmExportAndSend()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                List<String> files = clsHelper.getFilesFromAttachmentPath();
                clsSMTP smtp = new clsSMTP();
                smtp.sendMail(files);
            }
            catch(Exception ex)
            {
                clsHelper.WriteErrorLog(ex);
            }
        }
    }
}

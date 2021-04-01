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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void emailSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmailSetup frm = new frmEmailSetup();
            frm.ShowDialog();
        }
    }
}

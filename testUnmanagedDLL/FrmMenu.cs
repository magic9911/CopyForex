using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CopyForex {
    public partial class FrmMenu : Form {
        public FrmMenu() {
            InitializeComponent();
        }

        private void btnMaster_Click(object sender, EventArgs e) {
            // new instance form
            FrmServer serverForm = new FrmServer();

            // Set instance to Main DLL
            CopyDll.Master = serverForm;
            serverForm.Show();
        }

        private void btnSlave_Click(object sender, EventArgs e) {
            // new instance ctrl & form
            CopyDll.SlaveController = new Client.MsgController();
            FrmSlave slaveForm = new FrmSlave(CopyDll.SlaveController);

            // Set instance to Main DLL
            CopyDll.Slave = slaveForm;
            slaveForm.Show();
        }
    }
}

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
            
        }

        private void btnSlave_Click(object sender, EventArgs e) {
            new FrmSlave().Show();
        }
    }
}

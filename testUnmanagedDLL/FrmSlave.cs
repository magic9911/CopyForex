using CopyForex.Client;
using Nore.CommonLib.Message;
using Norr.CommonLib.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace CopyForex {
    public partial class FrmSlave : Form, IMsgView {
        private MsgController msgController;

        public FrmSlave() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            
            
            //msgController.SendMessageToRoom(new MessageData() {
            //    MessageText = "HELLO WORLD"
            //});
            //clientSocket.Connect("127.0.0.1", 9000);
            //label1.Text = "Client Socket Program - Server Connected ...";
        }

        public void msg(string msg) {
            textBox1.Text += "\r\n >> " + msg;
        }

        private void btnConnect_Click(object sender, EventArgs e) {
            msg("Client connecting...");
            msgController = new MsgController();
            msgController.FormView = this;
            msgController.UserInfo = new UserInfo() {
                Nick = "Test" + new Random().Next()
            };
            msgController.Connect();
        }

        private void chkHostSlave_CheckedChanged(object sender, EventArgs e) {
            var chkBox = sender as CheckBox;
            if (chkBox.Checked) {
                chkBox.Text = "Host";
            } else {
                chkBox.Text = "Slave";
            }
        }

        public void SendAll(string msg) {
            if (null != msgController) {
                MessageData msgData = new MessageData();
                msgData.MessageText = msg;
                msgController.SendMessageToRoom(msgData);
            }
        }

        public void OnMessageReceived(string nick, MessageData message) {
            msg(string.Format("[HOST] {0} : {1}", nick, message.MessageText));
        }

        public void OnPrivateMessageReceived(string nick, MessageData message) {
            msg(string.Format("{0} : {1}", nick, message.MessageText));
        }

        public void OnLoggedIn() {
            msg("Logged In.");
        }

        public void OnLoginError(string errorMessage) {
            msg("Error : " + errorMessage);
        }

        public void OnLoggedOut() {
            msg("Logged Out.");
        }

        public void AddUserToList(UserInfo userInfo) {
            msg("User joined : " + userInfo.Nick);
        }

        public void RemoveUserFromList(string nick) {
            msg("User quited : " + nick);
        }
    }
}

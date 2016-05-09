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
using System.Collections.Concurrent;

namespace CopyForex {
    public partial class FrmSlave : Form, IMsgView {
        private MsgController msgController;

        public FrmSlave() {
            InitializeComponent();
        }

        public FrmSlave(MsgController msgCtrl) : this() {
            msgController = msgCtrl;
        }

        private void Form1_Load(object sender, EventArgs e) {
            print("Client connecting...");

            if (null == msgController)
                msgController = new MsgController();

            msgController.FormView = this;
            msgController.UserInfo = new UserInfo() {
                Nick = "Test" + new Random().Next()
            };
            msgController.Connect();
        }

        public void print(string msg) {
            if (InvokeRequired) {
                Invoke(new Action(() => print(msg)));
                return;
            }
            textBox1.Text += "\r\n >> " + msg;
            textBox1.SelectionStart = textBox1.TextLength;
            textBox1.ScrollToCaret();
        }

        private void btnConnect_Click(object sender, EventArgs e) {
            print("Client connecting...");
            
            if (null == msgController)
                msgController = new MsgController();

            msgController.FormView = this;
            msgController.UserInfo = new UserInfo() {
                Nick = "Test" + new Random().Next()
            };
            msgController.Connect();
        }

        public void SendAll(string msg) {
            if (null != msgController) {
                MessageData msgData = new MessageData();
                msgData.MessageText = msg;
                msgController.SendMessageToRoom(msgData);
            }
        }

        public void OnMessageReceived(string nick, MessageData message) {
            print(string.Format("[HOST] {0} : {1}", nick, message.MessageText));
        }

        public void OnPrivateMessageReceived(string nick, MessageData message) {
            print(string.Format("{0} : {1}", nick, message.MessageText));
        }

        public void OnLoggedIn() {
            print("Logged In.");
        }

        public void OnLoginError(string errorMessage) {
            print("Error : " + errorMessage);
        }

        public void OnLoggedOut() {
            print("Logged Out.");
        }

        public void AddUserToList(UserInfo userInfo) {
            print("User joined : " + userInfo.Nick);
        }

        public void RemoveUserFromList(string nick) {
            print("User quited : " + nick);
        }

        private void btnSend_Click(object sender, EventArgs e) {
            SendAll("Hello from Slave");
        }

        public void OnOrderReceived(OrderData order) {
            print("In come order : " + order);

            // Check if null will exit.
            if (null == order) {
                print("Order = null !");
                return;
            }

            // Init object.
            ConcurrentDictionary<string, OrderData> slaveOrders = null;
            if (null == msgController.GetSlaveOrders()) {
                // Init on controller.
                msgController.InitSlaveOrders();

                // Get instance of Slave order again.
                slaveOrders = msgController.GetSlaveOrders();
            }

            // Get order ID.
            var orderId = order.OrderId;

            // Check if order ID existed.
            if (slaveOrders.ContainsKey(orderId)) {
                print("Order " + orderId + " existed.");

                // Checking order status.
                if (order.Status == StatusType.Running) {
                    // Replace order data to key.
                    slaveOrders[orderId] = order;
                } else {
                    // Removing order data.
                    OrderData removeOrder = null; // Init temp holder.
                    slaveOrders.TryRemove(orderId, out removeOrder); // Removing order data from order ID.
                    print("Removed order : " + removeOrder.OrderId);
                    removeOrder = null; // deassign.
                }
            } else {
                // Add new order data
                slaveOrders.TryAdd(orderId, order);
                print("Added order : " + orderId);
            }

        }
    }
}

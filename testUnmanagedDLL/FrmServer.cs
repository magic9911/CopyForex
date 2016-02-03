using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CopyForex.Client;
using CopyForex.Server;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using Nore.CommonLib.Message;
using Norr.CommonLib.Client;
using Norr.CommonLib.Service;
using YuriNET.Common;

namespace CopyForex {
    public partial class FrmServer : Form, IMsgView {

        private MsgController masterController = null;


        public FrmServer() {
            InitializeComponent();
            NativeMethods.AllocConsole();
        }
        
        /// <summary>
        /// This object is used to host Chat Service on a SCS server.
        /// </summary>
        private static IScsServiceApplication _serviceApplication;

        /// <summary>
        /// Chat Service object that serves clients.
        /// </summary>
        private static MsgService _msgService;

        private static void startServer() {
            Console.WriteLine("Starting server...");
            int port;
            try {
                port = 9000; // Hard code test
                if (port <= 0 || port > 65536) {
                    throw new Exception(port + " is not a valid TCP port number.");
                }
            } catch (Exception ex) {
                Console.WriteLine("TCP port must be a positive number. Exception detail: " + ex.Message);
                return;
            }

            try {
                Console.WriteLine("Listening on " + port);
                _serviceApplication = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(port));
                _msgService = new MsgService();
                _serviceApplication.AddService<IMsgService, MsgService>(_msgService);
                _msgService.UserListChanged += msgService_UserListChanged;
                _serviceApplication.Start();

                Console.WriteLine("Started.");
            } catch (Exception ex) {
                Console.WriteLine("Service can not be started. Exception detail: " + ex.Message);
                return;
            }

        }

        private static void stopServer() {
            if (_serviceApplication != null) {
                _serviceApplication.Stop();
                Console.WriteLine("Stopped.");
            }
        }

        public void print(string msg) {
            if (InvokeRequired) {
                Invoke(new Action(() => print(msg)));
                return;
            }

            Console.WriteLine(msg);
            txtLog.Text += "\r\n >> " + msg;
            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.ScrollToCaret();
        }

        private static void msgService_UserListChanged(object sender, EventArgs e) {
            updateUserList();
        }

        /// <summary>
        /// Updates user list on GUI.
        /// </summary>
        private static void updateUserList() {
            var users = new StringBuilder();
            foreach (var user in _msgService.UserList) {
                if (users.Length > 0) {
                    users.Append(", ");
                }

                users.Append(user.Nick);
            }
            Console.WriteLine(users.ToString());
        }

        private void btnStart_Click(object sender, EventArgs e) {
            startServer();

            masterController = new MsgController();
            masterController.FormView = this;
            masterController.UserInfo = new UserInfo("Master", true);
            masterController.Connect();
        }

        private void btnStop_Click(object sender, EventArgs e) {
            stopServer();
        }

        private void btnTestSend_Click(object sender, EventArgs e) {
            //masterController.SendMessageToRoom(new MessageData("Hello All"));

            // test send order
            masterController.SendOrder(new OrderData("1", "EURUSD", 1.5, OrderType.Buy.ToString(), 1.111, 1.2, 1.8, "running"));
        }

        /// <summary>
        /// Send order from DLL EXPORT
        /// </summary>
        /// <param name="order"></param>
        public void SendOrder(OrderData order) {
            print("Send : " + order);
            masterController.SendOrder(order);
        }

        // IMsgView
        public void OnMessageReceived(string nick, MessageData message) {
            print("[" + nick + "] : " + message.MessageText);
        }

        public void OnPrivateMessageReceived(string nick, MessageData message) {
            
        }

        public void OnLoggedIn() {
            MessageBox.Show("Master logged in.");
        }

        public void OnLoginError(string errorMessage) {
            
        }

        public void OnLoggedOut() {
            
        }

        public void AddUserToList(UserInfo userInfo) {
            
        }

        public void RemoveUserFromList(string nick) {
            
        }

        public void OnOrderReceived(OrderData order) {
            print("Order sent, " + order);
        }
    }
}

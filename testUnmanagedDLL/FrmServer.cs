using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CopyForex.Server;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using Nore.CommonLib.Message;
using Norr.CommonLib.Service;

namespace CopyForex {
    public partial class FrmServer : Form {
        public FrmServer() {
            InitializeComponent();
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
        }

        private void btnStop_Click(object sender, EventArgs e) {
            stopServer();
        }

        private void btnTestSend_Click(object sender, EventArgs e) {
            _msgService.SendMessageToRoom(new MessageData("Hello All"));
        }
    }
}

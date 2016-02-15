using Hik.Communication.Scs.Communication;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using Hik.Communication.ScsServices.Communication.Messages;
using Nore.CommonLib.Message;
using Norr.CommonLib.Client;
using Norr.CommonLib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace CopyForex.Client {
    /// <summary>
    /// This class is a mediator with view and SCS system.
    /// </summary>
    internal class MsgController : IMsgController {
        #region Private fields

        /// <summary>
        /// Reference to login form.
        /// </summary>
        //public ILoginFormView LoginForm { get; set; }

        /// <summary>
        /// Reference to chat room window.
        /// </summary>
        public IMsgView FormView { get; set; }

        /// <summary>
        /// The object which handles remote method calls from server.
        /// It implements IChatClient contract.
        /// </summary>
        private ChatClient _chatClient;

        /// <summary>
        /// This object is used to connect to SCS Chat Service as a client. 
        /// </summary>
        private IScsServiceClient<IMsgService> _scsClient;

        /// <summary>
        /// The object which store own information.
        /// </summary>
        public UserInfo UserInfo;

        /// <summary>
        /// A Key value pair of <see cref="OrderData"/>.
        /// </summary>
        public ConcurrentDictionary<string, OrderData> slaveOrders;

        #endregion

        #region IChatController implementation

        /// <summary>
        /// Connects to the server.
        /// It automatically Logins to server if connection success.
        /// </summary>
        public void Connect() {
            //Disconnect if currently connected
            Disconnect();

            //Create a ChatClient to handle remote method invocations by server
            _chatClient = new ChatClient(FormView);

            //Create a SCS client to connect to SCS server
            _scsClient = ScsServiceClientBuilder.CreateClient<IMsgService>(new ScsTcpEndPoint("127.0.0.1", 9000), _chatClient);

            
            //Register events of SCS client
            _scsClient.Connected += ScsClient_Connected;
            _scsClient.Disconnected += ScsClient_Disconnected;

            //Connect to the server
            _scsClient.Connect();
        }

        /// <summary>
        /// Disconnects from server if it is connected.
        /// </summary>
        public void Disconnect() {
            if (_scsClient != null && _scsClient.CommunicationState == CommunicationStates.Connected) {
                try {
                    _scsClient.Disconnect();
                } catch {

                }

                _scsClient = null;
            }
        }

        /// <summary>
        /// Sends a public message to room.
        /// It will be seen by all users in room.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        public void SendMessageToRoom(MessageData message) {
            _scsClient.ServiceProxy.SendMessageToRoom(message);
        }

        /// <summary>
        /// Change status of user.
        /// </summary>
        /// <param name="newStatus">New status</param>
        //public void ChangeStatus(UserStatus newStatus) {
        //    _scsClient.ServiceProxy.ChangeStatus(newStatus);
        //}

        /// <summary>
        /// Sends a private message to a user.
        /// </summary>
        /// <param name="nick">Destination nick</param>
        /// <param name="message">Message</param>
        public void SendPrivateMessage(string nick, MessageData message) {
            _scsClient.ServiceProxy.SendPrivateMessage(nick, message);
        }

        /// <summary>
        /// Initialize Slave Orders.
        /// </summary>
        public void InitSlaveOrders() {
            slaveOrders = new ConcurrentDictionary<string, OrderData>();
        }

        /// <summary>
        /// Get instance of Slave Orders.
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, OrderData> GetSlaveOrders() {
            return slaveOrders;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method handles Connected event of _scsClient.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void ScsClient_Connected(object sender, EventArgs e) {
            try {
                _scsClient.ServiceProxy.Login(UserInfo);
                FormView.OnLoggedIn();

                _scsClient.ServiceProxy.SendMessageToRoom(new MessageData() { MessageText = "Hello World" });
                Console.WriteLine("Client connected.");
            } catch (ScsRemoteException ex) {
                Disconnect();
                //ChatRoom.OnLoginError(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                Console.WriteLine("Client " + ex);
            } catch (Exception ex) {
                Disconnect();
                //ChatRoom.OnLoginError("Can not login to server. Please try again later.");
                Console.WriteLine("Client Disconnected. " + ex);
            }
        }

        /// <summary>
        /// This method handles Disconnected event of _scsClient.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void ScsClient_Disconnected(object sender, EventArgs e) {
            //ChatRoom.OnLoggedOut();
            Console.WriteLine("Client logged out.");
        }

        public void SendOrder(OrderData order) {
            _scsClient.ServiceProxy.SendOrder(order);
        }

        #endregion
    }
}

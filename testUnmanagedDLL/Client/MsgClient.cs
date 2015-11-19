using CopyForex.Client;
using Nore.CommonLib.Message;
using Nore.CommonLib.Service;
using Norr.CommonLib.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CopyForex {
    /// <summary>
    /// This class implements IChatClient to use to be invoked by Chat Server.
    /// </summary>
    internal class ChatClient : IMsgClient {
        #region Private fields

        /// <summary>
        /// Reference to Chat Room window.
        /// </summary>
        private readonly IMsgView _chatRoom;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new ChatClient.
        /// </summary>
        /// <param name="chatRoom">Reference to Chat Room window</param>
        public ChatClient(IMsgView chatRoom) {
            _chatRoom = chatRoom;
        }

        #endregion

        #region IChatClient implementation

        /// <summary>
        /// This method is used to get user list from chat server.
        /// It is called by server, once after user logged in to server.
        /// </summary>
        /// <param name="users">All online user informations</param>
        public void GetUserList(UserInfo[] users) {
            foreach (var user in users) {
                _chatRoom.AddUserToList(user);
            }
        }

        /// <summary>
        /// This method is called from chat server to inform that a message
        /// is sent to chat room publicly.
        /// </summary>
        /// <param name="nick">Nick of sender</param>
        /// <param name="message">Message text</param>
        public void OnMessageToRoom(string nick, MessageData message) {
            _chatRoom.OnMessageReceived(nick, message);
        }

        /// <summary>
        /// This method is called from chat server to inform that a message
        /// is sent to the current used privately.
        /// </summary>
        /// <param name="nick">Nick of sender</param>
        /// <param name="message">Message</param>
        public void OnPrivateMessage(string nick, MessageData message) {
            _chatRoom.OnPrivateMessageReceived(nick, message);
        }

        /// <summary>
        /// This method is called from chat server to inform that a new user
        /// joined to chat room.
        /// </summary>
        /// <param name="userInfo">Informations of new user</param>
        public void OnUserLogin(UserInfo userInfo) {
            _chatRoom.AddUserToList(userInfo);
        }

        /// <summary>
        /// This method is called from chat server to inform that an existing user
        /// has left the chat room.
        /// </summary>
        /// <param name="nick">Informations of new user</param>
        public void OnUserLogout(string nick) {
            _chatRoom.RemoveUserFromList(nick);
        }

        #endregion
    }
}
using Hik.Communication.ScsServices.Service;
using Nore.CommonLib.Message;
using Norr.CommonLib.Client;

namespace Norr.CommonLib.Service {
    /// <summary>
    /// This interface defines Chat Service Contract.
    /// It is used by Chat clients to interact with Chat Server.
    /// </summary>
    [ScsService(Version = "1.0.0.0")]
    public interface IMsgService {
        /// <summary>
        /// Used to login to chat service.
        /// </summary>
        /// <param name="userInfo">User informations</param>
        void Login(UserInfo userInfo);

        /// <summary>
        /// Sends a public message to room.
        /// It will be seen by all users in room.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        void SendMessageToRoom(MessageData message);

        /// <summary>
        /// Sends a private message to a specific user.
        /// Message will be seen only by destination user.
        /// </summary>
        /// <param name="destinationNick">Nick of the destination user
        /// who will receive the message</param>
        /// <param name="message">Message to be sent</param>
        void SendPrivateMessage(string destinationNick, MessageData message);

        /// <summary>
        /// Used to logout from chat service.
        /// Client may not call this method while logging out (in an application crash situation),
        /// it will also be logged out automatically when connection fails between
        /// client and server.
        /// </summary>
        void Logout();


        void SendOrder(OrderData order);
    }
}

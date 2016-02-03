using System;

namespace Norr.CommonLib.Client
{
    /// <summary>
    /// Represents a chat user.
    /// This object particularly used in Login of a user.
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        /// <summary>
        /// Nick of user.
        /// </summary>
        public string Nick { get; set; }


        public bool IsMaster { get; set; }
        

        public UserInfo() {
        }

        public UserInfo(string nick) : this(nick, false) {
        }

        public UserInfo(string nick, bool isMaster) {
            Nick = nick;
            IsMaster = isMaster;
        }
    }
}

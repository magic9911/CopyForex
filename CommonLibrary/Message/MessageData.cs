using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nore.CommonLib.Message
{

    /// <summary>
    /// Message holder to send
    /// </summary>
    [Serializable]
    public class MessageData
    {

        /// <summary>
        /// Message text.
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Create a new Message data
        /// </summary>
        public MessageData() : this("") {
        }

        /// <summary>
        /// Create a new Message data
        /// </summary>
        /// <param name="message"></param>
        public MessageData(String message) {
            MessageText = message;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nore.CommonLib.Message {

    /// <summary>
    /// Message holder to send
    /// </summary>
    [Serializable]
    public class MessageData {

        /// <summary>
        /// Message text.
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Order item model
        /// </summary>
        public OrderData Order { get; set; }

        /// <summary>
        /// Create a new Message data
        /// </summary>
        public MessageData() : this("") {
        }

        /// <summary>
        /// Create a new Message data
        /// </summary>
        /// <param name="message"></param>
        public MessageData(String message) : this(message, null) {
        }

        /// <summary>
        /// Create a new Message data with Order item
        /// </summary>
        /// <param name="message"></param>
        /// <param name="order"></param>
        public MessageData(String message, OrderData order) {
            MessageText = message;
            Order = order;
        }

        /// <summary>
        /// Create a new Message data with Order item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="symbol"></param>
        /// <param name="lot"></param>
        /// <param name="orderType"></param>
        /// <param name="price"></param>
        /// <param name="sl"></param>
        /// <param name="tp"></param>
        /// <param name="status"></param>
        public MessageData(string id, string symbol, double lot, string orderType, double price,
            double sl, double tp, string status)
            : this("", new OrderData(id, symbol, lot, orderType, price, sl, tp, status)) {
        }

    }
}

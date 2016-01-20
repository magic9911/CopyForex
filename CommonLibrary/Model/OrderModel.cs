using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nore.CommonLib.Model {
    public class OrderModel {
        /// <summary>
        /// Order Id
        /// </summary>
        public string OrderId { get; private set; }

        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// Lot
        /// </summary>
        public double Lot { get; private set; }

        /// <summary>
        /// Type of order
        /// Buy or Sell
        /// </summary>
        public OrderType Type { get; private set; }

        /// <summary>
        /// Price
        /// </summary>
        public double Price { get; private set; }

        /// <summary>
        /// S / L
        /// </summary>
        public double SL { get; private set; }

        /// <summary>
        /// T / P
        /// </summary>
        public double TP { get; private set; }

        /// <summary>
        /// Status of Order
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        /// Constructor for Message Data model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="symbol"></param>
        /// <param name="lot"></param>
        /// <param name="method"></param>
        /// <param name="price"></param>
        /// <param name="sl"></param>
        /// <param name="tp"></param>
        /// <param name="status"></param>
        public OrderModel(string id, string symbol, double lot, string method, double price,
            double sl, double tp, string status) {
            OrderId = id;
            Symbol = symbol;
            Lot = lot;
            Type = (OrderType)Enum.Parse(typeof(OrderType), method);
            Price = price;
            SL = sl;
            TP = tp;
            Status = status;
        }

    }

    /// <summary>
    /// Enumularation of Order Type : BUY or SELL
    /// </summary>
    [Flags]
    public enum OrderType {
        Buy = 1,
        Sell = 2
    }
}
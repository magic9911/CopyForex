﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nore.CommonLib.Message {

    [Serializable]
    public class OrderData {
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
        public StatusType Status { get; private set; }

        /// <summary>
        /// Constructor for Order Data model.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="symbol"></param>
        /// <param name="lot"></param>
        /// <param name="orderType"></param>
        /// <param name="price"></param>
        /// <param name="sl"></param>
        /// <param name="tp"></param>
        /// <param name="status"></param>
        public OrderData(string id, string symbol, double lot, OrderType orderType, double price,
            double sl, double tp, StatusType status) {
            OrderId = id;
            Symbol = symbol;
            Lot = lot;
            Type = orderType;
            Price = price;
            SL = sl;
            TP = tp;
            Status = status;
        }

        public override string ToString() {
            return string.Format("Order Data : {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", OrderId, Symbol, Lot, Type, Price, SL, TP, Status);
        }

        public string ToRawData() {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", OrderId, Symbol, Lot, (int)Type, Price, SL, TP, (int)Status);
        }

    }

    /// <summary>
    /// Enumuration of Order Type.
    /// </summary>
    [Flags]
    public enum OrderType {
        Buy = 0,
        Sell = 1
    }

    /// <summary>
    /// Enumuration of Status Type.
    /// </summary>
    [Flags]
    public enum StatusType {
        Closed = 0,
        Running = 1
    }
}
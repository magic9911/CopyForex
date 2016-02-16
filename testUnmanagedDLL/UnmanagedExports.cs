using System.Text;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CopyForex;
using System.Threading;
using System;
using Nore.CommonLib.Message;

namespace CopyForex {
    public class CopyDll {
        public static FrmServer Master;
        public static FrmSlave Slave;

        public static Client.MsgController MasterController;
        public static Client.MsgController SlaveController;

        private static FrmMenu menu;
        private static Thread appThread;
        //private static SendData senddata = new SendData();

        public static string res = string.Empty;


        [DllExport("Init", CallingConvention = CallingConvention.StdCall)]
        public static void Init() {
            appThread = new Thread(new ThreadStart(OpenForm));
            appThread.Start();
        }

        public static void OpenForm() {
            menu = new FrmMenu();
            menu.Show();
            Application.Run();
        }

        [DllExport("Shutdown", CallingConvention = CallingConvention.StdCall)]
        public static void Shutdown() {
            /*
            var tmp = new Form();
            if (null != menu) {
                tmp.Invoke(new Action(() => {
                    menu.Close();
                    menu.Dispose();
                    menu = null;
                }));
            }

            if (null != Slave) {
                tmp.Invoke(new Action(() => {
                    Slave.Close();
                    Slave.Dispose();
                    Slave = null;
                }));
            }
            */

            try {
                appThread.Abort();
            } catch (Exception) { }

            Application.Exit();
            //AppDomain.Unload(AppDomain.CurrentDomain);
        }

        /// <summary>
        /// Send Order to all slaves by calling from Meta Trader
        /// </summary>
        /// <param name="ptrId"></param>
        /// <param name="ptrSymbol"></param>
        /// <param name="lot"></param>
        /// <param name="ptrOrderType"></param>
        /// <param name="price"></param>
        /// <param name="sl"></param>
        /// <param name="tp"></param>
        /// <param name="ptrStatus"></param>
        [DllExport("SendOrder", CallingConvention = CallingConvention.StdCall)]
        public static void SendOrder(IntPtr ptrId, IntPtr ptrSymbol, double lot, int orderType,
            double price, double sl, double tp, int status) {

            // Get data string from pointers
            string id = Marshal.PtrToStringAnsi(ptrId);
            string symbol = Marshal.PtrToStringAnsi(ptrSymbol);

            if (null != Master) {
                OrderData order = new OrderData(id, symbol, lot, (OrderType)orderType, price, sl, tp, (StatusType)status);
                Master.SendOrder(order);
            } else {
                Console.WriteLine("Master form is NULL !!!");
            }
        }

        [DllExport("GetOrder", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr GetOrder(string orderId) {
            string order;

            // Check slave form are initialized.
            if (null != Slave) {
                // Get instance of Slave orders.
                var slaveOrders = SlaveController.GetSlaveOrders();
                if (null == slaveOrders) {
                    SlaveController.InitSlaveOrders();
                    slaveOrders = SlaveController.GetSlaveOrders();
                }

                // Check order data is existed.
                if (slaveOrders.ContainsKey(orderId)) {
                    // Found.
                    order = slaveOrders[orderId].ToRawData();
                } else {
                    // Not found.
                    order = "OrderNotFound";
                }
            } else {
                // Slave is not initialized.
                order = "SlaveNotInitialized";
            }

            // Return pointer of string.
            return Marshal.StringToHGlobalUni(order);
        }

        [DllExport("Data_POST", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr Data_POST(double Balance, IntPtr charPos) {
            //string res = string.Empty;
            //if (null == oneForm)
            //    return Marshal.StringToHGlobalUni(res);

            var Pos = Marshal.PtrToStringAnsi(charPos);

            Slave.SendAll(Pos);
            //oneForm.Invoke(new Action(() => {
            //    res = oneForm.SendData("POST;" + Balance.ToString() + ";" + Pos);
            //}));

            //senddata.Send("POST;" + Balance.ToString() + ";" + Pos);
            return Marshal.StringToHGlobalUni(res);
        }

        [DllExport("Get_NewOrder", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr Get_NewOrder(IntPtr symbol) {
            //string res = string.Empty;
            //if (null == oneForm)
            //    return Marshal.StringToHGlobalUni(res);

            var Symbol = Marshal.PtrToStringAnsi(symbol);

            //oneForm.Invoke(new Action(() => {
            //    res = oneForm.SendData("NewOrder;" + Symbol);
            //}));

            //senddata.Send("NewOrder;" + Symbol);
            return Marshal.StringToHGlobalUni(res);
        }

        [DllExport("Get_CloseOrder", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr Get_CloseOrder(int id) {
            //string res = string.Empty;
            //if (null == oneForm)
            //    return Marshal.StringToHGlobalUni(res);

            //oneForm.Invoke(new Action(() => {
            //    res = oneForm.SendData("CloseOrder;" + id.ToString());
            //}));

            //senddata.Send("CloseOrder;" + id.ToString());
            return Marshal.StringToHGlobalUni(res);
        }

        [DllExport("Connect", CallingConvention = CallingConvention.StdCall)]
        public static void Connect() {
            //senddata.Connect();
        }

        [DllExport("FormChangeTitle", CallingConvention = CallingConvention.StdCall)]
        public static void FormChangeTitle(IntPtr title) {
            if (null == menu)
                return;

            var Tital = Marshal.PtrToStringAnsi(title);

            menu.Invoke(new Action(() => {
                menu.Text = Tital;
            }));
        }
    }
}


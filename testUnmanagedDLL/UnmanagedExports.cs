using System.Text;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CopyForex;
using System.Threading;
using System;

namespace XNore {
    public class CopyDll {
        public static FrmSlave Slave;
        private static FrmMenu menu;
        private static Thread appThread;
        //private static SendData senddata = new SendData();

        public static string res = string.Empty;


        [DllExport("Init", CallingConvention = CallingConvention.StdCall)]
        public static void Init()
        {
            appThread = new Thread(new ThreadStart(OpenForm));
            appThread.Start();
        }

        public static void OpenForm()
        {
            Slave = new FrmSlave();
            menu = new FrmMenu();
            menu.Show();
            Application.Run();
        }

        [DllExport("Shutdown", CallingConvention = CallingConvention.StdCall)]
        public static void Shutdown()
        {
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


        [DllExport("Data_POST", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr Data_POST(double Balance, IntPtr charPos)
        {
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
        public static IntPtr Get_NewOrder(IntPtr symbol)
        {
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
        public static IntPtr Get_CloseOrder(int id)
        {
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
        public static void Connect()
        {
            //senddata.Connect();
        }

        [DllExport("FormChangeTitle", CallingConvention = CallingConvention.StdCall)]
        public static void FormChangeTitle(IntPtr title)
        {
            if (null == menu)
                return;

            var Tital = Marshal.PtrToStringAnsi(title);

            menu.Invoke(new Action(() => {
                menu.Text = Tital;
            }));
        }
    }
}


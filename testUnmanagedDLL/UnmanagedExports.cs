using System.Text;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using testUMD;
using System.Threading;
using System;
using CopyForex;

namespace testUnmanagedDLL {
    class CopyDll {

        private static Form1 oneForm;
        private static Thread appThread;
        private static SendData senddata = new SendData();

        public static string res = string.Empty;


        [DllExport("GUI_Form", CallingConvention = CallingConvention.StdCall)]
        public static void GUI_Form()
        {
            appThread = new Thread(new ThreadStart(OpenForm));
            appThread.Start();
        }

        public static void OpenForm()
        {
            Application.Run(oneForm = new Form1());
        }

        [DllExport("Shutdown", CallingConvention = CallingConvention.StdCall)]
        public static void Shutdown()
        {
            if (null == oneForm)
                return;

            oneForm.Invoke(new Action(() => {
                oneForm.Close();
                oneForm.Dispose();
                oneForm = null;
            }));

        }


        [DllExport("Data_POST", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr Data_POST(double Balance, IntPtr charPos)
        {
            //string res = string.Empty;
            //if (null == oneForm)
            //    return Marshal.StringToHGlobalUni(res);

            var Pos = Marshal.PtrToStringAnsi(charPos);

            //oneForm.Invoke(new Action(() => {
            //    res = oneForm.SendData("POST;" + Balance.ToString() + ";" + Pos);
            //}));

            senddata.Send("POST;" + Balance.ToString() + ";" + Pos);
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

            senddata.Send("NewOrder;" + Symbol);
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

            senddata.Send("CloseOrder;" + id.ToString());
            return Marshal.StringToHGlobalUni(res);
        }

        [DllExport("Connect", CallingConvention = CallingConvention.StdCall)]
        public static void Connect()
        {
            senddata.Connect();
        }

        [DllExport("FormChangeTitle", CallingConvention = CallingConvention.StdCall)]
        public static void FormChangeTitle(IntPtr title)
        {
            if (null == oneForm)
                return;

            var Tital = Marshal.PtrToStringAnsi(title);

            oneForm.Invoke(new Action(() => {
                oneForm.Text = Tital;
            }));
        }
    }
}


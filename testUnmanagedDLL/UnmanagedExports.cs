using System.Text;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using testUMD;
using System.Threading;

namespace testUnmanagedDLL {
    class Test {
        [DllExport("Data_POST", CallingConvention = CallingConvention.StdCall)]
        public static void Data_POST(double Bid,double Ask ,string Pos) {
            

        }

        [DllExport("Get_NewOrder", CallingConvention = CallingConvention.StdCall)]
        public static string Get_NewOrder() {

            return null;
        }

        [DllExport("Get_CloseOrder", CallingConvention = CallingConvention.StdCall)]
        public static string Get_CloseOrder() {

            return null;
        }

        [DllExport("GUI_Form", CallingConvention = CallingConvention.StdCall)]
        public static void GUI_Form() {
            new Thread(new ThreadStart(OpenForm)).Start();
        }

        public static void OpenForm() {
            Application.Run(new Form1());
        }

    }
}


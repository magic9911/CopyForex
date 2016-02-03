using System.Runtime.InteropServices;

namespace YuriNET.Common {

    public static class NativeMethods {

        // Debug console
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
    }
}
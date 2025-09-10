using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NSMBe4
{
    // Allows setting the WM_CLASS correctly in a Mono application
    // Written based on similar code from https://github.com/AirVPN/Eddie
    public static class WmClass
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct XClassHint
        {
            public IntPtr res_name;
            public IntPtr res_class;
        }

        [DllImport("libX11", CharSet = CharSet.Ansi)]
        private static extern int XSetClassHint(IntPtr display, IntPtr window, IntPtr classHint);

        public static void SetWmClass(string name, IntPtr formHandle)
        {
            var winformsAssembly = typeof(Form).Assembly;
            var xplatUIX11 = winformsAssembly.GetType("System.Windows.Forms.XplatUIX11");
            var hwndType = winformsAssembly.GetType("System.Windows.Forms.Hwnd");

            var displayHandleField = xplatUIX11.GetField("DisplayHandle", BindingFlags.NonPublic | BindingFlags.Static);
            IntPtr display = (IntPtr)displayHandleField.GetValue(null);

            var objectFromHandle = hwndType.GetMethod("ObjectFromHandle", BindingFlags.Public | BindingFlags.Static);
            var hwndObj = objectFromHandle.Invoke(null, new object[] { formHandle });
            var wholeWindowField = hwndType.GetField("whole_window", BindingFlags.NonPublic | BindingFlags.Instance);
            IntPtr window = (IntPtr)wholeWindowField.GetValue(hwndObj);

            IntPtr strPtr = Marshal.StringToCoTaskMemAnsi(name);
            var hint = new XClassHint { res_name = strPtr, res_class = strPtr };

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(hint));
            Marshal.StructureToPtr(hint, ptr, false);
            XSetClassHint(display, window, ptr);

            Marshal.FreeHGlobal(strPtr);
            Marshal.FreeHGlobal(ptr);
        }
    }
}

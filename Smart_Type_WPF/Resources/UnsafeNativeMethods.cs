using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Type_WPF.Hot
{
    internal static class UnsafeNativeMethods
    {
        const string _dllLocation = "WindowHookDll.dll";
        [DllImport(_dllLocation)]
        public static extern int Add(int a, int b); // 宣告 C++ 內的方法

        [DllImport(_dllLocation)]
        public static extern int CallWndRetProc(int nCode, IntPtr wParam, IntPtr lParam); // 宣告 C++ 內的方法
        [DllImport(_dllLocation)]
        public static extern bool InstallHook(); // 宣告 C++ 內的方法
        [DllImport(_dllLocation)]
        public static extern bool RemoveHook(); // 宣告 C++ 內的方法
    }
}

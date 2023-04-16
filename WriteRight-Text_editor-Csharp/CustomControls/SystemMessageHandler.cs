using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CustomControls
{
    public static class SystemMessageHandler
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, ref Point lParam);

        public const int WM_VSCROLL = 0x115;

        public const int EM_GETSCROLLPOS = 0x4DD;
        public const int EM_SETSCROLLPOS = 0x4DE;
    }
}

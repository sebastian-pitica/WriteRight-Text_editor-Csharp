using System;
using System.Drawing;
using System.Runtime.InteropServices;

/**************************************************************************
 *                                                                        *
 *  File:        Utilities.cs                                             *
 *  Copyright:   (c) 2023, Caulea Vasile                                  *
 *  Description: Fisierul contine clasa care se ocupa cu mentinearea      *
 *  functiilor care se ocupa cu trimiterea mesajelor catre sistem         *
 *                                                                        *
 **************************************************************************/

namespace CustomControls
{
    public static class SystemMessageHandler
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, ref Point lParam);
        
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int attrValue, int attrSize);
        
        public const int WmVscroll = 0x115;

        public const int EmGetscrollpos = 0x4DD;
        public const int EmSetscrollpos = 0x4DE;
        public const int WmGetdlgcode = 0x87;
        public const int WmMousefirst = 0x200;

    }
}

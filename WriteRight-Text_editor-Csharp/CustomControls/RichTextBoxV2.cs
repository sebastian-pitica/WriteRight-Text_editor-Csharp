using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/**************************************************************************
 *                                                                        *
 *  File:        Commons.cs                                               *
 *  Copyright:   (c) 2023, Pitica Sebastian                               *
 *  Description: Fișierul conține clasele cu caracter general utilizate   *
 *  în cadrul proiectului, separate pentru a putea fi folosit pe tot      *
 *  cuprinsul celorlalte module sau pentru că nu pot fi asociate cu alt   *
 *  modul concret.                                                        *
 *                                                                        *
 **************************************************************************/

namespace CustomControls
{
    /// <summary>
    /// Clase ce reprezintă un wrap-up peste richTextBox. 
    /// Utilizată din necesitatea implementării unor funcționalități ce 
    /// aveau nevoie de adaugarea unor caracteristici noi peste clasa de bază.
    /// </summary>
    /// <creator>Sebastian</creator>
    /// <updated>Vasile</updated>
    public class RichTextBoxV2 : RichTextBox
    {
        private readonly RichTextBox _richTextBoxRef;
        private bool _isSaved;
        private string _filePath;

        private const int WM_VSCROLL = 0x115;  //tells the control to scroll
        private const int WM_GETDLGCODE = 0x87;   //sent when the caret is going out of the 'visible area' (so scroll is needed)
        private const int WM_MOUSEFIRST = 0x200;  //scrolls if the mouse leaves the 'visible area' (example when you select text)
        private const int EM_GETSCROLLPOS = 0x4DD;
        private const int EM_SETSCROLLPOS = 0x4DE;
        private const int WM_MOUSEWHEEL = 0x20A;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, ref Point lParam);


        public RichTextBoxV2(RichTextBox richTextBox) : base()
        {
            _richTextBoxRef = richTextBox;
            _isSaved = true;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_VSCROLL || m.Msg == WM_GETDLGCODE || m.Msg == WM_MOUSEFIRST || m.Msg == WM_MOUSEWHEEL)
            {
                Point p = new Point();
                SendMessage(Handle, EM_GETSCROLLPOS, IntPtr.Zero, ref p);
                SendMessage(_richTextBoxRef.Handle, EM_SETSCROLLPOS, IntPtr.Zero, ref p);
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
            }
        }

        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; }
        }

        public string FileName
        {
            get
            {
                return Path.GetFileName(_filePath);
            }
        }

        public string FileType
        {
            get
            {
                return Path.GetExtension(_filePath);
            }
        }
    }
}

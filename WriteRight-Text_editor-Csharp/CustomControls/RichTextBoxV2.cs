using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static CustomControls.SystemMessageHandler;


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
        private const int ScrollDown = 1;
        private const int ScrollUp = 0;

        private readonly RichTextBox _richTextBoxRef;
        private bool _isSaved;
        private string _filePath;

        public RichTextBoxV2(RichTextBox richTextBox) : base()
        {
            MouseWheel += new MouseEventHandler(RichTextBoxContentMouseWheel);
            _richTextBoxRef = richTextBox;
            _isSaved = true;
        }

        private void RichTextBoxContentMouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
            // Determinam directia miscarii rotitei mouse-ului
            int direction = Math.Sign(e.Delta);
            // Setam directia de derulare
            int scrollDirection = direction == -1 ? ScrollDown : ScrollUp;
            SendMessage(Handle, WM_VSCROLL, (IntPtr)scrollDirection, IntPtr.Zero);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_VSCROLL)
            {
                Point p = new Point();
                SendMessage(Handle, EM_GETSCROLLPOS, IntPtr.Zero, ref p);
                p.X = 0;
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

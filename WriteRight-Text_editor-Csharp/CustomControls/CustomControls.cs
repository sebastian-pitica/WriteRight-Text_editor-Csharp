using Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static CustomControls.SystemMessageHandler;


/**************************************************************************
 *                                                                        *
 *  File:        CustomControls.cs                                        *
 *  Copyright:   (c) 2023, Pitica Sebastian, Caulea Vasile                *
 *  Description: Fișierul conține clase de tip wrap-up (peste controalele *
 *  clasice) utilizate în cadrul proiectului, separate pentru             *
 *  a putea fi folosit pe tot cuprinsul celorlalte module                 *
 *  Updated by: Caulea Vasile                                             *
 *                                                                        *
 **************************************************************************/

namespace CustomControls
{
    #region <design>Pitica Sebastian</design>
    #region <updated>Caulea Vasile</updated>
    /// <summary>
    /// Clase ce reprezintă un wrap-up peste richTextBox. 
    /// Utilizată din necesitatea implementării unor funcționalități ce 
    /// aveau nevoie de adaugarea unor caracteristici noi peste clasa de bază.
    /// </summary>
    public class RichTextBoxV2 : RichTextBox, IRichTextBoxV2, ISubject
    {
        private const int ScrollDown = 1;
        private const int ScrollUp = 0;

        private readonly RichTextBox _richTextBoxRef;
        private bool _isSaved;

        private readonly List<IObserver> _observers;

        public RichTextBoxV2(RichTextBox richTextBox)
        {
            MouseWheel += RichTextBoxContentMouseWheel;
            _richTextBoxRef = richTextBox;
            _isSaved = true;
            _observers = new List<IObserver>();
        }

        private void RichTextBoxContentMouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
            // Determinam directia miscarii rotitei mouse-ului
            int direction = Math.Sign(e.Delta);
            // Setam directia de derulare
            int scrollDirection = direction == -1 ? ScrollDown : ScrollUp;
            SendMessage(Handle, WmVscroll, (IntPtr)scrollDirection, IntPtr.Zero);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg != WmVscroll && m.Msg != WmGetdlgcode && m.Msg != WmMousefirst) return;
            Point p = new Point();
            SendMessage(Handle, EmGetscrollpos, IntPtr.Zero, ref p);
            p.X = 0;
            SendMessage(_richTextBoxRef.Handle, EmSetscrollpos, IntPtr.Zero, ref p);
        }

        public string FilePath { get; set; }

        public bool IsSaved
        {
            get => _isSaved;
            set 
            { 
                 _isSaved = value;
                 NotifyObservers();
            }
        }

        public string FileName => Path.GetFileName(FilePath);

        public string FileType => Path.GetExtension(FilePath);

        public void Attach(IObserver observer)
        {
            if (_observers.Contains(observer))
                return;
            _observers.Add(observer);
        }

        public void NotifyObservers()
        {
            foreach(IObserver observer in _observers)
            {
                observer.UpdateObserver();
            }
        }
    }
    #endregion
    #endregion

    public class TextEditorControl : SplitContainer, ITextEditorControl
    {
        private readonly RichTextBoxV2 _richTextBoxContent;
        private readonly RichTextBox _richTextBoxNumbering;
        private int _previousLineNumber;

        public TextEditorControl()
        {
            _richTextBoxNumbering = new RichTextBox
            {
                ScrollBars = RichTextBoxScrollBars.None,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                SelectionAlignment = HorizontalAlignment.Center,
                WordWrap = false
            };

            _richTextBoxNumbering.GotFocus += (sender, e) => { _richTextBoxContent?.Focus(); };
            _richTextBoxContent = new RichTextBoxV2(_richTextBoxNumbering)
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                WordWrap = false,
                AcceptsTab = true
            };
            _richTextBoxNumbering.Font = _richTextBoxContent.Font;

            _richTextBoxContent.TextChanged += RichTextBoxTextChanged;

            // this control style
            Dock = DockStyle.Fill;
            Panel1.Controls.Add(_richTextBoxNumbering);
            Panel2.Controls.Add(_richTextBoxContent);
            FixedPanel = FixedPanel.Panel1;
            Panel1MinSize = 20;
            SplitterWidth = 1;
            IsSplitterFixed = true;

            UpdateLineNumbers();
        }
        private void RichTextBoxTextChanged(object sender, EventArgs e)
        {
            if (_previousLineNumber != _richTextBoxContent.Lines.Length)
            {
                _previousLineNumber = _richTextBoxContent.Lines.Length;
                UpdateLineNumbers();
            }
            _richTextBoxContent.IsSaved = false;
        }

        private void UpdateLineNumbers()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("1");
            for (int i = 2; i <= _richTextBoxContent.Lines.Length; ++i)
            {
                sb.AppendLine(i.ToString());
            }
            _richTextBoxNumbering.Text = sb.ToString();
        }

        public override Font Font
        {
            get => base.Font;

            set
            {
                base.Font = value;
                _richTextBoxNumbering.Font = value;
            }
        }

        public IRichTextBoxV2 RichTextBoxEditor => _richTextBoxContent;

        public RichTextBox RichTextBoxNumbering => _richTextBoxNumbering;

        public float ZoomFactor
        {
            get => _richTextBoxContent.ZoomFactor;

            set
            {
                _richTextBoxContent.ZoomFactor = value;
                _richTextBoxNumbering.ZoomFactor = value;
            }
        }
    }
}

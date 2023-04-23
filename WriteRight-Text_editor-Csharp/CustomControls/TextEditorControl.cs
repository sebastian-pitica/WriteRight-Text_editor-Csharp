using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CustomControls
{
    public class TextEditorControl : SplitContainer
    {
        private readonly RichTextBoxV2 _richTextBoxContent;
        private readonly RichTextBox _richTextBoxNumbering;
        private int _previousLineNumber = 0;

        public TextEditorControl()
        {
            _richTextBoxNumbering = new RichTextBox
            {
                ScrollBars = RichTextBoxScrollBars.None,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                SelectionAlignment = HorizontalAlignment.Center,
            };

            _richTextBoxNumbering.GotFocus += (sender, e) => { _richTextBoxContent.Focus(); };
            _richTextBoxContent = new RichTextBoxV2(_richTextBoxNumbering)
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                WordWrap = false
            };
            _richTextBoxNumbering.Font = _richTextBoxContent.Font;

            _richTextBoxContent.TextChanged += new EventHandler(RichTextBoxTextChanged);

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

        public RichTextBoxV2 RichTextBoxEditor
        {
            get => _richTextBoxContent;
        }

        public RichTextBox RichTextBoxNumbering
        {
            get => _richTextBoxNumbering;
        }
    }
}

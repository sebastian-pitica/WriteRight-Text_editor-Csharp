using CommonsModule;
using Interfaces;
using System;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;
using System.Configuration;

/**************************************************************************
 *                                                                        *
 *  File:        EditRibbonModule.cs                                      *
 *  Copyright:   (c) 2023, Pitica Sebastian                               *
 *  Description: Fișierul conține obiectele de tip Singleton-Command      *
 *  aferente ribbonului Edit, separate pentru a se putea lucra individual *
 *  de celelalte ribbonuri.                                               *
 *                                                                        *
 **************************************************************************/


namespace EditRibbonModule
{

    public class UndoCommand : IMainTextBoxCommand
    {
        private static UndoCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;

        private UndoCommand() { }

        public static new UndoCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new UndoCommand();
            }

            return _singletonInstance;
        }

        public override void SetTarget(RichTextBoxV2 textBox)
        {
            _mainTextBoxRef = textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.Undo();
        }
    }

    public class RedoCommand : IMainTextBoxCommand
    {
        private static RedoCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;

        private RedoCommand() { }

        public static new RedoCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new RedoCommand();
            }

            return _singletonInstance;
        }

        public override void SetTarget(RichTextBoxV2 textBox)
        {
            _mainTextBoxRef = textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.Redo();
        }
    }

    public class CutCommand : IMainTextBoxCommand
    {
        private static CutCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;

        private CutCommand() { }

        public static new CutCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new CutCommand();
            }

            return _singletonInstance;
        }

        public override void SetTarget(RichTextBoxV2 textBox)
        {
            _mainTextBoxRef = textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.Cut();
        }
    }

    public class CopyCommand : IMainTextBoxCommand
    {
        private static CopyCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;

        private CopyCommand() { }

        public static new CopyCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new CopyCommand();
            }

            return _singletonInstance;
        }

        public override void SetTarget(RichTextBoxV2 textBox)
        {
            _mainTextBoxRef = textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.Copy();
        }
    }

    public class PasteCommand : IMainTextBoxCommand
    {
        private static PasteCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;

        private PasteCommand() { }

        public static new PasteCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new PasteCommand();
            }

            return _singletonInstance;
        }

        public override void SetTarget(RichTextBoxV2 textBox)
        {
            _mainTextBoxRef = textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.Paste();
        }
    }

    public class DeleteCommand : IMainTextBoxCommand
    {
        private static DeleteCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;

        private DeleteCommand() { }

        public static new DeleteCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new DeleteCommand();
            }

            return _singletonInstance;
        }

        public override void SetTarget(RichTextBoxV2 textBox)
        {
            _mainTextBoxRef = textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.SelectedText = "";
        }
    }

    public class SearchAndReplaceCommand : IMainTextBoxCommand
    {
        private static SearchAndReplaceCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;
        private int indexReplaceNext;
        private string lastSearchedText;
        private SearchAndReplaceCommand() { }

        public static new SearchAndReplaceCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new SearchAndReplaceCommand();
            }

            return _singletonInstance;
        }

        public override void SetTarget(RichTextBoxV2 textBox)
        {
            this._mainTextBoxRef = textBox;
        }

        public override void Execute()
        {
            // Creez fereastra pentru preluarea inputului
            Form windowInputData = new Form
            {
                Text = "Search",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(20, 10, 20, 10)
            };

            // Creaza un panou pentru atasarea elementelor grafice
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
            };

            Label labelSearchText = new Label
            {
                Text = "Search Text:",
                Margin = new Padding(0),
            };

            Label labelReplaceText = new Label
            {
                Text = "Replace Text:",
                Margin = new Padding(0)
            };

            TextBox textBoxSearchText = new TextBox
            {
                Width = 200,
                Margin = new Padding(0, 0, 0, 10)
            };

            TextBox textBoxReplaceText = new TextBox
            {
                Width = 200,
                Margin = new Padding(0, 0, 0, 10)
            };

            Button buttonReplaceAll = new Button
            {
                Text = "Replace All",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            Button buttonReplaceNext = new Button
            {
                Text = "Replace Next",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            Button buttonCancel = new Button
            {
                Text = "Cancel",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            buttonReplaceAll.Click += (sender, args) =>
            {
                string searchWord = textBoxSearchText.Text;
                string replaceText = textBoxReplaceText.Text;

                if (searchWord == string.Empty)
                    return;

                int index = 0;
                while ((index = _mainTextBoxRef.Text.IndexOf(searchWord, index)) != -1)
                {
                    _mainTextBoxRef.Select(index, searchWord.Length);
                    _mainTextBoxRef.SelectedText = replaceText;
                    index += replaceText.Length;
                }

            };

            ///<bug>next replacement position</bug>
            buttonReplaceNext.Click += (sender, args) =>
            {
                string searchWord = textBoxSearchText.Text;
                string replaceText = textBoxReplaceText.Text;

                if (searchWord == string.Empty)
                    return;

                // verificam daca s-a introdus alt text de cautare pentru a reinitializa indexul de cautare
                if (lastSearchedText == null)
                {
                    lastSearchedText = string.Copy(searchWord);
                }
                else
                {
                    if (!lastSearchedText.Equals(searchWord))
                    {
                        indexReplaceNext = 0;
                    }
                }

                if (indexReplaceNext != -1)
                {
                    indexReplaceNext = _mainTextBoxRef.Text.IndexOf(searchWord, indexReplaceNext);

                    if (indexReplaceNext != -1)
                    {
                        _mainTextBoxRef.Select(indexReplaceNext, searchWord.Length);
                        _mainTextBoxRef.SelectedText = replaceText;
                        indexReplaceNext += replaceText.Length;
                    }
                }
            };

            buttonCancel.Click += (sender, args) =>
            {
                indexReplaceNext = 0;
                windowInputData.Close();
            };

            flowLayoutPanel.Controls.Add(labelSearchText);
            flowLayoutPanel.Controls.Add(textBoxSearchText);
            flowLayoutPanel.Controls.Add(labelReplaceText);
            flowLayoutPanel.Controls.Add(textBoxReplaceText);
            flowLayoutPanel.Controls.Add(buttonReplaceAll);
            flowLayoutPanel.Controls.Add(buttonReplaceNext);
            flowLayoutPanel.Controls.Add(buttonCancel);

            //Adaug panoul in fereastra si afisez fereastra
            windowInputData.Controls.Add(flowLayoutPanel);
            windowInputData.ShowDialog();
        }
    }

    public class SearchCommand : IMainTextBoxCommand
    {
        private static SearchCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;

        private SearchCommand() { }

        public static new SearchCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new SearchCommand();
            }

            return _singletonInstance;
        }

        public override void SetTarget(RichTextBoxV2 mainTextBox)
        {
            _mainTextBoxRef = mainTextBox;
        }

        public override void Execute()
        {
            // Creez fereastra pentru preluarea inputului
            Form inputDataForm = new Form
            {
                Text = "Search",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(20, 10, 20, 10)
            };

            // Creaza un panou pentru atasarea elementelor grafice
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,

            };

            Label labelSearchText = new Label
            {
                Text = "Search Text:",
                Margin = new Padding(0)
            };

            TextBox textBoxSearchText = new TextBox
            {
                Width = 200,
                Margin = new Padding(0, 0, 0, 10)
            };

            Button buttonSearch = new Button
            {
                Text = "Search",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            Button buttonCancel = new Button
            {
                Text = "Cancel",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            buttonSearch.Click += (sender, args) =>
            {
                string searchWord = textBoxSearchText.Text;

                if (searchWord == string.Empty)
                    return;

                int index = 0;
                while ((index = _mainTextBoxRef.Text.IndexOf(searchWord, index)) != -1)
                {
                    _mainTextBoxRef.Select(index, searchWord.Length);
                    _mainTextBoxRef.SelectionBackColor = Color.Yellow;
                    index += searchWord.Length;
                }
            };

            buttonCancel.Click += (sender, args) =>
            {
                inputDataForm.Close();
            };

            flowLayoutPanel.Controls.Add(labelSearchText);
            flowLayoutPanel.Controls.Add(textBoxSearchText);
            flowLayoutPanel.Controls.Add(buttonSearch);
            flowLayoutPanel.Controls.Add(buttonCancel);

            // Adaug panoul in fereastra si afisez fereastra
            inputDataForm.Controls.Add(flowLayoutPanel);
            inputDataForm.Show();
        }
    }

    public class ReportBugCommand : IMainTextBoxCommand
    {
        private static ReportBugCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;

        private ReportBugCommand() { }

        public static new ReportBugCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new ReportBugCommand();
            }

            return _singletonInstance;
        }

        public override void SetTarget(RichTextBoxV2 mainTextBox)
        {
            _mainTextBoxRef = mainTextBox;
        }

        public override void Execute()
        {
            // Creez fereastra pentru preluarea inputului
            Form windowInputData = new Form
            {
                Text = "Send bug report",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(20, 10, 20, 10)
            };

            // Creaza un panou pentru atasarea elementelor grafice
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
            };

            Label labelBugDetails = new Label
            {
                Text = "Bug Details:",
                Margin = new Padding(0),
            };

            Label labelEmail = new Label
            {
                Text = "Your email:",
                Margin = new Padding(0),
            };

            Label labelPassword = new Label
            {
                Text = "Your password:",
                Margin = new Padding(0),
            };

            TextBox textBoxBugDetails = new TextBox
            {
                Width = 200,
                Height = 50,
                Margin = new Padding(0, 0, 0, 10),
                Multiline = true,
                ScrollBars = ScrollBars.Both
            };

            TextBox textBoxEmail = new TextBox
            {
                Width = 200,
                Margin = new Padding(0, 0, 0, 10),
                MaxLength = 50
            };

            TextBox textBoxPassword = new TextBox
            {
                Width = 200,
                Margin = new Padding(0, 0, 0, 10),
                UseSystemPasswordChar = true
            };


            Button buttonSend = new Button
            {
                Text = "Send report",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            Button buttonCancel = new Button
            {
                Text = "Cancel",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            buttonSend.Click += (senderl, args) =>
            {
                try
                {
                    string defaultRecipient = ConfigurationManager.AppSettings["DefaultRecipient"];
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(textBoxEmail.Text);
                    message.To.Add(defaultRecipient);
                    message.Subject = "Bug Report from App";
                    message.Body = textBoxBugDetails.Text;

                    // Create a new SmtpClient object and send the email
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(textBoxEmail.Text, textBoxPassword.Text);
                    client.Send(message);

                    MessageBox.Show("Bug report sent successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + textBoxEmail.Text + "\n" + textBoxPassword.Text + "\n" + ConfigurationManager.AppSettings["DefaultRecipient"], "Exceptie!");
                }
            };


            buttonCancel.Click += (senderl, args) =>
            {
                MessageBox.Show("Bug report canceled.");
                windowInputData.Close();
            };

            flowLayoutPanel.Controls.Add(labelBugDetails);
            flowLayoutPanel.Controls.Add(textBoxBugDetails);
            flowLayoutPanel.Controls.Add(labelEmail);
            flowLayoutPanel.Controls.Add(textBoxEmail);
            flowLayoutPanel.Controls.Add(labelPassword);
            flowLayoutPanel.Controls.Add(textBoxPassword);
            flowLayoutPanel.Controls.Add(buttonSend);
            flowLayoutPanel.Controls.Add(buttonCancel);

            //Adaug panoul in fereastra si afisez fereastra
            windowInputData.Controls.Add(flowLayoutPanel);
            windowInputData.ShowDialog();
        }
    }
}



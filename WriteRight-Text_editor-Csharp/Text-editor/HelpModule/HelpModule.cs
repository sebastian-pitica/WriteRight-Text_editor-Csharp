using CustomControls;
using Interfaces;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using static CommonsModule.Utilities;
    
/**************************************************************************
 *                                                                        *
 *  File:        HelpModule.cs                                            *
 *  Copyright:   (c) 2023, Pitica Sebastian                               *
 *  Description: Fișierul conține obiectele de tip Singleton-Command      *
 *  aferente ribbonului Help și pentru unele comenzi de pe interfața      *
 *  grafică, separate pentru a se putea lucra individual de celelalte     *
 *  ribbonuri.                                                            *
 *  Updated by: Matei Rares                                               *
 **************************************************************************/


namespace HelpModule
{
    public class ReportBugCommand : SingletonCommand
    {
        private static ReportBugCommand _singletonInstance;
        
        private ReportBugCommand() { }

        public new static ReportBugCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new ReportBugCommand());
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
                    HandleException(new Exception(ex.Message + "\n" + textBoxEmail.Text + "\n" + textBoxPassword.Text + "\n" + ConfigurationManager.AppSettings["DefaultRecipient"]));
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

    public class HelpCommand : MainTextBoxCommand
    {
        private static HelpCommand _singletonInstance;
        private RichTextBoxV2 _mainTextBoxRef;

        private HelpCommand() { }

        public new static HelpCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new HelpCommand());
        }

        public override void SetTarget(IRichTextBoxV2 mainTextBox)
        {
            _mainTextBoxRef = (RichTextBoxV2)mainTextBox;
        }

        public override void Execute()
        {
            Help.ShowHelp(_mainTextBoxRef, ""); //todo
        }
    }

    public class AboutCommand : SingletonCommand
    {
        private static AboutCommand _singletonInstance;

        private AboutCommand() { }

        public new static AboutCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new AboutCommand());
        }
        
        public override void Execute()
        {
            MessageBox.Show("Home:https://github.com/sebastian-pitica/WriteRight-Text_editor-Csharp\n\nThis program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 3 of the License, or at your option any later version.\r\n\r\nThis program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. \r\n\r\nYou should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.\n\nCreated by: Pitica Sebastian, Vasile Caulea, Matei Rareș.", "WRITE RIGHT - About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
       
    }

    public class ZoomInCommand : TextEditorControlCommand
    {
        private static ZoomInCommand _singletonInstance;
        private TextEditorControl _textEditorControlRef;

        private ZoomInCommand() { }

        public new static ZoomInCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new ZoomInCommand());
        }

        public override void Execute()
        {
            if (!(_textEditorControlRef.ZoomFactor < 4.25f)) return;
            _textEditorControlRef.ZoomFactor += 0.25f;
            _textEditorControlRef.SplitterDistance += 3;
        }

        public override void SetTarget(ITextEditorControl textEditorControl)
        {
            _textEditorControlRef = (TextEditorControl)textEditorControl;
        }
    }

    public class ZoomOutCommand : TextEditorControlCommand
    {
        private static ZoomOutCommand _singletonInstance;
        private TextEditorControl _textEditorControlRef;

        private ZoomOutCommand() { }

        public new static ZoomOutCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new ZoomOutCommand());
        }

        public override void Execute()
        {
            if (!(_textEditorControlRef.ZoomFactor > 0.25f)) return;
            _textEditorControlRef.ZoomFactor -= 0.25f;
            _textEditorControlRef.SplitterDistance -= 3;
        }

        public override void SetTarget(ITextEditorControl textEditorControl)
        {
            _textEditorControlRef = (TextEditorControl)textEditorControl;
        }
    }
}

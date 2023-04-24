using CommonsModule;
using CustomControls;
using Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace FileRibbonModule
{
    public class NewFileCommand : ITabControlCommand
    {
        private static NewFileCommand _singletonInstance = null;
        private TabControl _mainTabControlRef;

        private NewFileCommand()
        {

        }

        public static new NewFileCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new NewFileCommand();
            }
            return _singletonInstance;
        }

        public override void Execute()
        {
            TabPage tabPage = Utilities.CreateTab("new");
            if (UtilitiesFormat.isDarkmode)
            {
                tabPage.BackColor =  ColorTranslator.FromHtml("#24292E");
                tabPage.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                ((TextEditorControl)tabPage.Controls[0]).RichTextBoxEditor.BackColor = ColorTranslator.FromHtml("#24292E");
                ((TextEditorControl)(tabPage.Controls[0])).RichTextBoxEditor.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                ((TextEditorControl)(tabPage.Controls[0])).BackColor = ColorTranslator.FromHtml("#C8D3DA");

                (((TextEditorControl)tabPage.Controls[0])).RichTextBoxNumbering.BackColor = ColorTranslator.FromHtml("#24292E");
                (((TextEditorControl)tabPage.Controls[0])).RichTextBoxNumbering.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
            }
            _mainTabControlRef.TabPages.Add(tabPage);
            
            _mainTabControlRef.SelectedIndex = _mainTabControlRef.TabPages.Count - 1;
        }

        public override void SetTarget(TabControl tabControl)
        {
            _mainTabControlRef = tabControl;
        }
    }

    public class OpenFileCommand : ITabControlCommand
    {
        private static OpenFileCommand _singletonInstance = null;
        private TabControl _mainTabControlRef;
        private OpenFileDialog _openFileDialog;
        private OpenFileCommand()
        {
            _openFileDialog = new OpenFileDialog
            {
                Filter = "C/C++ Files (*.c, *.cpp)|*.c;*.cpp|Text Files(*.txt)|*.txt|All Files(*.*)|*.*"
            };
        }

        public static new OpenFileCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new OpenFileCommand();
            }
            return _singletonInstance;
        }

        public override void Execute()
        {
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = _openFileDialog.FileName;
                if (string.IsNullOrEmpty(path))
                    return;

                try
                {
                    StreamReader streamReader = new StreamReader(path);

                    TabPage tabPage = Utilities.CreateTab(Path.GetFileName(path));
                    _mainTabControlRef.TabPages.Add(tabPage);
                    if (UtilitiesFormat.isDarkmode)
                    {
                        tabPage.BackColor = ColorTranslator.FromHtml("#24292E");
                        tabPage.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                        ((TextEditorControl)tabPage.Controls[0]).RichTextBoxEditor.BackColor = ColorTranslator.FromHtml("#24292E");
                        ((TextEditorControl)(tabPage.Controls[0])).RichTextBoxEditor.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                        ((TextEditorControl)(tabPage.Controls[0])).BackColor = ColorTranslator.FromHtml("#C8D3DA");

                        (((TextEditorControl)tabPage.Controls[0])).RichTextBoxNumbering.BackColor = ColorTranslator.FromHtml("#24292E");
                        (((TextEditorControl)tabPage.Controls[0])).RichTextBoxNumbering.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                    }

                    _mainTabControlRef.SelectedIndex = _mainTabControlRef.TabPages.Count - 1;

                    RichTextBoxV2 mainTextBoxRef = Utilities.GetRichTextBoxV2FTabControl(_mainTabControlRef);

                    mainTextBoxRef.Text = streamReader.ReadToEnd();
                    streamReader.Close();
                    mainTextBoxRef.FilePath = path;
                    mainTextBoxRef.IsSaved = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not open the file..", "Error: Open file ");
                }
            }
        }

        public override void SetTarget(TabControl tabControl)
        {
            _mainTabControlRef = tabControl;
        }
    }

    public class SaveFileCommand : IMainTextBoxCommand
    {
        private static SaveFileCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;
        private SaveFileDialog _saveFileDialog;
        private SaveFileCommand()
        {
            _saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "Text Files(*.txt)",
                Filter = "Text Files(*.txt)|*.txt|All Files(*.*)|*.*"
            };
        }

        public static new SaveFileCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new SaveFileCommand();
            }
            return _singletonInstance;
        }

        public override void Execute()
        {
            string filePath = _mainTextBoxRef.FilePath;
            if (!File.Exists(filePath))
            {
                if (_saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = _saveFileDialog.FileName;
                    try
                    {
                        Utilities.WriteFile(path, _mainTextBoxRef.Text);
                        _mainTextBoxRef.FilePath = path;
                        _mainTextBoxRef.IsSaved = true;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Could not save the file..", "Error: Save file");
                    }
                }
            }
            else if (!_mainTextBoxRef.IsSaved)
            {
                try
                {
                    Utilities.WriteFile(filePath, _mainTextBoxRef.Text);
                    _mainTextBoxRef.IsSaved = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not save the file..", "Error: Save file");
                }
            }
        }

        public override void SetTarget(RichTextBoxV2 mainTextBox)
        {
            _mainTextBoxRef = mainTextBox;
        }
    }

    public class CloseFileCommand : ITabControlCommand
    {
        private static CloseFileCommand _singletonInstance = null;
        private TabControl _mainTabControlRef;

        private CloseFileCommand()
        {

        }

        public static new CloseFileCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new CloseFileCommand();
            }
            return _singletonInstance;
        }

        public override void Execute()
        {
            RichTextBoxV2 richTextBoxV2 = Utilities.GetRichTextBoxV2FTabControl(_mainTabControlRef);
            if (!richTextBoxV2.IsSaved)
            {
                string fileName = Utilities.GetFileNameFromTabControl(_mainTabControlRef);
                string message = "Save file \"" + fileName + "\"?";
                DialogResult result = MessageBox.Show(message, "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    IMainTextBoxCommand command = SaveFileCommand.GetCommandObj();
                    command.SetTarget(richTextBoxV2);
                    command.Execute();

                    if (richTextBoxV2.IsSaved)
                        RemoveCurrentTab();
                }
                else if (result == DialogResult.No)
                {
                    RemoveCurrentTab();
                }
            }
            else
            {
                RemoveCurrentTab();
            }
        }

        private void RemoveCurrentTab()
        {
            int index = _mainTabControlRef.SelectedIndex;
            _mainTabControlRef.TabPages.RemoveAt(index);
        }

        public override void SetTarget(TabControl tabControl)
        {
            _mainTabControlRef = tabControl;
        }
    }

    public class OpenNewWindowCommand : ISingletonCommand
    {
        private static OpenNewWindowCommand _singletonInstance;

        private OpenNewWindowCommand()
        {

        }

        public static new OpenNewWindowCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new OpenNewWindowCommand();
            }
            return _singletonInstance;
        }

        public override void Execute()
        {
            try
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error creating new window...");
            }
        }
    }
}

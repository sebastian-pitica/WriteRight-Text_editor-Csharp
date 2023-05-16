using CommonsModule;
using CustomControls;
using Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

/**************************************************************************
 *                                                                        *
 *  File:        FileRibbonModule.cs                                      *
 *  Copyright:   (c) 2023, Caulea Vasile                                  *
 *  Description: Fișierul conține obiectele de tip Singleton-Command      *
 *  asociate cu ribbon-ul File, care pun la dispozitie comenzile de       *
 *  creare, salvare, deschidere, inchidere a unui fisier si deschidere a  *
 *  unei noi ferestre.                                                    *
 *                                                                        *
 **************************************************************************/

namespace FileRibbonModule
{
    public class NewFileCommand : TabControlCommand
    {
        private static NewFileCommand _singletonInstance;
        private TabControl _mainTabControlRef;

        private NewFileCommand()
        {

        }

        public new static NewFileCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new NewFileCommand());
        }

        public override void Execute()
        {
            TabPage tabPage = Utilities.CreateTab("new");
            _mainTabControlRef.TabPages.Add(tabPage);
            _mainTabControlRef.SelectedIndex = _mainTabControlRef.TabPages.Count - 1;
        }

        public override void SetTarget(TabControl tabControl)
        {
            _mainTabControlRef = tabControl;
        }
    }

    public class OpenFileCommand : TabControlCommand
    {
        private static OpenFileCommand _singletonInstance;
        private TabControl _mainTabControlRef;
        private readonly OpenFileDialog _openFileDialog;
        private OpenFileCommand()
        {
            _openFileDialog = new OpenFileDialog
            {
                Filter = string.Join("|", Utilities.FileFilters)
            };
        }

        public new static OpenFileCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new OpenFileCommand());
        }

        public override void Execute()
        {
            if (_openFileDialog.ShowDialog() != DialogResult.OK) return;
            string path = _openFileDialog.FileName;
            if (string.IsNullOrEmpty(path))
                return;

            try
            {
                StreamReader streamReader = new StreamReader(path);

                TabPage tabPage = Utilities.CreateTab(Path.GetFileName(path));
                _mainTabControlRef.TabPages.Add(tabPage);
                _mainTabControlRef.SelectedIndex = _mainTabControlRef.TabPages.Count - 1;

                RichTextBoxV2 mainTextBoxRef = Utilities.GetRichTextBoxV2FromTabControl(_mainTabControlRef);

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

        public override void SetTarget(TabControl tabControl)
        {
            _mainTabControlRef = tabControl;
        }
    }

    public class SaveFileCommand : MainTextBoxCommand
    {
        private static SaveFileCommand _singletonInstance;
        private RichTextBoxV2 _mainTextBoxRef;
        private readonly SaveFileDialog _saveFileDialog;
        private SaveFileCommand()
        {
            _saveFileDialog = new SaveFileDialog
            {
                Filter = string.Join("|", Utilities.FileFilters)
            };
        }

        public new static SaveFileCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new SaveFileCommand());
        }

        public override void Execute()
        {
            string filePath = _mainTextBoxRef.FilePath;
            // verificare daca fisierul e nou si trebuie salvat sau
            // daca fisierul a fost deschis in editor si nu mai exista in sistem
            if (!File.Exists(filePath))
            {
                if (_saveFileDialog.ShowDialog() != DialogResult.OK) return;
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

        public override void SetTarget(IRichTextBoxV2 mainTextBox)
        {
            _mainTextBoxRef = (RichTextBoxV2)mainTextBox;
        }
    }

    public class CloseFileCommand : TabControlCommand
    {
        private static CloseFileCommand _singletonInstance;
        private TabControl _mainTabControlRef;

        private CloseFileCommand()
        {

        }

        public new static CloseFileCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new CloseFileCommand());
        }

        public override void Execute()
        {
            RichTextBoxV2 richTextBoxV2 = Utilities.GetRichTextBoxV2FromTabControl(_mainTabControlRef);
            if (!richTextBoxV2.IsSaved)
            {
                string fileName = Utilities.GetFileNameFromTabControl(_mainTabControlRef, true);
                string message = "Save file \"" + fileName + "\"?";
                DialogResult result = MessageBox.Show(message, "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        {
                            MainTextBoxCommand command = SaveFileCommand.GetCommandObj();
                            command.SetTarget(richTextBoxV2);
                            command.Execute();

                            if (richTextBoxV2.IsSaved)
                                RemoveCurrentTab();
                            break;
                        }
                    case DialogResult.No:
                        RemoveCurrentTab();
                        break;
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

    public class OpenNewWindowCommand : SingletonCommand
    {
        private static OpenNewWindowCommand _singletonInstance;

        private OpenNewWindowCommand()
        {

        }

        public new static OpenNewWindowCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new OpenNewWindowCommand());
        }

        public override void Execute()
        {
            try
            {
                Process.Start(Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error creating new window...");
            }
        }
    }
}

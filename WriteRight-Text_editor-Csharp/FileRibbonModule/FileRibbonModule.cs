using CommonsModule;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileRibbonModule
{
    public class NewFileCommand : IMainTextBoxCommand
    {
        private static NewFileCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;

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
            MessageBox.Show("Executing creating new file...");
        }

        public override void SetTarget(RichTextBoxV2 mainTextBox)
        {
            _mainTextBoxRef = mainTextBox;
        }
    }

    public class OpenFileCommand : IMainTextBoxCommand
    {
        private static OpenFileCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;

        private OpenFileCommand()
        {

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
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                if (string.IsNullOrEmpty(path))
                    return;
                try
                {
                    StreamReader streamReader = new StreamReader(path, true);
                    _mainTextBoxRef.baseComponent.Text = streamReader.ReadToEnd();
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not open the file..", "Error: Open file ");
                }
            }
        }

        public override void SetTarget(RichTextBoxV2 mainTextBox)
        {
            _mainTextBoxRef = mainTextBox;
        }
    }

    public class SaveFileCommand : IMainTextBoxCommand
    {
        private static SaveFileCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;

        private SaveFileCommand()
        {

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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "Text Files(*.txt)";
            saveFileDialog.Filter = "Text Files(*.txt) | *.txt | All Files(*.*) | *.* ";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;
                try
                {
                    StreamWriter streamWriter = new StreamWriter(path);
                    streamWriter.Write(_mainTextBoxRef.baseComponent.Text);
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

using CommonsModule;
using CustomControls;
using FileRibbonModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TextEditor;

/**************************************************************************
 *                                                                        *
 *  File:        UnitTestEditMenu.cs                                      *
 *  Copyright:   (c) 2023, Vasile Caulea                                  *
 *  Description: Fișierul conține funcți de test pentru funcționalitățile *
 *  moduluilui File                                                       *               
 *                                                                        * 
 **************************************************************************/

namespace UnitTestProjectFileRibbonMenu
{
    [TestClass]
    public class UnitTestFileMenu
    {
        private readonly FormMainWindow _formMainWindow = new FormMainWindow();
        private TabControl _tabControlFiles;
        private RichTextBoxV2 _mainTextBox;

        [TestInitialize]
        public void Setup()
        {
            var tabControlField = _formMainWindow.GetType().GetField("tabControlFiles", BindingFlags.NonPublic | BindingFlags.Instance);
            if (tabControlField != null) _tabControlFiles = (TabControl)tabControlField.GetValue(_formMainWindow);

            var mainTextBoxField = _formMainWindow.GetType().GetField("_richTextBoxMainV2", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mainTextBoxField != null) _mainTextBox = (RichTextBoxV2)mainTextBoxField.GetValue(_formMainWindow);
        }

        [TestMethod]
        public void TestMethodNewFile()
        {
            int expectedOutput = _tabControlFiles.TabCount + 1;

            NewFileCommand command = NewFileCommand.GetCommandObj();
            command.SetTarget(_tabControlFiles);
            command.Execute();
            
            Assert.AreEqual(expectedOutput, _tabControlFiles.TabCount);
        }

        [TestMethod]
        public void TestMethodCloseFile()
        {
            int expectedOutput = _tabControlFiles.TabCount - 1;
            CloseFileCommand command = CloseFileCommand.GetCommandObj();
            command.SetTarget(_tabControlFiles);
            command.Execute();

            Assert.AreEqual(expectedOutput, _tabControlFiles.TabCount);
        }


        [TestMethod]
        public void TestMethodOpenFile()
        {
            int expectedTabNumber = _tabControlFiles.TabCount + 1;
            OpenFileCommand command = OpenFileCommand.GetCommandObj();
            command.SetTarget(_tabControlFiles);
            command.Execute();

            Assert.AreEqual(expectedTabNumber, _tabControlFiles.TabCount);

            _mainTextBox = Utilities.GetRichTextBoxV2FromTabControl(_tabControlFiles);
            StreamReader streamReader = new StreamReader(_mainTextBox.FilePath);

            Assert.AreEqual(streamReader.ReadToEnd(), _mainTextBox.Text);
            streamReader.Close();
        }


        [TestMethod]
        public void TestMethodSaveEmptyNewFile()
        {
            // first, the file must appear as saved
            Assert.IsTrue(_mainTextBox.IsSaved);
            
            // modify the text
            _mainTextBox.Text = "acesta este un text modificat 22";

            //Assert.IsFalse(mainTextBox.IsSaved);

            SaveFileCommand command = SaveFileCommand.GetCommandObj();
            command.SetTarget(_mainTextBox);
            command.Execute();
            // if executed properly the file should appear as saved
            Assert.IsTrue(_mainTextBox.IsSaved, "The file could not be saved.");

            // verify if content saved properly
            StreamReader streamReader = new StreamReader(_mainTextBox.FilePath);
            Assert.AreEqual(streamReader.ReadToEnd(), _mainTextBox.Text);
            streamReader.Close();
        }
    }
}

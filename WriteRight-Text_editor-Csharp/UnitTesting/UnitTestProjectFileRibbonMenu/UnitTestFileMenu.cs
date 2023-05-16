using CommonsModule;
using CustomControls;
using FileRibbonModule;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TextEditor;

namespace UnitTestProjectFileRibbonMenu
{
    [TestClass]
    public class UnitTestFileMenu
    {
        private readonly FormMainWindow formMainWindow = new FormMainWindow();
        private TabControl tabControlFiles;
        private RichTextBoxV2 mainTextBox;

        [TestInitialize]
        public void Setup()
        {
            var tabControlField = formMainWindow.GetType().GetField("tabControlFiles", BindingFlags.NonPublic | BindingFlags.Instance);
            tabControlFiles = (TabControl)tabControlField.GetValue(formMainWindow);

            var mainTextBoxField = formMainWindow.GetType().GetField("_richTextBoxMainV2", BindingFlags.NonPublic | BindingFlags.Instance);
            mainTextBox = (RichTextBoxV2)mainTextBoxField.GetValue(formMainWindow);
        }

        [TestMethod]
        public void TestMethodNewFile()
        {
            int expectedOutput = tabControlFiles.TabCount + 1;

            NewFileCommand command = NewFileCommand.GetCommandObj();
            command.SetTarget(tabControlFiles);
            command.Execute();
            
            Assert.AreEqual(expectedOutput, tabControlFiles.TabCount);
        }

        [TestMethod]
        public void TestMethodCloseFile()
        {
            int expectedOutput = tabControlFiles.TabCount - 1;
            CloseFileCommand command = CloseFileCommand.GetCommandObj();
            command.SetTarget(tabControlFiles);
            command.Execute();

            Assert.AreEqual(expectedOutput, tabControlFiles.TabCount);
        }


        [TestMethod]
        public void TestMethodOpenFile()
        {
            int expectedTabNumber = tabControlFiles.TabCount + 1;
            OpenFileCommand command = OpenFileCommand.GetCommandObj();
            command.SetTarget(tabControlFiles);
            command.Execute();

            Assert.AreEqual(expectedTabNumber, tabControlFiles.TabCount);

            mainTextBox = Utilities.GetRichTextBoxV2FromTabControl(tabControlFiles);
            StreamReader streamReader = new StreamReader(mainTextBox.FilePath);

            Assert.AreEqual(streamReader.ReadToEnd(), mainTextBox.Text);
            streamReader.Close();
        }


        [TestMethod]
        public void TestMethodSaveEmptyNewFile()
        {
            // first, the file must appear as saved
            Assert.IsTrue(mainTextBox.IsSaved);
            
            // modify the text
            mainTextBox.Text = "acesta este un text modificat 22";

            //Assert.IsFalse(mainTextBox.IsSaved);

            SaveFileCommand command = SaveFileCommand.GetCommandObj();
            command.SetTarget(mainTextBox);
            command.Execute();
            // if executed properly the file should appear as saved
            Assert.IsTrue(mainTextBox.IsSaved, "The file could not be saved.");

            // verify if content saved properly
            StreamReader streamReader = new StreamReader(mainTextBox.FilePath);
            Assert.AreEqual(streamReader.ReadToEnd(), mainTextBox.Text);
            streamReader.Close();
        }
    }
}

using CommonsModule;
using CustomControls;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Windows.Forms;
using TextEditor;

/**************************************************************************
 *                                                                        *
 *  File:        UnitTestEditMenu.cs                                      *
 *  Copyright:   (c) 2023, Vasile Caulea                                  *
 *  Description: Fișierul conține funcții de test pentru funcționalitățile*
 *  moduluilui Utilities                                                  *               
 *                                                                        * 
 **************************************************************************/

namespace UnitTestProjectUtilities
{
    [TestClass]
    public class UnitTestUtilities
    {
        private readonly FormMainWindow _formMainWindow = new FormMainWindow();
        private TabControl _tabControlFiles;

        [TestInitialize]
        public void Setup()
        {
            var tabControlField = _formMainWindow.GetType()
                .GetField("tabControlFiles", BindingFlags.NonPublic | BindingFlags.Instance);
            _tabControlFiles = (TabControl)tabControlField?.GetValue(_formMainWindow);
        }

        [TestMethod]
        public void TestMethodCreateTab()
        {
            const string tabName = "newTab";
            var tab = Utilities.CreateTab(tabName);

            Assert.AreEqual(tabName, tab.Text);
            Assert.IsInstanceOfType(tab.Controls[0], typeof(ITextEditorControl));
        }

        [TestMethod]
        public void TestMethodGetFileNameFromTabControlFilePath()
        {
            var result = Utilities.GetFileNameFromTabControl(_tabControlFiles, true);
            Assert.AreEqual("new", result);
        }

        [TestMethod]
        public void TestMethodGetFileNameFromTabControlFullFilePath()
        {
            const string filePath = "/new/file.txt";
            ((TextEditorControl)_tabControlFiles.TabPages[0].Controls[0]).RichTextBoxEditor.FilePath = filePath;

            var result = Utilities.GetFileNameFromTabControl(_tabControlFiles, true);
            Assert.AreEqual(filePath, result);
        }

        [TestMethod]
        public void TestMethodGetFileNameFromTabControlFileName()
        {
            const string filePath = "/new/file.txt";
            ((TextEditorControl)_tabControlFiles.TabPages[0].Controls[0]).RichTextBoxEditor.FilePath = filePath;

            var result = Utilities.GetFileNameFromTabControl(_tabControlFiles, false);
            Assert.AreEqual(filePath.Substring(filePath.LastIndexOf('/') + 1), result);
        }
    }
}
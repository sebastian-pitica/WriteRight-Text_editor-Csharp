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
 *  Description: Fișierul conține funcți de test pentru funcționalitățile *
 *  moduluilui Utilities                                                  *               
 *                                                                        * 
 **************************************************************************/

namespace UnitTestProjectUtilities
{
    [TestClass]
    public class UnitTestUtilities
    {
        [TestMethod]
        public void TestMethodCreateTab()
        {
            string tabName = "newTab";
            TabPage tab = Utilities.CreateTab(tabName);

            Assert.AreEqual(tabName, tab.Text);
            Assert.IsInstanceOfType(tab.Controls[0], typeof(ITextEditorControl));
        }

        [TestMethod]
        public void TestMethodGetFileNameFromTabControl()
        {
            FormMainWindow formMainWindow = new FormMainWindow();
            var tabControlField = formMainWindow.GetType().GetField("tabControlFiles", BindingFlags.NonPublic | BindingFlags.Instance);
            var tabControlFiles = (TabControl)tabControlField.GetValue(formMainWindow);

            string result = Utilities.GetFileNameFromTabControl(tabControlFiles, true);

            Assert.AreEqual("new", result);

            var filePath = "/new/file.txt";
            ((TextEditorControl)tabControlFiles.TabPages[0].Controls[0]).RichTextBoxEditor.FilePath = filePath;

            result = Utilities.GetFileNameFromTabControl(tabControlFiles, true);
            Assert.AreEqual(filePath, result);

            result = Utilities.GetFileNameFromTabControl(tabControlFiles, false);
            Assert.AreEqual(filePath.Substring(filePath.LastIndexOf('/') + 1), result);
        }
    }
}

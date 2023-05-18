using CommonsModule;
using CustomControls;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using TextEditor;

/**************************************************************************
 *                                                                        *
 *  File:        UnitTestEditMenu.cs                                      *
 *  Copyright:   (c) 2023, Vasile Caulea, Matei Rares                     *
 *  Description: Fișierul conține funcții de test pentru funcționalitățile*
 *  moduluilui Utilities                                                  *
 *  Updated: Matei Rares                                                  *               
 *                                                                        * 
 **************************************************************************/

namespace UnitTestProjectUtilities
{
    [TestClass]
    public class UnitTestUtilities
    {
        private readonly FormMainWindow _formMainWindow = new FormMainWindow();
        private RichTextBoxV2 _mainTextBox;
        private TabControl _tabControlFiles;

        [TestInitialize]
        public void Setup()
        {
            FieldInfo tabControlField = _formMainWindow.GetType()
                .GetField("tabControlFiles", BindingFlags.NonPublic | BindingFlags.Instance);
            _tabControlFiles = (TabControl)tabControlField?.GetValue(_formMainWindow);

            FieldInfo mainTextBoxField = _formMainWindow.GetType().GetField("_richTextBoxMainV2", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mainTextBoxField != null) _mainTextBox = (RichTextBoxV2)mainTextBoxField.GetValue(_formMainWindow);

        }

        [TestMethod]
        public void TestMethodCreateTab()
        {
            const string tabName = "newTab";
            TabPage tab = Utilities.CreateTab(tabName);

            Assert.AreEqual(tabName, tab.Text);
            Assert.IsInstanceOfType(tab.Controls[0], typeof(ITextEditorControl));
        }

        [TestMethod]
        public void TestMethodGetFileNameFromTabControlFilePath()
        {
            string result = Utilities.GetFileNameFromTabControl(_tabControlFiles, true);
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

            string result = Utilities.GetFileNameFromTabControl(_tabControlFiles, false);
            Assert.AreEqual(filePath.Substring(filePath.LastIndexOf('/') + 1), result);
        }

        #region <creator>Matei Rares</creator>
        [TestMethod]
        public void TestMethodSelectionInBlock()
        {
            _mainTextBox.Text = "Acesta este un test menit sa /* afle daca selectia \n se afla intr-un comment block*/";
            _mainTextBox.SelectionStart = 35;
            Assert.AreEqual(true, UtilitiesFormat.IsSelectionInCommentBlock(_mainTextBox)) ;
        }

        [TestMethod]
        public void TestMethodEnterTab()
        {
            _mainTextBox.Text = "Acesta este un test menit adauge un tab { daca selectia \n se afla intr-un code block }";
            const string test = "Acesta este un test menit adauge un tab { \tdaca selectia \n se afla intr-un code block }";
            _mainTextBox.SelectionStart = 42;
            UtilitiesFormat.EnterTab(_mainTextBox);
            Assert.AreEqual(test, _mainTextBox.Text);
        }

        [TestMethod]
        public void TestMethodCommentUncomment()
        {
            _mainTextBox.Text ="Acesta este un test pentru comment, asa ca voi pune un coment block undeva";
            const string  test= "/*Acesta este un test pentru comment, asa ca voi pune un coment block undeva*/";
            _mainTextBox.SelectAll();
            
            UtilitiesFormat.CommentUncomment(_mainTextBox);

            Assert.AreEqual(test, _mainTextBox.Text);
        }

        [TestMethod]
        public void TestMethodFormatPaste()
        {
            _mainTextBox.Text = "Voi insera un text aici://; textul ar trebui sa aiba acelasi font ca cel din RichTextBox";
            _mainTextBox.SelectionStart=25;
            Font fontCurrent=_mainTextBox.Font;
            
            Clipboard.Clear();
            Clipboard.SetText("Un text format din 35 de caracteree");
           
            const string test = "Voi insera un text aici:/Un text format din 35 de caracteree/; textul ar trebui sa aiba acelasi font ca cel din RichTextBox";
            UtilitiesFormat.FormatPaste(_mainTextBox);
            
            Assert.AreEqual(test,_mainTextBox.Text);
            _mainTextBox.Select(25, 60);
            Assert.AreEqual(fontCurrent, _mainTextBox.SelectionFont);
        }

        [TestMethod]
        public void TestMethodCheckColor()
        {
            _mainTextBox.Text = "Un text pentru colorare: int , double , void , float ( spatiile dinaintea virgulelor sunt intentionate)";
            
            UtilitiesFormat.LinearHighLighting(_mainTextBox);
            
            _mainTextBox.Select(26, 28); 
            Color wordColor = _mainTextBox.SelectionColor;
            
            Assert.AreNotEqual(Color.Black,wordColor );
        }
        #endregion
    }
}
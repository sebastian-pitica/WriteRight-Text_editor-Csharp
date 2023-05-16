using CommonsModule;
using CustomControls;
using HelpRibbonModule;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TextEditor;

/**************************************************************************
 *                                                                        *
 *  File:        UnitTestHelpMenu.cs                                      *
 *  Copyright:   (c) 2023, Pitica Sebastian                               *
 *  Description: Fișierul conține funcții de test pentru funcționalitățile*
 *  moduluilui Help                                                       *
 *                                                                        * 
 **************************************************************************/


namespace UnitTestProjectHelpRibbonMenu
{
    [TestClass]
    public class UnitTestHelpMenu
    {
        private readonly FormMainWindow _formMainWindow = new FormMainWindow();
        private TextEditorControl _textEditorControl;
        private RichTextBoxV2 _mainTextBox;

        [TestInitialize]
        public void Setup()
        {
            var textEditorControlField = _formMainWindow.GetType()
                .GetField("_textEditorControl", BindingFlags.NonPublic | BindingFlags.Instance);
            if (textEditorControlField != null) _textEditorControl = (TextEditorControl)textEditorControlField.GetValue(_formMainWindow);

            var mainTextBoxField = _formMainWindow.GetType()
              .GetField("_richTextBoxMainV2", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mainTextBoxField != null) _mainTextBox = (RichTextBoxV2)mainTextBoxField.GetValue(_formMainWindow);
        }

        bool ExecuteCommand(SingletonCommand command)
        {
            command.Execute();
            var windowField = command.GetType()
               .GetField("_isVisible", BindingFlags.NonPublic | BindingFlags.Instance);
            var visible = (Boolean)windowField.GetValue(command);
            return visible;
        }

        [TestMethod]
        public void TestMethodReportBugPopUpCommand()
        {
            ReportBugCommand command = ReportBugCommand.GetCommandObj();
            Assert.AreEqual(true, ExecuteCommand(command));
        }

        [TestMethod]
        public void TestMethodHelpPopUpCommand()
        {
            HelpCommand command = HelpCommand.GetCommandObj();
            Assert.AreEqual(true, ExecuteCommand(command));
        }

        [TestMethod]
        public void TestMethodAboutPopUpCommand()
        {
            AboutCommand command = AboutCommand.GetCommandObj();
            Assert.AreEqual(true, ExecuteCommand(command));
        }

        float GetZoomFactor(RichTextBox richTextBox)
        {
            float originalWidth = richTextBox.ClientSize.Width;
            float currentWidth = richTextBox.GetPositionFromCharIndex(richTextBox.TextLength).X;
            return currentWidth / originalWidth;
        }

        [TestMethod]
        public void TestMethodZoomInCommand()
        {
            ZoomInCommand command = ZoomInCommand.GetCommandObj();
            command.SetTarget(_textEditorControl);
            var zoomFactorBefore = GetZoomFactor(_mainTextBox);
            command.Execute();
            var zoomFactorAfter = GetZoomFactor(_mainTextBox);
            Assert.IsTrue(zoomFactorBefore<=zoomFactorAfter);
        }

        [TestMethod]
        public void TestMethodZoomOutCommand()
        {
            ZoomOutCommand command = ZoomOutCommand.GetCommandObj();
            command.SetTarget(_textEditorControl);
            var zoomFactorBefore = GetZoomFactor(_mainTextBox);
            command.Execute();
            var zoomFactorAfter = GetZoomFactor(_mainTextBox);
            Assert.IsTrue(zoomFactorBefore >= zoomFactorAfter);
        }
    }
}

using CustomControls;
using EditRibbonModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using TextEditor;

/**************************************************************************
 *                                                                        *
 *  File:        UnitTestEditMenu.cs                                      *
 *  Copyright:   (c) 2023, Pitica Sebastian                               *
 *  Description: Fișierul conține funcții de test pentru funcționalitățile*
 *  moduluilui Edit                                                       *
 *                                                                        * 
 **************************************************************************/

namespace UnitTestProjectEditRibbonMenu
{
    [TestClass]
    public class UnitTestEditMenu
    {
        private readonly FormMainWindow _formMainWindow = new FormMainWindow();
        private RichTextBoxV2 _mainTextBox;

        private const string DefaultText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n" +
                                           "Vivamus interdum id quam vitae lobortis. Phasellus pulvinar aliquet quam in bibendum.\n" +
                                           "Aliquam nec felis lacus. Nam scelerisque velit neque, at vestibulum libero tempor eget.\n" +
                                           "Donec maximus commodo nisl. Vivamus semper pulvinar leo. Etiam ultricies est tellus, nec tempus nibh dignissim sed.";

        [TestInitialize]
        public void Setup()
        {
            var mainTextBoxField = _formMainWindow.GetType()
                .GetField("_richTextBoxMainV2", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mainTextBoxField != null) _mainTextBox = (RichTextBoxV2)mainTextBoxField.GetValue(_formMainWindow);
        }

        [TestMethod]
        public void TestMethodCutCommand()
        {
            //setez textul, selectez primele linii, le decupez
            _mainTextBox.Text = DefaultText;
            int firstCharOfThirdLine = _mainTextBox.GetFirstCharIndexFromLine(2);
            int lengthOfFirstTwoLines =
                firstCharOfThirdLine - _mainTextBox.GetFirstCharIndexFromLine(0);
            _mainTextBox.SelectionStart = 0;
            _mainTextBox.SelectionLength = lengthOfFirstTwoLines;
            CutCommand command = CutCommand.GetCommandObj();
            command.SetTarget(_mainTextBox);
            command.Execute();

            //acceasi operatie doar ca folosind alte functii
            var expectedText = DefaultText.Substring(firstCharOfThirdLine);
            Assert.AreEqual(expectedText, _mainTextBox.Text);
        }

        [TestMethod]
        public void TestMethodUndoCommand()
        {
            //setez textul, selectez primele linii, le decupez și fac undo
            _mainTextBox.Text = DefaultText;
            int firstCharOfThirdLine = _mainTextBox.GetFirstCharIndexFromLine(2);
            int lengthOfFirstTwoLines =
                firstCharOfThirdLine - _mainTextBox.GetFirstCharIndexFromLine(0);
            _mainTextBox.SelectionStart = 0;
            _mainTextBox.SelectionLength = lengthOfFirstTwoLines;
            CutCommand command1 = CutCommand.GetCommandObj();
            command1.SetTarget(_mainTextBox);
            command1.Execute();
            UndoCommand command2 = UndoCommand.GetCommandObj();
            command2.SetTarget(_mainTextBox);
            command2.Execute();

            Assert.AreEqual(DefaultText, _mainTextBox.Text);
        }

        [TestMethod]
        public void TestMethodURedoCommand()
        {
            //setez textul, selectez primele linii, le decupez, fac undo apoi redo
            _mainTextBox.Text = DefaultText;
            int firstCharOfThirdLine = _mainTextBox.GetFirstCharIndexFromLine(2);
            int lengthOfFirstTwoLines =
                firstCharOfThirdLine - _mainTextBox.GetFirstCharIndexFromLine(0);
            _mainTextBox.SelectionStart = 0;
            _mainTextBox.SelectionLength = lengthOfFirstTwoLines;
            CutCommand command1 = CutCommand.GetCommandObj();
            command1.SetTarget(_mainTextBox);
            command1.Execute();
            var expectedText = _mainTextBox.Text;
            UndoCommand command2 = UndoCommand.GetCommandObj();
            command2.SetTarget(_mainTextBox);
            command2.Execute();
            RedoCommand command3 = RedoCommand.GetCommandObj();
            command3.SetTarget(_mainTextBox);
            command3.Execute();

            Assert.AreEqual(expectedText, _mainTextBox.Text);
        }

        [TestMethod]
        public void TestMethodDeleteCommand()
        {
            //setez textul, selectez primele linii, le sterg
            _mainTextBox.Text = DefaultText;
            int firstCharOfThirdLine = _mainTextBox.GetFirstCharIndexFromLine(2);
            int lengthOfFirstTwoLines =
                firstCharOfThirdLine - _mainTextBox.GetFirstCharIndexFromLine(0);
            _mainTextBox.SelectionStart = 0;
            _mainTextBox.SelectionLength = lengthOfFirstTwoLines;
            DeleteCommand command = DeleteCommand.GetCommandObj();
            command.SetTarget(_mainTextBox);
            command.Execute();

            //acceasi operatie doar ca folosind alte functii
            var expectedText = DefaultText.Substring(firstCharOfThirdLine);
            Assert.AreEqual(expectedText, _mainTextBox.Text);
        }

        [TestMethod]
        public void TestMethodSearchAndReplacePopUpCommand()
        {
            SearchAndReplaceCommand command = SearchAndReplaceCommand.GetCommandObj();
            command.SetTarget(_mainTextBox);
            command.Execute();
            var windowField = command.GetType()
               .GetField("_isVisible", BindingFlags.NonPublic | BindingFlags.Instance);
            var visible = (bool)windowField.GetValue(command);
            Assert.AreEqual(true, visible);
        }

        [TestMethod]
        public void TestMethodSearchPopUpCommand()
        {
            SearchCommand command = SearchCommand.GetCommandObj();
            command.SetTarget(_mainTextBox);
            command.Execute();
            var windowField = command.GetType()
               .GetField("_isVisible", BindingFlags.NonPublic | BindingFlags.Instance);
            var visible = (bool)windowField.GetValue(command);
            Assert.AreEqual(true, visible);
        }
    }
}
using CommonsModule;
using EditRibbonModule;
using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using FileRibbonModule;

namespace TextEditor
{
    public partial class FormMainWindow : Form
    {
        private RichTextBoxV2 _richTextBoxMainV2;

        public FormMainWindow()
        {
            InitializeComponent();
            _richTextBoxMainV2 = new RichTextBoxV2();
            _richTextBoxMainV2.baseComponent = richTextBoxMain;
        }

        private void NewFileClick(object sender, EventArgs e)
        {
            ExecuteCommand(NewFileCommand.GetCommandObj());
        }

        private void OpenFileClick(object sender, EventArgs e)
        {
            ExecuteCommand(OpenFileCommand.GetCommandObj());
        }

        private void SaveFileClick(object sender, EventArgs e)
        {
            ExecuteCommand(SaveFileCommand.GetCommandObj());
        }

        private void NewWindowClick(object sender, EventArgs e)
        {
            ISingletonCommand command = OpenNewWindowCommand.GetCommandObj();
            command.Execute();
        }

        private void UndoClick(object sender, EventArgs e)
        {
            ExecuteCommand(UndoCommand.GetCommandObj());
        }

        private void RedoClick(object sender, EventArgs e)
        {
            ExecuteCommand(RedoCommand.GetCommandObj());
        }

        private void CutClick(object sender, EventArgs e)
        {
            ExecuteCommand(CutCommand.GetCommandObj());
        }

        private void CopyClick(object sender, EventArgs e)
        {
            ExecuteCommand(CopyCommand.GetCommandObj());
        }

        private void PasteClick(object sender, EventArgs e)
        {
            ExecuteCommand(PasteCommand.GetCommandObj());
        }

        private void DeleteClick(object sender, EventArgs e)
        {
            ExecuteCommand(DeleteCommand.GetCommandObj());
        }

        private void SearchClick(object sender, EventArgs e)
        {
            ExecuteCommand(SearchCommand.GetCommandObj());
        }

        private void SearchAndReplaceClick(object sender, EventArgs e)
        {
            ExecuteCommand(SearchAndReplaceCommand.GetCommandObj());
        }

        private void ReportBugClick(object sender, EventArgs e)
        {
            ExecuteCommand(ReportBugCommand.GetCommandObj());
        }

        private void ZoomOutClick(object sender, EventArgs e)
        {

        }

        private void ZoomInClick(object sender, EventArgs e)
        {

        }

        private void FormatDocumentClick(object sender, EventArgs e)
        {

        }

        private void ToggleCommentClick(object sender, EventArgs e)
        {

        }

        private void ColoringClick(object sender, EventArgs e)
        {

        }

        private void ColoringPreferencesClick(object sender, EventArgs e)
        {

        }

        private void FontClick(object sender, EventArgs e)
        {

        }

        private void SyntaxCheckerClick(object sender, EventArgs e)
        {

        }

        private void WordCountClick(object sender, EventArgs e)
        {

        }

        private void DocsClick(object sender, EventArgs e)
        {

        }

        private void AboutClick(object sender, EventArgs e)
        {

        }

        private void ExecuteCommand(IMainWindowCommand mainWindowCommand)
        {
            mainWindowCommand.SetTarget(this);
            mainWindowCommand.Execute();
        }

        private void ExecuteCommand(IMainTextBoxCommand mainTextBoxCommand)
        {
            mainTextBoxCommand.SetTarget(_richTextBoxMainV2);
            mainTextBoxCommand.Execute();
        }

        private void RichTextBoxMainTextChanged(object sender, EventArgs e)
        {
            textBoxLinesNr.Text = CountMainBoxLines().ToString();
            textBoxWordsNr.Text = CountMainBoxWords().ToString();
        }

        private int CountMainBoxLines()
        {
            return _richTextBoxMainV2.baseComponent.Lines.Length;
        }

        /// <summary>
        /// Sparge textul în funcție de caracterele din acolade pentru a identifica cuvintele individuale.
        /// Argumentul "StringSplitOptions.RemoveEmptyEntries" exclude toate intrările ce conțin doar caractere albe
        /// ce pot apărea neintenționat.
        /// </summary>
        /// <returns>Numarul de cuvinte</returns>
        private int CountMainBoxWords()
        {
            return _richTextBoxMainV2.baseComponent.Text.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReportBugClick(sender, e);
        }
    }

}

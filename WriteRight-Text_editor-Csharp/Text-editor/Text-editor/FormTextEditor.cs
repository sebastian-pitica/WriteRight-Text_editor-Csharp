using CommonsModule;
using EditRibbonModule;
using Interfaces;
using System;
using System.Windows.Forms;
using FileRibbonModule;
using System.Drawing;
using System.Runtime.InteropServices;
using WriteRight_Text_editor_Csharp.Properties;
using CustomControls;

namespace TextEditor
{
    public partial class FormMainWindow : Form
    {
        private RichTextBoxV2 _richTextBoxMainV2;
        private readonly string _windowTitle = "Editorescu";

        public FormMainWindow()
        {
            InitializeComponent();
            ExecuteCommand(NewFileCommand.GetCommandObj());
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

        private void CloseFileClick(object sender, EventArgs e)
        {
            ExecuteCommand(CloseFileCommand.GetCommandObj());
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
            //ExecuteCommand(SyntaxCheckerCommand.GetCommandObj());
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

        private void ExecuteCommand(ITabControlCommand tabControlCommand)
        {
            tabControlCommand.SetTarget(tabControlFiles);
            tabControlCommand.Execute();
        }

        private void RichTextBoxMainTextChanged(object sender, EventArgs e)
        {
            textBoxLinesNr.Text = CountMainBoxLines().ToString();
            textBoxWordsNr.Text = CountMainBoxWords().ToString();
        }

        private int CountMainBoxLines()
        {
            return _richTextBoxMainV2.Lines.Length;
        }

        /// <summary>
        /// Sparge textul în funcție de caracterele din acolade pentru a identifica cuvintele individuale.
        /// Argumentul "StringSplitOptions.RemoveEmptyEntries" exclude toate intrările ce conțin doar caractere albe
        /// ce pot apărea neintenționat.
        /// </summary>
        /// <returns>Numarul de cuvinte</returns>
        private int CountMainBoxWords()
        {
            return _richTextBoxMainV2.Text.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        private void TabControlFilesDrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                e.Graphics.FillRectangle(new SolidBrush(tabControlFiles.BackColor), e.Bounds);
                TabPage tabPage = tabControlFiles.TabPages[e.Index];

                Rectangle tabRect = tabControlFiles.GetTabRect(e.Index);
                
                TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font,
                    tabRect, tabPage.ForeColor, TextFormatFlags.Left);
                
                var closeImage = Resources.CloseButton;
                e.Graphics.DrawImage(closeImage,
                (tabRect.Right - closeImage.Width),
                tabRect.Top + (tabRect.Height - closeImage.Height) / 2);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void TabControlFilesMouseDown(object sender, MouseEventArgs e)
        {
            for (var i = 0; i < tabControlFiles.TabPages.Count; i++)
            {
                var tabRect = tabControlFiles.GetTabRect(i);
                var closeImage = Resources.CloseButton;
                var imageRect = new Rectangle(
                    (tabRect.Right - closeImage.Width),
                    tabRect.Top + (tabRect.Height - closeImage.Height) / 2,
                    closeImage.Width,
                    closeImage.Height);
                if (imageRect.Contains(e.Location))
                {
                    tabControlFiles.SelectedIndex = i;
                    ExecuteCommand(CloseFileCommand.GetCommandObj());
                    break;
                }
            }
        }

        private void TabControlFilesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlFiles.SelectedIndex == -1)
            {
                ExecuteCommand(NewFileCommand.GetCommandObj());
                return;
            }
            SetMainTextBoxReference();
            SetWindowTitle();
        }

        private void TabControlFilesControlAdded(object sender, ControlEventArgs e)
        {
            SetMainTextBoxReference();
            SetWindowTitle();
        }

        /// <summary>
        /// Functia preia elementul RichTextBox din noul tab accesat pentru a pastra referinta la acesta
        /// </summary>
        private void SetMainTextBoxReference()
        {
            RichTextBoxV2 reference = Utilities.GetRichTextBoxV2FTabControl(tabControlFiles);
            _richTextBoxMainV2 = reference;
        }

        private void SetWindowTitle()
        {
            string titleFileName = Utilities.GetFileNameFromTabControl(tabControlFiles);
            this.Text = titleFileName + " - " + _windowTitle;
        }
    }
}

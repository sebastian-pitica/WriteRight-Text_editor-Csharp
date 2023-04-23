using CommonsModule;
using EditRibbonModule;
using Interfaces;
using System;
using System.Windows.Forms;
using FileRibbonModule;
using System.Drawing;
using WriteRight_Text_editor_Csharp.Properties;
using CustomControls;
using static CommonsModule.UtilitiesFormat;
using FormatRibbonModule;

namespace TextEditor
{
    public partial class FormMainWindow : Form
    {
        private RichTextBoxV2 _richTextBoxMainV2;
        private TextEditorControl _textEditorControl;
        private readonly string _windowTitle = "Editorescu";

        public FormMainWindow()
        {
            InitializeComponent();
            ExecuteCommand(NewFileCommand.GetCommandObj());
            ExecuteCommand(ThemeCommand.GetCommandObj());
            ExecuteCommand(ThemeCommand.GetCommandObj());
            _textEditorControl.ZoomFactor = 1.50f;
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
            if (_textEditorControl.ZoomFactor > 0.25f)
            {
                _textEditorControl.ZoomFactor -= 0.25f;
                _textEditorControl.SplitterDistance -= 3;
            }
        }

        private void ZoomInClick(object sender, EventArgs e)
        {
            if (_textEditorControl.ZoomFactor < 4.25f)
            {
                _textEditorControl.ZoomFactor += 0.25f;
                _textEditorControl.SplitterDistance += 3;
            }
        }

        private void FormatDocumentClick(object sender, EventArgs e)
        {
            ExecuteCommand(FormatDocument.GetCommandObj());
            CompleteHighlight(_richTextBoxMainV2);
        }

        private void ToggleCommentClick(object sender, EventArgs e)
        {
            CommentUncomment(_richTextBoxMainV2);
        }

        private void ThemeClick(object sender, EventArgs e)
        {
            ExecuteCommand(ThemeCommand.GetCommandObj());
        }

        private void SyntaxHighlighClick(object sender, EventArgs e)
        {
            ExecuteCommand(SyntaxHighlightCommand.GetCommandObj());
        }

        private void FontClick(object sender, EventArgs e)
        {
            ExecuteCommand(FontCommand.GetCommandObj());
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
                TabPage tabPage = tabControlFiles.TabPages[e.Index];

                Rectangle tabRect = tabControlFiles.GetTabRect(e.Index);
                e.Graphics.FillRectangle(new SolidBrush(tabPage.BackColor), tabRect);

                tabRect.Inflate(-5, -2);
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
            SetTextEditorReference();
            SetWindowTitle();
        }

        private void TabControlFilesControlAdded(object sender, ControlEventArgs e)
        {
            SetMainTextBoxReference();
            SetTextEditorReference();
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

        /// <summary>
        /// 
        /// </summary>
        private void SetTextEditorReference()
        {
            TextEditorControl reference = Utilities.GetTextEditorControlFTabControl(tabControlFiles);
            _textEditorControl = reference;
        }

        private void SetWindowTitle()
        {
            string titleFileName = Utilities.GetFileNameFromTabControl(tabControlFiles);
            Text = titleFileName + " - " + _windowTitle;
        }

        private void RichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            tabControlFiles.SelectedTab.BorderStyle = BorderStyle.None;

            //_richTextBoxMainV2.AcceptsTab = true; 

            if (e.Control && e.KeyCode == Keys.V)
            {
                e.Handled = true;
                FormatPaste(_richTextBoxMainV2);

                if (_richTextBoxMainV2.FileType == ".cpp" || _richTextBoxMainV2.FileType == ".cs" || _richTextBoxMainV2.FileType == ".c")
                {

                    CompleteHighlight(_richTextBoxMainV2);
                }
            }
        }

        /// <summary>
        ///  Aplica functiile dupa ce s-a terminat de apasat butonul si s-au facut schimbarile in textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RichTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (_richTextBoxMainV2.FileType == ".cpp" || _richTextBoxMainV2.FileType == ".cs" || _richTextBoxMainV2.FileType == ".c")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    EnterTab(_richTextBoxMainV2);
                }
                if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Back || e.KeyCode == Keys.Tab)
                {
                    _richTextBoxMainV2.HideSelection = true;
                    this.ActiveControl = tabControlFiles;
                    LiniarHighLighting(_richTextBoxMainV2);
                    _richTextBoxMainV2.Select();
                }
            }
            textBoxLinesNr.Text = CountMainBoxLines().ToString();
            textBoxWordsNr.Text = CountMainBoxWords().ToString();
        }
    }
}

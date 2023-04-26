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
using HelpModule;
using static CommonsModule.Utilities;

/**************************************************************************
 *                                                                        *
 *  File:        FormTextEditor.cs                                        *
 *  Copyright:                                                            *
 *  Description: Fișierul conține codul interfeței grafice pentru         *
 *  proiectul WriteRight.                                                 *
 *  Designed by: Pitica Sebastian                                         *
 *  Updated by: Pitica Sebastian, Caulea Vasile                           *
 *                                                                        *
 **************************************************************************/

namespace TextEditor
{
    public partial class FormMainWindow : Form, IObserver
    {
        private RichTextBoxV2 _richTextBoxMainV2;
        private TextEditorControl _textEditorControl;
        private const string WindowTitle = "WriteRight";

        public FormMainWindow()
        {
            ExecuteInitCommands();
        }

        #region Simple command objects calls
        #region <creator>Pitica Sebastian</creator>
        private void NewFileClick(object sender, EventArgs e)
        {
            ExecuteCommand(NewFileCommand.GetCommandObj());
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
            ExecuteCommand(OpenNewWindowCommand.GetCommandObj());
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

        private void SyntaxHighlightClick(object sender, EventArgs e)
        {
            ExecuteCommand(SyntaxHighlightCommand.GetCommandObj());
        }

        private void FontClick(object sender, EventArgs e)
        {
            ExecuteCommand(FontCommand.GetCommandObj());
        }

        private void DocsClick(object sender, EventArgs e)
        {
            ExecuteCommand(HelpCommand.GetCommandObj());
        }

        private void AboutClick(object sender, EventArgs e)
        {
            ExecuteCommand(AboutCommand.GetCommandObj());
        }

        private void ZoomOutClick(object sender, EventArgs e)
        {
            ExecuteCommand(ZoomOutCommand.GetCommandObj());
        }

        private void ZoomInClick(object sender, EventArgs e)
        {
            ExecuteCommand(ZoomInCommand.GetCommandObj());
        }
        #endregion
        #endregion

        #region Complex command objects calls
        private void OpenFileClick(object sender, EventArgs e)
        {
            SetStatus(Loading);
            ExecuteCommand(OpenFileCommand.GetCommandObj());
            if (_richTextBoxMainV2.FileType == ".cpp" || _richTextBoxMainV2.FileType == ".cs" || _richTextBoxMainV2.FileType == ".c")
            {
                CompleteHighlight(_richTextBoxMainV2);
            }
            SetStatus(Ready);
        }

        private void FormatDocumentClick(object sender, EventArgs e)
        {
            SetStatus(Loading);
            ExecuteCommand(FormatDocument.GetCommandObj());
            CompleteHighlight(_richTextBoxMainV2);
            SetStatus(Ready);
        }

        private void ToggleCommentClick(object sender, EventArgs e)
        {
            CommentUncomment(_richTextBoxMainV2);
        }

        private void ThemeClick(object sender, EventArgs e)
        {
            SetStatus(Loading);
            ExecuteCommand(ThemeCommand.GetCommandObj());
            if (_richTextBoxMainV2.FileType == ".cpp" || _richTextBoxMainV2.FileType == ".cs" || _richTextBoxMainV2.FileType == ".c")
            {
                CompleteHighlight(_richTextBoxMainV2);
            }
            SetStatus(Ready);
        }
        #endregion

        #region Execute command function series
        #region <creator>Pitica Sebastian</creator>
        private void ExecuteCommand(MainWindowCommand mainWindowCommand)
        {
            mainWindowCommand.SetTarget(this);
            mainWindowCommand.Execute();
        }

        private void ExecuteCommand(MainTextBoxCommand mainTextBoxCommand)
        {
            mainTextBoxCommand.SetTarget(_richTextBoxMainV2);
            mainTextBoxCommand.Execute();
        }

        private void ExecuteCommand(TabControlCommand tabControlCommand)
        {
            tabControlCommand.SetTarget(tabControlFiles);
            tabControlCommand.Execute();
        }

        #region <updated>Caulea Vasile</updated>

        private void ExecuteCommand(TextEditorControlCommand textEditorControlCommand)
        {
            textEditorControlCommand.SetTarget(_textEditorControl);
            textEditorControlCommand.Execute();
        }
        
        #endregion
        
        private void ExecuteCommand(SingletonCommand singletonCommand)
        {
            singletonCommand.Execute();
        }

        private void ExecuteInitCommands()
        {
            InitializeComponent();
            ExecuteCommand(NewFileCommand.GetCommandObj());
            ExecuteCommand(ThemeCommand.GetCommandObj());
            ExecuteCommand(ThemeCommand.GetCommandObj());
            _textEditorControl.ZoomFactor = 1.50f;
        }
        #endregion
        #endregion

        #region Other functions
        #region <creator>Pitica Sebastian</creator>

        private void SetStatus(string status)
        {
            toolStripStatusLabel.Text = status;
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

        private void PrintCounts()
        {
            textBoxLinesNr.Text = CountMainBoxLines().ToString();
            textBoxWordsNr.Text = CountMainBoxWords().ToString();
        }
        #endregion

        #region <creator>Vasile</creator>

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
                if (!imageRect.Contains(e.Location)) continue;
                tabControlFiles.SelectedIndex = i;
                ExecuteCommand(CloseFileCommand.GetCommandObj());
                break;
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
            SetTitles();
        }

        private void TabControlFilesControlAdded(object sender, ControlEventArgs e)
        {
            SetMainTextBoxReference();
            SetTextEditorReference();
            SetTitles();
        }

        /// <summary>
        /// Functia preia elementul RichTextBoxV2 din noul tab accesat pentru a pastra referinta la acesta
        /// </summary>
        private void SetMainTextBoxReference()
        {
            RichTextBoxV2 reference = GetRichTextBoxV2FromTabControl(tabControlFiles);
            _richTextBoxMainV2 = reference;
            _richTextBoxMainV2.Attach(this);
        }

        /// <summary>
        /// Functia preia elementul TextEditorControl din noul tab accesat pentru a pastra referinta la acesta
        /// </summary>
        private void SetTextEditorReference()
        {
            TextEditorControl reference = GetTextEditorControlFromTabControl(tabControlFiles);
            _textEditorControl = reference;
        }

        /// <summary>
        /// Functia seteaza titlul ferestrei curente si titlul tab-ului selectat.
        /// </summary>
        private void SetTitles()
        {
            bool saved = _richTextBoxMainV2.IsSaved;

            string titleFileName = ((!saved) ? "*" : "") + GetFileNameFromTabControl(tabControlFiles, true);
            string titleTab = ((!saved) ? "* " : "") + GetFileNameFromTabControl(tabControlFiles, false);

            Text = titleFileName + " - " + WindowTitle;
            tabControlFiles.SelectedTab.Text = titleTab;
        }

        public void UpdateObserver()
        {
            SetTitles();
        }

        #endregion

        private void RichTextBoxKeyDown(object sender, KeyEventArgs e)
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
            PrintCounts();
        }

        /// <summary>
        ///  Aplica functiile dupa ce s-a terminat de apasat butonul si s-au facut schimbarile in textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RichTextBoxKeyUp(object sender, KeyEventArgs e)
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
                    ActiveControl = tabControlFiles;
                    LiniarHighLighting(_richTextBoxMainV2);
                    _richTextBoxMainV2.Select();
                }
            }
            PrintCounts();
        }

        #endregion

    }
}

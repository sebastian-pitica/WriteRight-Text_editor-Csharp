using Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;
using CustomControls;
using static CommonsModule.Utilities;

/**************************************************************************
 *                                                                        *
 *  File:        EditRibbonModule.cs                                      *
 *  Copyright:   (c) 2023, Pitica Sebastian                               *
 *  Description: Fișierul conține obiectele de tip Singleton-Command      *
 *  aferente ribbonului Edit, separate pentru a se putea lucra individual *
 *  de celelalte ribbonuri.                                               *
 *                                                                        *
 **************************************************************************/


namespace EditRibbonModule
{

    public class UndoCommand : MainTextBoxCommand
    {
        private static UndoCommand _singletonInstance;
        private RichTextBoxV2 _mainTextBoxRef;

        private UndoCommand() { }

        public new static UndoCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new UndoCommand());
        }

        public override void SetTarget(IRichTextBoxV2 textBox)
        {
            _mainTextBoxRef =(RichTextBoxV2) textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.Undo();
        }
    }

    public class RedoCommand : MainTextBoxCommand
    {
        private static RedoCommand _singletonInstance;
        private RichTextBoxV2 _mainTextBoxRef;

        private RedoCommand() { }

        public new static RedoCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new RedoCommand());
        }

        public override void SetTarget(IRichTextBoxV2 textBox)
        {
            _mainTextBoxRef = (RichTextBoxV2)textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.Redo();
        }
    }

    public class CutCommand : MainTextBoxCommand
    {
        private static CutCommand _singletonInstance;
        private RichTextBoxV2 _mainTextBoxRef;

        private CutCommand() { }

        public new static CutCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new CutCommand());
        }

        public override void SetTarget(IRichTextBoxV2 textBox)
        {
            _mainTextBoxRef = (RichTextBoxV2)textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.Cut();
        }
    }

    public class CopyCommand : MainTextBoxCommand
    {
        private static CopyCommand _singletonInstance;
        private RichTextBoxV2 _mainTextBoxRef;

        private CopyCommand() { }

        public new static CopyCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new CopyCommand());
        }

        public override void SetTarget(IRichTextBoxV2 textBox)
        {
            _mainTextBoxRef = (RichTextBoxV2)textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.Copy();
        }
    }

    public class PasteCommand : MainTextBoxCommand
    {
        private static PasteCommand _singletonInstance;
        private RichTextBoxV2 _mainTextBoxRef;

        private PasteCommand() { }

        public new static PasteCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new PasteCommand());
        }

        public override void SetTarget(IRichTextBoxV2 textBox)
        {
            _mainTextBoxRef = (RichTextBoxV2)textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.Paste();
        }
    }

    public class DeleteCommand : MainTextBoxCommand
    {
        private static DeleteCommand _singletonInstance;
        private RichTextBoxV2 _mainTextBoxRef;

        private DeleteCommand() { }

        public new static DeleteCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new DeleteCommand());
        }

        public override void SetTarget(IRichTextBoxV2 textBox)
        {
            _mainTextBoxRef = (RichTextBoxV2)textBox;
        }

        public override void Execute()
        {
            _mainTextBoxRef.SelectedText = "";
        }
    }

    public class SearchAndReplaceCommand : MainTextBoxCommand
    {
        private static SearchAndReplaceCommand _singletonInstance;
        private RichTextBoxV2 _mainTextBoxRef;
        private int _indexReplaceNext;
        private string _lastSearchedText;
        private Boolean _isVisible=false;
        private SearchAndReplaceCommand() { }

        public new static SearchAndReplaceCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new SearchAndReplaceCommand());
        }

        public override void SetTarget(IRichTextBoxV2 textBox)
        {
            this._mainTextBoxRef = (RichTextBoxV2)textBox;
        }

        public override void Execute()
        {
            // Creez fereastra pentru preluarea inputului
            Form windowInputData = new Form
            {
                Text = "Search",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(20, 10, 20, 10)
            };
            // Creaza un panou pentru atasarea elementelor grafice
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
            };

            Label labelSearchText = new Label
            {
                Text = "Search Text:",
                Margin = new Padding(0),
            };

            Label labelReplaceText = new Label
            {
                Text = "Replace Text:",
                Margin = new Padding(0)
            };

            TextBox textBoxSearchText = new TextBox
            {
                Width = 200,
                Margin = new Padding(0, 0, 0, 10)
            };

            TextBox textBoxReplaceText = new TextBox
            {
                Width = 200,
                Margin = new Padding(0, 0, 0, 10)
            };

            Button buttonReplaceAll = new Button
            {
                Text = "Replace All",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            Button buttonReplaceNext = new Button
            {
                Text = "Replace Next",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            Button buttonCancel = new Button
            {
                Text = "Cancel",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            buttonReplaceAll.Click += (sender, args) =>
            {
                try
                {
                    string searchWord = textBoxSearchText.Text;
                    string replaceText = textBoxReplaceText.Text;

                    if (searchWord == string.Empty)
                        return;

                    int index = 0;
                    while ((index = _mainTextBoxRef.Text.IndexOf(searchWord, index, StringComparison.Ordinal)) != -1)
                    {
                        _mainTextBoxRef.Select(index, searchWord.Length);
                        _mainTextBoxRef.SelectedText = replaceText;
                        index += replaceText.Length;
                    }
                }
                catch (Exception e)
                {
                    HandleException(e);
                }

            };

            buttonReplaceNext.Click += (sender, args) =>
            {
                try
                {
                    string searchWord = textBoxSearchText.Text;
                    string replaceText = textBoxReplaceText.Text;

                    if (searchWord == string.Empty)
                        return;

                    // verificam daca s-a introdus alt text de cautare pentru a reinitializa indexul de cautare
                    if (_lastSearchedText == null)
                    {
                        _lastSearchedText = string.Copy(searchWord);
                    }
                    else
                    {
                        if (!_lastSearchedText.Equals(searchWord))
                        {
                            _indexReplaceNext = 0;
                        }
                    }

                    if (_indexReplaceNext == -1) return;
                    _indexReplaceNext = _mainTextBoxRef.Text.IndexOf(searchWord, _indexReplaceNext, StringComparison.Ordinal);

                    if (_indexReplaceNext == -1) return;
                    _mainTextBoxRef.Select(_indexReplaceNext, searchWord.Length);
                    _mainTextBoxRef.SelectedText = replaceText;
                    _indexReplaceNext += replaceText.Length;
                }
                catch (Exception e)
                {
                    HandleException(e);
                }
            };

            buttonCancel.Click += (sender, args) =>
            {
                _indexReplaceNext = 0;
                windowInputData.Close();
            };

            flowLayoutPanel.Controls.Add(labelSearchText);
            flowLayoutPanel.Controls.Add(textBoxSearchText);
            flowLayoutPanel.Controls.Add(labelReplaceText);
            flowLayoutPanel.Controls.Add(textBoxReplaceText);
            flowLayoutPanel.Controls.Add(buttonReplaceAll);
            flowLayoutPanel.Controls.Add(buttonReplaceNext);
            flowLayoutPanel.Controls.Add(buttonCancel);

            //Adaug panoul in fereastra si afisez fereastra
            windowInputData.Controls.Add(flowLayoutPanel);
            windowInputData.ShowDialog();
            _isVisible = true;
        }
    }

    public class SearchCommand : MainTextBoxCommand
    {
        private static SearchCommand _singletonInstance;
        private RichTextBoxV2 _mainTextBoxRef;
        private Boolean _isVisible=false;


        private SearchCommand() { }

        public new static SearchCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new SearchCommand());
        }

        public override void SetTarget(IRichTextBoxV2 mainTextBox)
        {
            _mainTextBoxRef = (RichTextBoxV2)mainTextBox;
        }

        public override void Execute()
        {
            // Creez fereastra pentru preluarea inputului
            Form inputDataForm = new Form
            {
                Text = "Search",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(20, 10, 20, 10)
            };

            // Creaza un panou pentru atasarea elementelor grafice
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,

            };

            Label labelSearchText = new Label
            {
                Text = "Search Text:",
                Margin = new Padding(0)
            };

            TextBox textBoxSearchText = new TextBox
            {
                Width = 200,
                Margin = new Padding(0, 0, 0, 10)
            };

            Button buttonSearch = new Button
            {
                Text = "Search",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            Button buttonCancel = new Button
            {
                Text = "Cancel",
                Width = 100,
                Margin = new Padding(0),
                Anchor = AnchorStyles.None
            };

            buttonSearch.Click += (sender, args) =>
            {
                try
                {
                    string searchWord = textBoxSearchText.Text;

                    if (searchWord == string.Empty)
                        return;

                    int index = 0;
                    while ((index = _mainTextBoxRef.Text.IndexOf(searchWord, index, StringComparison.Ordinal)) != -1)
                    {
                        _mainTextBoxRef.Select(index, searchWord.Length);
                        _mainTextBoxRef.SelectionBackColor = Color.Yellow;
                        index += searchWord.Length;
                    }
                }
                catch (Exception e)
                {
                   HandleException(e);
                }
            };

            buttonCancel.Click += (sender, args) =>
            {
                inputDataForm.Close();
            };

            flowLayoutPanel.Controls.Add(labelSearchText);
            flowLayoutPanel.Controls.Add(textBoxSearchText);
            flowLayoutPanel.Controls.Add(buttonSearch);
            flowLayoutPanel.Controls.Add(buttonCancel);

            // Adaug panoul in fereastra si afisez fereastra
            inputDataForm.Controls.Add(flowLayoutPanel);
            inputDataForm.ShowDialog();
            _isVisible = true;
        }
    }

}



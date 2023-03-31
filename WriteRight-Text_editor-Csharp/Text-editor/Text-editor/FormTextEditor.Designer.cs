namespace TextEditor
{
    partial class FormMainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMainWindow));
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonNew = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonSave = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonNewWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonCut = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonSearchAndReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonFormatDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonToggleComment = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.buttoColoring = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonColoringPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonFont = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.syntaxCheckerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonDocs = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonReportBug = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripRibbons = new System.Windows.Forms.MenuStrip();
            this.richTextBoxMain = new System.Windows.Forms.RichTextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.textBoxWordsNr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLinesNr = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.menuStripRibbons.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonNew,
            this.buttonOpen,
            this.buttonSave,
            this.buttonNewWindow});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(41, 23);
            this.menuFile.Text = "File";
            // 
            // buttonNew
            // 
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(159, 24);
            this.buttonNew.Text = "New";
            this.buttonNew.Click += new System.EventHandler(this.NewFileClick);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(159, 24);
            this.buttonOpen.Text = "Open";
            this.buttonOpen.Click += new System.EventHandler(this.OpenFileClick);
            // 
            // buttonSave
            // 
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(159, 24);
            this.buttonSave.Text = "Save";
            this.buttonSave.Click += new System.EventHandler(this.SaveFileClick);
            // 
            // buttonNewWindow
            // 
            this.buttonNewWindow.Name = "buttonNewWindow";
            this.buttonNewWindow.Size = new System.Drawing.Size(159, 24);
            this.buttonNewWindow.Text = "New Window";
            this.buttonNewWindow.Click += new System.EventHandler(this.NewWindowClick);
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonUndo,
            this.buttonRedo,
            this.toolStripSeparator1,
            this.buttonCut,
            this.buttonCopy,
            this.buttonPaste,
            this.buttonDelete,
            this.toolStripSeparator2,
            this.buttonSearch,
            this.buttonSearchAndReplace,
            this.toolStripSeparator3,
            this.buttonFormatDocument,
            this.buttonToggleComment});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(44, 23);
            this.menuEdit.Text = "Edit";
            // 
            // buttonUndo
            // 
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(220, 24);
            this.buttonUndo.Text = "Undo";
            this.buttonUndo.Click += new System.EventHandler(this.UndoClick);
            // 
            // buttonRedo
            // 
            this.buttonRedo.Name = "buttonRedo";
            this.buttonRedo.Size = new System.Drawing.Size(220, 24);
            this.buttonRedo.Text = "Redo";
            this.buttonRedo.Click += new System.EventHandler(this.RedoClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(217, 6);
            // 
            // buttonCut
            // 
            this.buttonCut.Name = "buttonCut";
            this.buttonCut.Size = new System.Drawing.Size(220, 24);
            this.buttonCut.Text = "Cut";
            this.buttonCut.Click += new System.EventHandler(this.CutClick);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(220, 24);
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.Click += new System.EventHandler(this.CopyClick);
            // 
            // buttonPaste
            // 
            this.buttonPaste.Name = "buttonPaste";
            this.buttonPaste.Size = new System.Drawing.Size(220, 24);
            this.buttonPaste.Text = "Paste";
            this.buttonPaste.Click += new System.EventHandler(this.PasteClick);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(220, 24);
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.Click += new System.EventHandler(this.DeleteClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(217, 6);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(220, 24);
            this.buttonSearch.Text = "Search";
            this.buttonSearch.Click += new System.EventHandler(this.SearchClick);
            // 
            // buttonSearchAndReplace
            // 
            this.buttonSearchAndReplace.Name = "buttonSearchAndReplace";
            this.buttonSearchAndReplace.Size = new System.Drawing.Size(220, 24);
            this.buttonSearchAndReplace.Text = "Search and Replace";
            this.buttonSearchAndReplace.Click += new System.EventHandler(this.SearchAndReplaceClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(217, 6);
            // 
            // buttonFormatDocument
            // 
            this.buttonFormatDocument.Name = "buttonFormatDocument";
            this.buttonFormatDocument.Size = new System.Drawing.Size(220, 24);
            this.buttonFormatDocument.Text = "Format Document";
            this.buttonFormatDocument.Click += new System.EventHandler(this.FormatDocumentClick);
            // 
            // buttonToggleComment
            // 
            this.buttonToggleComment.Name = "buttonToggleComment";
            this.buttonToggleComment.Size = new System.Drawing.Size(220, 24);
            this.buttonToggleComment.Text = "Comment/Uncomment";
            this.buttonToggleComment.Click += new System.EventHandler(this.ToggleCommentClick);
            // 
            // menuFormat
            // 
            this.menuFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttoColoring,
            this.buttonColoringPreferences,
            this.buttonFont});
            this.menuFormat.Name = "menuFormat";
            this.menuFormat.Size = new System.Drawing.Size(65, 23);
            this.menuFormat.Text = "Format";
            // 
            // buttoColoring
            // 
            this.buttoColoring.Name = "buttoColoring";
            this.buttoColoring.Size = new System.Drawing.Size(204, 24);
            this.buttoColoring.Text = "Coloring";
            this.buttoColoring.Click += new System.EventHandler(this.ColoringClick);
            // 
            // buttonColoringPreferences
            // 
            this.buttonColoringPreferences.Name = "buttonColoringPreferences";
            this.buttonColoringPreferences.Size = new System.Drawing.Size(204, 24);
            this.buttonColoringPreferences.Text = "Coloring Preferences";
            this.buttonColoringPreferences.Click += new System.EventHandler(this.ColoringPreferencesClick);
            // 
            // buttonFont
            // 
            this.buttonFont.Name = "buttonFont";
            this.buttonFont.Size = new System.Drawing.Size(204, 24);
            this.buttonFont.Text = "Font";
            this.buttonFont.Click += new System.EventHandler(this.FontClick);
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.syntaxCheckerToolStripMenuItem,
            this.wordCountToolStripMenuItem});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(70, 23);
            this.menuOptions.Text = "Options";
            // 
            // syntaxCheckerToolStripMenuItem
            // 
            this.syntaxCheckerToolStripMenuItem.Name = "syntaxCheckerToolStripMenuItem";
            this.syntaxCheckerToolStripMenuItem.Size = new System.Drawing.Size(171, 24);
            this.syntaxCheckerToolStripMenuItem.Text = "Syntax Checker";
            this.syntaxCheckerToolStripMenuItem.Click += new System.EventHandler(this.SyntaxCheckerClick);
            // 
            // wordCountToolStripMenuItem
            // 
            this.wordCountToolStripMenuItem.Name = "wordCountToolStripMenuItem";
            this.wordCountToolStripMenuItem.Size = new System.Drawing.Size(171, 24);
            this.wordCountToolStripMenuItem.Text = "Word Count";
            this.wordCountToolStripMenuItem.Click += new System.EventHandler(this.WordCountClick);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonDocs,
            this.buttonReportBug,
            this.buttonAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(49, 23);
            this.menuHelp.Text = "Help";
            // 
            // buttonDocs
            // 
            this.buttonDocs.Name = "buttonDocs";
            this.buttonDocs.Size = new System.Drawing.Size(151, 24);
            this.buttonDocs.Text = "Docs";
            this.buttonDocs.Click += new System.EventHandler(this.DocsClick);
            // 
            // buttonReportBug
            // 
            this.buttonReportBug.Name = "buttonReportBug";
            this.buttonReportBug.Size = new System.Drawing.Size(151, 24);
            this.buttonReportBug.Text = "Report  Bug";
            this.buttonReportBug.Click += new System.EventHandler(this.ReportBugClick);
            // 
            // buttonAbout
            // 
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(151, 24);
            this.buttonAbout.Text = "About";
            this.buttonAbout.Click += new System.EventHandler(this.AboutClick);
            // 
            // menuStripRibbons
            // 
            this.menuStripRibbons.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStripRibbons.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripRibbons.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuFormat,
            this.menuOptions,
            this.menuHelp});
            this.menuStripRibbons.Location = new System.Drawing.Point(0, 0);
            this.menuStripRibbons.Name = "menuStripRibbons";
            this.menuStripRibbons.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStripRibbons.Size = new System.Drawing.Size(940, 27);
            this.menuStripRibbons.TabIndex = 1;
            // 
            // richTextBoxMain
            // 
            this.richTextBoxMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxMain.Location = new System.Drawing.Point(30, 32);
            this.richTextBoxMain.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxMain.Name = "richTextBoxMain";
            this.richTextBoxMain.Size = new System.Drawing.Size(898, 450);
            this.richTextBoxMain.TabIndex = 2;
            this.richTextBoxMain.Text = "";
            this.richTextBoxMain.TextChanged += new System.EventHandler(this.RichTextBoxMainTextChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 487);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(940, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripProgressBar.Size = new System.Drawing.Size(200, 16);
            this.toolStripProgressBar.Visible = false;
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripSplitButton1.Size = new System.Drawing.Size(23, 23);
            this.toolStripSplitButton1.Text = "+";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonZoomIn.AutoSize = true;
            this.buttonZoomIn.Location = new System.Drawing.Point(889, 485);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(24, 25);
            this.buttonZoomIn.TabIndex = 6;
            this.buttonZoomIn.Text = "+";
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            this.buttonZoomIn.Click += new System.EventHandler(this.ZoomInClick);
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonZoomOut.AutoSize = true;
            this.buttonZoomOut.Location = new System.Drawing.Point(861, 485);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(23, 25);
            this.buttonZoomOut.TabIndex = 9;
            this.buttonZoomOut.Text = "-";
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            this.buttonZoomOut.Click += new System.EventHandler(this.ZoomOutClick);
            // 
            // textBoxWordsNr
            // 
            this.textBoxWordsNr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWordsNr.Location = new System.Drawing.Point(748, 487);
            this.textBoxWordsNr.Name = "textBoxWordsNr";
            this.textBoxWordsNr.ReadOnly = true;
            this.textBoxWordsNr.Size = new System.Drawing.Size(51, 20);
            this.textBoxWordsNr.TabIndex = 10;
            this.textBoxWordsNr.Text = "0";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(701, 492);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Words:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(595, 492);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Lines:";
            // 
            // textBoxLinesNr
            // 
            this.textBoxLinesNr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLinesNr.Location = new System.Drawing.Point(636, 487);
            this.textBoxLinesNr.Name = "textBoxLinesNr";
            this.textBoxLinesNr.ReadOnly = true;
            this.textBoxLinesNr.Size = new System.Drawing.Size(51, 20);
            this.textBoxLinesNr.TabIndex = 13;
            this.textBoxLinesNr.Text = "0";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(0, 32);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(25, 450);
            this.richTextBox1.TabIndex = 15;
            this.richTextBox1.Text = "";
            // 
            // FormMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(940, 509);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxLinesNr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxWordsNr);
            this.Controls.Add(this.buttonZoomOut);
            this.Controls.Add(this.buttonZoomIn);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.richTextBoxMain);
            this.Controls.Add(this.menuStripRibbons);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormMainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editorescu";
            this.menuStripRibbons.ResumeLayout(false);
            this.menuStripRibbons.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem buttonNew;
        private System.Windows.Forms.ToolStripMenuItem buttonOpen;
        private System.Windows.Forms.ToolStripMenuItem buttonSave;
        private System.Windows.Forms.ToolStripMenuItem buttonNewWindow;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem buttonUndo;
        private System.Windows.Forms.ToolStripMenuItem buttonRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem buttonCut;
        private System.Windows.Forms.ToolStripMenuItem buttonCopy;
        private System.Windows.Forms.ToolStripMenuItem buttonPaste;
        private System.Windows.Forms.ToolStripMenuItem buttonDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem buttonSearch;
        private System.Windows.Forms.ToolStripMenuItem buttonSearchAndReplace;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem buttonFormatDocument;
        private System.Windows.Forms.ToolStripMenuItem buttonToggleComment;
        private System.Windows.Forms.ToolStripMenuItem menuFormat;
        private System.Windows.Forms.ToolStripMenuItem buttoColoring;
        private System.Windows.Forms.ToolStripMenuItem buttonColoringPreferences;
        private System.Windows.Forms.ToolStripMenuItem buttonFont;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem syntaxCheckerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordCountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem buttonDocs;
        private System.Windows.Forms.ToolStripMenuItem buttonReportBug;
        private System.Windows.Forms.ToolStripMenuItem buttonAbout;
        private System.Windows.Forms.MenuStrip menuStripRibbons;
        private System.Windows.Forms.RichTextBox richTextBoxMain;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.TextBox textBoxWordsNr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLinesNr;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}


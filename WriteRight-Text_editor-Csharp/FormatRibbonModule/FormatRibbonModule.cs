using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml;
using CustomControls;
using static CommonsModule.UtilitiesFormat;
using static CustomControls.SystemMessageHandler;

namespace FormatRibbonModule
{
    public class ThemeCommand : MainWindowCommand
    {
        private static void SetDarkTitleBar(IntPtr handle)
        {
            int preference = 1;    
            DwmSetWindowAttribute(handle, 20, ref preference, sizeof(int));
        }
        
        private static ThemeCommand _singletonInstance;
        private Form _mainFormWindowRef;
        private readonly List<Control> _controls= new List<Control>();
        
        private ThemeCommand(){}

        public new static ThemeCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new ThemeCommand());
        }
        
        public override void Execute()
        {
            if (!IsDarkmode)
            {
                SetDarkTitleBar(_mainFormWindowRef.Handle);
                _mainFormWindowRef.BackColor = ColorTranslator.FromHtml("#1F2428");
                for (int i = 0; i < _controls.Count; i++) {
                    string type = _controls[i].GetType().ToString().Trim().Replace("System.Windows.Forms.", "").Replace("CommonsModule.", "");                    
                    switch (type) {
                        case "RichTextBox":
                            _controls[i].BackColor = ColorTranslator.FromHtml("#24292E");
                            _controls[i].ForeColor = ColorTranslator.FromHtml("#E1E4E8");
                            break;
                        case "Label":
                            _controls[i].BackColor = ColorTranslator.FromHtml("#24292E"); 
                            _controls[i].ForeColor = ColorTranslator.FromHtml("#C8D3DA"); 
                            break;
                        case "TextBox":
                            _controls[i].BackColor = ColorTranslator.FromHtml("#24292E");
                            _controls[i].ForeColor = ColorTranslator.FromHtml("#C8D3DA"); 
                            break;
                        case "MenuStrip":
                            _controls[i].BackColor = ColorTranslator.FromHtml("#1F2428");
                            foreach (ToolStripMenuItem item in ((MenuStrip)_controls[i]).Items)
                            {
                                item.BackColor = ColorTranslator.FromHtml("#1F2428");
                                item.ForeColor = ColorTranslator.FromHtml("#D7DADE");
                                foreach (ToolStripItem subItem in item.DropDownItems)
                                {
                                    if (subItem.Text == "Theme: Light") {
                                        subItem.Text = "Theme: Dark";
                                    }
                                    subItem.BackColor = ColorTranslator.FromHtml("#1F2428");
                                    subItem.ForeColor = ColorTranslator.FromHtml("#D7DADE");
                                }
                            }
                            break;
                        case "StatusStrip":
                            _controls[i].BackColor = ColorTranslator.FromHtml("#24292E");
                            _controls[i].ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                            break;
                        case "Button":
                            _controls[i].BackColor = ColorTranslator.FromHtml("#24292E");
                            _controls[i].ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                            break;
                        case "TabControl":
                            foreach (TabPage page in ((TabControl)_controls[i]).TabPages) {
                                page.BackColor = ColorTranslator.FromHtml("#24292E");
                                page.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                                ((RichTextBoxV2)((TextEditorControl)page.Controls[0]).RichTextBoxEditor).BackColor = ColorTranslator.FromHtml("#24292E");
                                ((RichTextBoxV2)((TextEditorControl)page.Controls[0]).RichTextBoxEditor).ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                                ((TextEditorControl)page.Controls[0]).BackColor = ColorTranslator.FromHtml("#C8D3DA");

                                ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.BackColor = ColorTranslator.FromHtml("#24292E");
                                ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                            }
                            break;
                    }
                }
                IsDarkmode = true;
            }
            else {
                foreach (Control control in _controls)
                { 
                   control.BackColor= new Color();
                   control.ForeColor=new Color();
                }
                _mainFormWindowRef.BackColor = new Color();
                for (int i = 0; i < _controls.Count; i++) {
                    if (_controls[i] is MenuStrip)
                    {
                        foreach (ToolStripMenuItem item in ((MenuStrip)_controls[i]).Items)
                        {
                            item.BackColor = new Color();
                            item.ForeColor = new Color();
                            foreach (ToolStripItem subItem in item.DropDownItems)
                            {
                                if (subItem.Text == "Theme: Dark")
                                {
                                    subItem.Text = "Theme: Light";
                                }
                                subItem.BackColor = new Color();
                                subItem.ForeColor = new Color();
                            }
                        }
                    }
                    else if (_controls[i] is TabControl) {
                        foreach (TabPage page in ((TabControl)_controls[i]).Controls) {
                            page.BackColor = new Color();
                            page.ForeColor = new Color();
                            ((RichTextBoxV2)((TextEditorControl)page.Controls[0]).RichTextBoxEditor).BackColor = new Color();
                            ((RichTextBoxV2)((TextEditorControl)page.Controls[0]).RichTextBoxEditor).ForeColor = new Color();

                            ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.BackColor = new Color();
                            ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.ForeColor = new Color();
                        }
                    }
                }
                IsDarkmode = false;
            }
        }

        public override void SetTarget(Form mainWindow)
        {
            _mainFormWindowRef = mainWindow;
            for (int i = 0; i < mainWindow.Controls.Count; i++) {
                _controls.Add(mainWindow.Controls[i]);
            }
        }
    }
    public class SyntaxHighlightCommand : MainTextBoxCommand
    {
        private static SyntaxHighlightCommand _singletonInstance;
        private RichTextBoxV2 _mainRichTextBoxV2Ref;
        private Form _popupForm;
        private Label _label;
        private bool _isDefault=true;
        public static bool IsXmlChanged=false;

        private SyntaxHighlightCommand() { }

        public new static SyntaxHighlightCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new SyntaxHighlightCommand());
        }
        public override void Execute()
        {
            ShowDialog();
        }

        public override void SetTarget(IRichTextBoxV2 mainTextBox)
        {
            _mainRichTextBoxV2Ref =(RichTextBoxV2) mainTextBox;
        }

        private void ShowDialog()
        {
            _popupForm = new Form();
            _popupForm.Height = 210;
            _popupForm.Width = 195;
            _popupForm.MinimizeBox = false;
            _popupForm.StartPosition = FormStartPosition.CenterScreen;

             _label = new Label();
            _label.Text = "Choose your setup";
            _label.AutoSize = true;
            _label.Location = new Point(20, 10);
            _popupForm.Controls.Add(_label);

            Button button1 = new Button();
            button1.Text = "Default";
            button1.Location = new Point(20, 50);
            button1.Click += ChangeProfile_Click;
            _popupForm.Controls.Add(button1);

            Button button2 = new Button();
            button2.Text = "Personalized";
            button2.Location = new Point(20, 80);
            button2.Click += ChangeProfile_Click;
            _popupForm.Controls.Add(button2);
            
            Button button3 = new Button();
            button3.Location = new Point(100, 140);
            button3.Text = "Close";
            button3.Click += ButtonActions_Click;
            _popupForm.Controls.Add(button3);

            Button button4 = new Button();
            button4.Location = new Point(20, 140);
            button4.Text = "Save";
            button4.Click += ButtonActions_Click;
            _popupForm.Controls.Add(button4);
            
            Button button5 = new Button();
            button5.Text = "Types";
            button5.Location = new Point(120, 50);
            button5.Tag = "Types";
            button5.Click += ButtonChooseColor_Click;
            button5.Visible = false;
            _popupForm.Controls.Add(button5);

            Button buttton6 = new Button();
            buttton6.Text = "Expressions";
            buttton6.Location = new Point(120, 80);
            buttton6.Tag = "Expressions";
            buttton6.Click += ButtonChooseColor_Click;
            buttton6.Visible = false;
            _popupForm.Controls.Add(buttton6);

            Button button7 = new Button();
            button7.Text = "Operators";
            button7.Location = new Point(120, 110);
            button7.Tag = "Operators";
            button7.Click += ButtonChooseColor_Click;
            button7.Visible = false;
            _popupForm.Controls.Add(button7);

            Button button8 = new Button();
            button8.Text = "Comments";
            button8.Location = new Point(200, 50);
            button8.Tag = "Comments";
            button8.Click += ButtonChooseColor_Click;
            button8.Visible = false;
            _popupForm.Controls.Add(button8);

            Button button9 = new Button();
            button9.Text = "Strings";
            button9.Location = new Point(200, 80);
            button9.Tag = "Strings";
            button9.Click += ButtonChooseColor_Click;
            button9.Visible = false;
            _popupForm.Controls.Add(button9);

            Button button10 = new Button();
            button10.Text = "Preprocesor";
            button10.Location = new Point(200, 110);
            button10.Tag = "Preprocesor";
            button10.Click += ButtonChooseColor_Click;
            button10.Visible = false;
            _popupForm.Controls.Add(button10);

            _popupForm.ShowDialog();
        }
        private void ChangeProfile_Click(object sender, EventArgs e)
        {
            string buttonText = ((Button)sender).Text;
            switch (buttonText)
            {
                case "Default":
                    _isDefault = true;
                    break;
                case "Personalized":
                    if (_popupForm.Width == 300)
                    {
                        _popupForm.Width = 195;
                    }
                    else if (_popupForm.Width == 195) {
                        _popupForm.Width = 300;
                    }
                    _isDefault = false;
                    foreach (Control control in _popupForm.Controls)
                    {
                        if (!(control is Button) || (control.Text != "Strings" && control.Text != "Types" &&
                                                     control.Text != "Expressions" && control.Text != "Comments" &&
                                                     control.Text != "Preprocesor" &&
                                                     control.Text != "Operators")) continue;
                        switch (control.Visible)
                        {
                            case false:
                                control.Visible = true;
                                break;
                            case true:
                                control.Visible = false;
                                break;
                        }
                    }
                    break;
            }
        }

        private void ButtonActions_Click(object sender, EventArgs e)
        {
            string buttonText = ((Button)sender).Text;
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../../colors.xml");
            switch (buttonText) { 
                case "Close":
                    _popupForm.Close();
                    break;
                case "Save":
                    if (_isDefault)
                    {
                        doc.SelectSingleNode("//default").InnerText="true";
                        IsXmlChanged = true;
                    }
                    else {
                        doc.SelectSingleNode("//default").InnerText = "false";
                        IsXmlChanged = true;
                    }
                    break;
            }
            doc.Save("../../../../colors.xml");
        }


        private void ButtonChooseColor_Click(object sender, EventArgs e)
        {
            string buttonTag = (string)((Button)sender).Tag;
            ColorDialog colorDialog = new ColorDialog();
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../../colors.xml");
            DialogResult result = colorDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                XmlNode xml=doc.SelectSingleNode("//type");
                switch (buttonTag)
                {
                    case "Types":
                        _mainRichTextBoxV2Ref.Text += "types";
                        xml = doc.SelectSingleNode("//type");
                        break;
                    case "Expressions":
                        _mainRichTextBoxV2Ref.Text += "expression";
                        xml = doc.SelectSingleNode("//expression");
                        break;
                    case "Operators":
                        _mainRichTextBoxV2Ref.Text += "operator";
                        xml = doc.SelectSingleNode("//operator");
                        break;
                    case "Comments":
                        _mainRichTextBoxV2Ref.Text += "comment";
                        xml = doc.SelectSingleNode("//comment");
                        break;
                    case "Strings":
                        _mainRichTextBoxV2Ref.Text += "string";
                        xml = doc.SelectSingleNode("//string");
                        break;
                    case "Preprocesor":
                        _mainRichTextBoxV2Ref.Text += "preproces";
                        xml = doc.SelectSingleNode("//preprocesor");
                        break;
                    case "Text":
                        break;
                }
                ((Button)sender).BackColor = colorDialog.Color;
                int r = 255 - colorDialog.Color.R;
                int g = 255 - colorDialog.Color.G;
                int b = 255 - colorDialog.Color.B;
                Color contrastingColor = Color.FromArgb(r, g, b);
                ((Button)sender).ForeColor = contrastingColor;
                if (xml != null) xml.InnerText = ColorTranslator.ToHtml(colorDialog.Color);
                doc.Save("../../../../colors.xml");
            IsXmlChanged = true;
            }
        }
    }
    public class FontCommand : MainTextBoxCommand
    {
        private static FontCommand _singletonInstance;
        private RichTextBoxV2 _mainRichTextBoxV2Ref;

        private FontCommand(){}

        public new static FontCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new FontCommand());
        }
       
        public override void Execute()
        {
            FontDialog fontDialog = new FontDialog();         
            DialogResult fontResult = fontDialog.ShowDialog();          
                    
            if (fontResult == DialogResult.OK)
            {
                _mainRichTextBoxV2Ref.SelectionFont = fontDialog.Font;
            }            
        }

        public override void SetTarget(IRichTextBoxV2 mainTextBox)
        {
            _mainRichTextBoxV2Ref = (RichTextBoxV2) mainTextBox;
          
        }
    }
    public class FormatDocument : MainTextBoxCommand
    {
        private static FormatDocument _singletonInstance;
        private RichTextBoxV2 _mainRichTextBoxV2Ref;
        private int _tabNum;

        private FormatDocument() { }

        public new static FormatDocument GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new FormatDocument());
        }

        public override void Execute()
        {
            ExecuteFormat();
        }

        public override void SetTarget(IRichTextBoxV2 mainTextBox)
        {
            _mainRichTextBoxV2Ref =(RichTextBoxV2)mainTextBox;

        }

        private string Tabs()
        {
            string tabs = "";
            if (_tabNum == 0)
            {
                return "";
            }
            else
            {
                int nums = _tabNum;
                while (nums > 0)
                {
                    tabs += "\t";
                    nums--;
                }
                return tabs;
            }
        }

        private void ExecuteFormat()
        {
            RichTextBoxV2 richTextBox = _mainRichTextBoxV2Ref;
            richTextBox.Text = richTextBox.Text.Replace("\t", " ");
            richTextBox.Text = (Regex.Replace(richTextBox.Text, @"\s+", " ")).Replace("\n\n", "\n");
            
            for (int i = 0; i < 3; i++)
            {
                _tabNum = 0;
                string total = "";
                foreach (string line in richTextBox.Lines)
                {
                    string modif = "";
                    string replacedLine = line.Replace("\t", "");
                    if (replacedLine == "") { continue; }
                    if (replacedLine.Contains(';') )
                    {
                        int index = 0;
                        if (replacedLine.Contains("for"))
                        {
                            index = 0;
                            while (index != -1)
                            {
                                index = replacedLine.IndexOf(";", index, StringComparison.Ordinal);
                                if (index == -1) break;
                                int open = replacedLine.LastIndexOf("(", index, StringComparison.Ordinal);
                                if (open == -1) break;
                                int closed = replacedLine.IndexOf(")", open, StringComparison.Ordinal);

                                if (index > open && index < closed){}
                                else
                                {
                                    replacedLine = replacedLine.Remove(index, 1).Insert(index, ";\n");
                                }
                                index++;
                            }
                        }
                        else if (replacedLine.Contains("\";\""))
                        {
                            index = 0;
                            while (index != -1)
                            {
                                int indexSimple = replacedLine.IndexOf(";", index, StringComparison.Ordinal);
                                int indexString = replacedLine.IndexOf("\";\"", index, StringComparison.Ordinal);
                                if (indexSimple == -1) break;

                                if (indexString + 1 != indexSimple)
                                {
                                    replacedLine = replacedLine.Remove(indexSimple, 1).Insert(indexSimple, ";\n");
                                }
                                index = indexSimple + 1;
                            }
                        }
                        else
                        {
                            replacedLine = replacedLine.Replace(";", ";\n");
                        }
                        modif = replacedLine;
                    }
                    if (replacedLine.Contains('{') && replacedLine[0] != '{')
                    {
                        modif = Tabs() + replacedLine.Replace("{", "\n" + Tabs() + "{\n");
                        _tabNum++;
                    }
                    else switch (replacedLine[0])
                    {
                        case '{':
                            modif = replacedLine.Replace("{", Tabs() + "{\n");
                            _tabNum++;
                            break;
                        case '}':
                        {
                            _tabNum--;
                            if (replacedLine.Length >= 2 && replacedLine[1] == ';')
                            {
                                modif = replacedLine.Replace("};", Tabs() + "};\n");
                            }
                            else
                            {
                                modif = replacedLine.Replace("}", Tabs() + "}\n");
                            }

                            break;
                        }
                        default:
                        {
                            if (replacedLine.Contains('}') && replacedLine[0] != '}')
                            {
                                _tabNum--;
                                if (replacedLine.Contains("};"))
                                {
                                    modif = replacedLine.Replace("};", "\n" + Tabs() + "};\n");
                                    modif = Tabs() + modif;
                                }
                                else
                                {
                                    modif = replacedLine.Replace("}", "\n" + Tabs() + "}\n");
                                    modif = Tabs() + modif;
                                }
                            }
                            else if (!replacedLine.Contains('{') && !replacedLine.Contains('}'))
                            {
                                modif = Tabs() + replacedLine + "\n";
                            }

                            break;
                        }
                    }
                    total += modif;
                }
                richTextBox.Text = Regex.Replace(total, @"^[ \t\n]+$", "", RegexOptions.Multiline).Replace("\n\n","\n");
            }
        }
    }
}





using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using CustomControls;
using static CommonsModule.UtilitiesFormat;

namespace FormatRibbonModule
{
    public class ThemeCommand : IMainWindowCommand
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        public static void SetDarkTitleBar(IntPtr handle)
        {
            int preference = 1;    
            DwmSetWindowAttribute(handle, 20, ref preference, sizeof(int));
        }
        
        private static ThemeCommand _singletonInstance = null;
        internal Form _mainFormWindowRef;
        internal List<Control> _controls= new List<Control>();
        
        private ThemeCommand(){}

        public static new ThemeCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new ThemeCommand();
            }
            return _singletonInstance;
        }
   

        public override void Execute()
        {
            if (!isDarkmode)
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
                                ((TextEditorControl)page.Controls[0]).RichTextBoxEditor.BackColor = ColorTranslator.FromHtml("#24292E");
                                ((TextEditorControl)page.Controls[0]).RichTextBoxEditor.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                                ((TextEditorControl)page.Controls[0]).BackColor = ColorTranslator.FromHtml("#C8D3DA");

                                ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.BackColor = ColorTranslator.FromHtml("#24292E");
                                ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                            }
                            break;
                    }
                }
                isDarkmode = true;
            }
            else {
                foreach (Control control in this._controls)
                { 
                   control.BackColor= new Color(); ;
                   control.ForeColor=new Color(); ;
                }
                _mainFormWindowRef.BackColor = new Color(); ;
                for (int i = 0; i < _controls.Count; i++) {
                    if (_controls[i] is MenuStrip)
                    {
                        foreach (ToolStripMenuItem item in ((MenuStrip)_controls[i]).Items)
                        {
                            item.BackColor = new Color(); ;
                            item.ForeColor = new Color(); ;
                            foreach (ToolStripItem subItem in item.DropDownItems)
                            {
                                if (subItem.Text == "Theme: Dark")
                                {
                                    subItem.Text = "Theme: Light";
                                }
                                subItem.BackColor = new Color(); ;
                                subItem.ForeColor = new Color(); ;
                            }
                        }
                    }
                    else if (_controls[i] is TabControl) {
                        foreach (TabPage page in ((TabControl)_controls[i]).Controls) {
                            page.BackColor = new Color(); ;
                            page.ForeColor = new Color(); ;
                            ((TextEditorControl)page.Controls[0]).RichTextBoxEditor.BackColor = new Color(); ;
                            ((TextEditorControl)page.Controls[0]).RichTextBoxEditor.ForeColor = new Color(); ;

                           ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.BackColor = new Color(); ;
                            ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.ForeColor = new Color(); ;
                        }
                    }
                }
                isDarkmode = false;
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
    public class SyntaxHighlightCommand : IMainTextBoxCommand
    {
        private static SyntaxHighlightCommand _singletonInstance = null;
        internal RichTextBoxV2 _mainRichTextBoxV2Ref;
        internal Form popupForm;
        internal Label label;
        internal bool isDefault=true;
        public static bool isXMLChanged=false;

        private SyntaxHighlightCommand() { }

        public static new SyntaxHighlightCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new SyntaxHighlightCommand();
            }
            return _singletonInstance;
        }
        public override void Execute()
        {
            ShowDialog();
        }

        public override void SetTarget(RichTextBoxV2 mainTextBox)
        {
            _mainRichTextBoxV2Ref = mainTextBox;
        }

        internal void ShowDialog()
        {
            popupForm = new Form();
            popupForm.Height = 210;
            popupForm.Width = 195;
            popupForm.MinimizeBox = false;
            popupForm.StartPosition = FormStartPosition.CenterScreen;

             label = new Label();
            label.Text = "Choose your setup";
            label.AutoSize = true;
            label.Location = new Point(20, 10);
            popupForm.Controls.Add(label);

            Button button1 = new Button();
            button1.Text = "Default";
            button1.Location = new Point(20, 50);
            button1.Click += new EventHandler(ChangeProfile_Click);
            popupForm.Controls.Add(button1);

            Button button2 = new Button();
            button2.Text = "Personalized";
            button2.Location = new Point(20, 80);
            button2.Click += new EventHandler(ChangeProfile_Click);
            popupForm.Controls.Add(button2);
            
            Button button3 = new Button();
            button3.Location = new Point(100, 140);
            button3.Text = "Close";
            button3.Click += new EventHandler(ButtonActions_Click);
            popupForm.Controls.Add(button3);

            Button button4 = new Button();
            button4.Location = new Point(20, 140);
            button4.Text = "Save";
            button4.Click += new EventHandler(ButtonActions_Click);
            popupForm.Controls.Add(button4);
            
            Button button5 = new Button();
            button5.Text = "Types";
            button5.Location = new Point(120, 50);
            button5.Tag = "Types";
            button5.Click += new EventHandler(ButtonChooseColor_Click);
            button5.Visible = false;
            popupForm.Controls.Add(button5);

            Button buttton6 = new Button();
            buttton6.Text = "Expressions";
            buttton6.Location = new Point(120, 80);
            buttton6.Tag = "Expressions";
            buttton6.Click += new EventHandler(ButtonChooseColor_Click);
            buttton6.Visible = false;
            popupForm.Controls.Add(buttton6);

            Button button7 = new Button();
            button7.Text = "Operators";
            button7.Location = new Point(120, 110);
            button7.Tag = "Operators";
            button7.Click += new EventHandler(ButtonChooseColor_Click);
            button7.Visible = false;
            popupForm.Controls.Add(button7);

            Button button8 = new Button();
            button8.Text = "Comments";
            button8.Location = new Point(200, 50);
            button8.Tag = "Comments";
            button8.Click += new EventHandler(ButtonChooseColor_Click);
            button8.Visible = false;
            popupForm.Controls.Add(button8);

            Button button9 = new Button();
            button9.Text = "Strings";
            button9.Location = new Point(200, 80);
            button9.Tag = "Strings";
            button9.Click += new EventHandler(ButtonChooseColor_Click);
            button9.Visible = false;
            popupForm.Controls.Add(button9);

            Button button10 = new Button();
            button10.Text = "Preprocesor";
            button10.Location = new Point(200, 110);
            button10.Tag = "Preprocesor";
            button10.Click += new EventHandler(ButtonChooseColor_Click);
            button10.Visible = false;
            popupForm.Controls.Add(button10);

            popupForm.ShowDialog();
        }
        private void ChangeProfile_Click(object sender, EventArgs e)
        {
            string buttonText = (string)((Button)sender).Text;
            switch (buttonText)
            {
                case "Default":
                    isDefault = true;
                    break;
                case "Personalized":
                    if (popupForm.Width == 300)
                    {
                        popupForm.Width = 195;
                    }
                    else if (popupForm.Width == 195) {
                        popupForm.Width = 300;
                    }
                    isDefault = false;
                    foreach (Control control in popupForm.Controls) {
                        if (control is Button  && (control.Text == "Strings" || control.Text == "Types" || control.Text == "Expressions" || control.Text == "Comments" || control.Text == "Preprocesor" || control.Text == "Operators")) {
                            if (control.Visible == false)
                            {
                                control.Visible = true;
                            }
                            else if (control.Visible==true){
                                control.Visible = false;
                            }
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
                    popupForm.Close();
                    break;
                case "Save":
                    if (isDefault)
                    {
                        doc.SelectSingleNode("//default").InnerText="true";
                        isXMLChanged = true;
                    }
                    else {
                        doc.SelectSingleNode("//default").InnerText = "false";
                        isXMLChanged = true;
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
            xml.InnerText = ColorTranslator.ToHtml(colorDialog.Color);
            doc.Save("../../../../colors.xml");
            isXMLChanged = true;
            }
        }
    }
    public class FontCommand : IMainTextBoxCommand
    {
        private static FontCommand _singletonInstance = null;
        internal RichTextBoxV2 _mainRichTextBoxV2Ref;

        private FontCommand(){}

        public static new FontCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new FontCommand();
            }
            return _singletonInstance;
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

        public override void SetTarget(RichTextBoxV2 mainTextBox)
        {
            _mainRichTextBoxV2Ref = mainTextBox;
          
        }
    }
    public class FormatDocument : IMainTextBoxCommand
    {
        private static FormatDocument _singletonInstance = null;
        internal RichTextBoxV2 _mainRichTextBoxV2Ref;
        internal int _tabNum = 0;

        private FormatDocument() { }

        public static new FormatDocument GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new FormatDocument();
            }
            return _singletonInstance;
        }

        public override void Execute()
        {
            ExecuteFormat();
        }

        public override void SetTarget(RichTextBoxV2 mainTextBox)
        {
            _mainRichTextBoxV2Ref = mainTextBox;
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
        public void ExecuteFormat()
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
                                index = replacedLine.IndexOf(";", index);
                                if (index == -1) break;
                                int open = replacedLine.LastIndexOf("(", index);
                                if (open == -1) break;
                                int closed = replacedLine.IndexOf(")", open);

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
                                int indexSimple = replacedLine.IndexOf(";", index);
                                int indexString = replacedLine.IndexOf("\";\"", index);
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
                    else if (replacedLine[0] == '{')
                    {
                        modif = replacedLine.Replace("{", Tabs() + "{\n");
                        _tabNum++;
                    }
                    else if (replacedLine[0] == '}')
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
                    }
                    else if (replacedLine.Contains('}') && replacedLine[0] != '}')
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
                    total += modif;
                }
                richTextBox.Text = Regex.Replace(total, @"^[ \t\n]+$", "", RegexOptions.Multiline).Replace("\n\n","\n");
            }
        }
    }
}





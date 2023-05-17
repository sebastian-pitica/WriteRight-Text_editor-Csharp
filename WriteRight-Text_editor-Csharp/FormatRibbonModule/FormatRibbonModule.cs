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
using CommonsModule;

/**************************************************************************
 *                                                                        *
 *  File:        FormatRibbonModule.cs                                    *
 *  Copyright:   (c) 2023, Matei Rares                                    *
 *  Desciprtion: Fișierul conține obiectele de tip Singleton-Command      *
 *  aferente ribbonului Format, separate pentru a se putea lucra          *
 *  individual de celelalte ribbonuri.                                    *
 *  Updated by: Matei Rares                                               *
 *                                                                        *
 **************************************************************************/

namespace FormatRibbonModule
{
    /// <summary>
    /// Această clasă are ca scop schimbarea tematicii din Form-ul principal.
    /// </summary>
    public class ThemeCommand : MainWindowCommand
    {
        private static ThemeCommand _singletonInstance;
        private Form _mainFormWindowRef;
        private readonly List<Control> _controls = new List<Control>();
        private static readonly Color BackColor = ColorTranslator.FromHtml("#24292E");
        private static readonly Color ForeColor = ColorTranslator.FromHtml("#C8D3DA");

        /// <summary>
        /// Această funcție schimbă culoarea de fundal a barei de fundal din default, în negru.
        /// </summary>
        /// <param name="handle">Parametrul reprezinta Handle-ul Form-ului</param>
        private static void SetDarkTitleBar(IntPtr handle)
        {
            int preference = 1;    
            DwmSetWindowAttribute(handle, 20, ref preference, sizeof(int));
        }
        
        private ThemeCommand(){}

        public new static ThemeCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new ThemeCommand());
        }
        
        public override void Execute()
        {
            if (!IsDarkMode)
            {
                try
                {
                    SetDarkMode();
                }
                catch (Exception ex) {
                    Utilities.HandleException(ex);  
                }
                IsDarkMode = true;
            }
            else {
                try
                {
                    SetWhiteMode();
                }
                catch (Exception ex) {
                    Utilities.HandleException(ex);
                }
                IsDarkMode = false;
            }
        }

        /// <summary>
        /// Această functie aplică un background și un foreground pentru fiecare Control din Form.
        /// </summary>
        internal void SetDarkMode() {
            SetDarkTitleBar(_mainFormWindowRef.Handle);
            _mainFormWindowRef.BackColor = ColorTranslator.FromHtml("#1F2428");
            for (int i = 0; i < _controls.Count; i++)
            {
                string type = _controls[i].GetType().ToString().Trim().Replace("System.Windows.Forms.", "").Replace("CommonsModule.", "");
                switch (type)
                {
                    case "RichTextBox":
                        _controls[i].BackColor = BackColor;
                        _controls[i].ForeColor = ColorTranslator.FromHtml("#E1E4E8");
                        break;
                    case "Label":
                    case "TextBox":
                    case "StatusStrip":
                    case "Button":
                        _controls[i].BackColor = BackColor;
                        _controls[i].ForeColor = ForeColor;
                        break;
                    case "MenuStrip":
                        _controls[i].BackColor = ColorTranslator.FromHtml("#1F2428");
                        foreach (ToolStripMenuItem item in ((MenuStrip)_controls[i]).Items)
                        {
                            item.BackColor = ColorTranslator.FromHtml("#1F2428");
                            item.ForeColor = ColorTranslator.FromHtml("#D7DADE");
                            foreach (ToolStripItem subItem in item.DropDownItems)
                            {
                                if (subItem.Text == "Theme: Light")
                                {
                                    subItem.Text = "Theme: Dark";
                                }
                                subItem.BackColor = ColorTranslator.FromHtml("#1F2428");
                                subItem.ForeColor = ColorTranslator.FromHtml("#D7DADE");
                            }
                        }
                        break;
                    case "TabControl":
                        foreach (TabPage page in ((TabControl)_controls[i]).TabPages)
                        {
                            page.BackColor = BackColor;
                            page.ForeColor = ForeColor;
                            ((RichTextBoxV2)((TextEditorControl)page.Controls[0]).RichTextBoxEditor).BackColor = BackColor;
                            ((RichTextBoxV2)((TextEditorControl)page.Controls[0]).RichTextBoxEditor).ForeColor = ForeColor;
                            ((TextEditorControl)page.Controls[0]).BackColor = ForeColor;
                            ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.BackColor = BackColor;
                            ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.ForeColor = ForeColor;
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Această funcție aplică un fundal alb și un font negru pentru controalele din Form.
        /// </summary>
        internal void SetWhiteMode()
        {
            foreach (Control control in _controls)
            {
                control.BackColor = new Color();
                control.ForeColor = new Color();
            }
            _mainFormWindowRef.BackColor = new Color();
            for (int i = 0; i < _controls.Count; i++)
            {
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
                else if (_controls[i] is TabControl)
                {
                    foreach (TabPage page in ((TabControl)_controls[i]).Controls)
                    {
                        page.BackColor = new Color();
                        page.ForeColor = new Color();
                        ((RichTextBoxV2)((TextEditorControl)page.Controls[0]).RichTextBoxEditor).BackColor = new Color();
                        ((RichTextBoxV2)((TextEditorControl)page.Controls[0]).RichTextBoxEditor).ForeColor = new Color();

                        ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.BackColor = new Color();
                        ((TextEditorControl)page.Controls[0]).RichTextBoxNumbering.ForeColor = new Color();
                    }
                }
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

    /// <summary>
    /// Această clasă are ca scop crearea unui dialog care face posibilă alegerea unui profil de culori pentru cuvintele speciale din RichTextBox.
    /// </summary>
    public class SyntaxHighlightCommand : MainTextBoxCommand
    {
        private static SyntaxHighlightCommand _singletonInstance;
        private RichTextBoxV2 _mainRichTextBoxV2Ref;
        private Form _popupForm;
        private Label _label;
        private bool _isDefault=true;

        private SyntaxHighlightCommand() { }

        public new static SyntaxHighlightCommand GetCommandObj()
        {
            return _singletonInstance ?? (_singletonInstance = new SyntaxHighlightCommand());
        }
        public override void Execute()
        {
            try
            {
                ShowDialog();
            }
            catch (Exception ex) { 
                Utilities.HandleException(ex);
            }
        }

        public override void SetTarget(IRichTextBoxV2 mainTextBox)
        {
            _mainRichTextBoxV2Ref =(RichTextBoxV2) mainTextBox;
        }

        /// <summary>
        /// Acestă funcție afișează un Form din care se poate selecta profilul de culori pentru cuvintele cheie.
        /// </summary>
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

            Button button6 = new Button();
            button6.Text = "Expressions";
            button6.Location = new Point(120, 80);
            button6.Tag = "Expressions";
            button6.Click += ButtonChooseColor_Click;
            button6.Visible = false;
            _popupForm.Controls.Add(button6);

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

        /// <summary>
        /// Această funcție tratează evenimentul de click pe unul din butoanele de selectare a profilului de culori.
        /// Dacă s-a apăsat default/personalized se setează profilul de culori respectiv.
        /// În cazul în care s-a apăsat Personalized, butoanele pentru fiecare tip de cuvânt special devin vizibile.
        /// </summary>
        /// <param name="sender">Acest parametru reprezintă butonul apăsat</param>
        /// <param name="e">Acest parametru reprezintă evenimentul de click</param>
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
        /// <summary>
        /// Această funcție tratează evenimentul de salvare sau închidere a Form-ului de selectare a profilului de culori.
        /// Daca se apasa Save, se salveaza profilul ales.
        /// Daca se apasa Close,ferestra se inchide.
        /// </summary>
        /// <param name="sender">Acest parametru reprezintă butonul apăsat</param>
        /// <param name="e">Acest parametru reprezintă evenimentul de click</param>
        private void ButtonActions_Click(object sender, EventArgs e)
        {
            string buttonText = ((Button)sender).Text;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("../../../../colors.xml");
                switch (buttonText)
                {
                    case "Close":
                        _popupForm.Close();
                        break;
                    case "Save":
                        if (_isDefault)
                        {
                            doc.SelectSingleNode("//default").InnerText = "true";
                        }
                        else
                        {
                            doc.SelectSingleNode("//default").InnerText = "false";
                        }
                        IsXmlChanged = true;

                        break;
                }
                doc.Save("../../../../colors.xml");
            }
            catch (Exception ex) {
                Utilities.HandleException(ex);
            }
        }

        /// <summary>
        /// Această funcție tratează evenimentul de alegere a unei culori pentru un tip de cuvânt special.
        /// Apăsarea unui buton va deschide un ColorDialog prin care va seta culoarea pentru o categorie de cuvinte.
        /// Alegerea unei culori duce la salvarea culorii în fișierul colors.xml.
        /// </summary>
        /// <param name="sender">Acest parametru reprezintă butonul apăsat</param>
        /// <param name="e">Acest parametru reprezintă evenimentul de click</param>
        private void ButtonChooseColor_Click(object sender, EventArgs e)
        {
            string buttonTag = (string)((Button)sender).Tag;
            ColorDialog colorDialog = new ColorDialog();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("../../../../colors.xml");
                DialogResult result = colorDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    XmlNode xml = doc.SelectSingleNode("//type");
                    switch (buttonTag)
                    {
                        case "Types":
                            xml = doc.SelectSingleNode("//type");
                            break;
                        case "Expressions":
                            xml = doc.SelectSingleNode("//expression");
                            break;
                        case "Operators":
                            xml = doc.SelectSingleNode("//operator");
                            break;
                        case "Comments":
                            xml = doc.SelectSingleNode("//comment");
                            break;
                        case "Strings":
                            xml = doc.SelectSingleNode("//string");
                            break;
                        case "Preprocesor":
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
            catch (Exception ex) {
                Utilities.HandleException(ex);
            }
        }
    }
    // /// <summary>
    // /// Această clasă are ca scop crearea unui dialog pentru schimbarea fontului unui RichTextBox.
    // /// </summary>
    // public class FontCommand : MainTextBoxCommand
    // {
    //     private static FontCommand _singletonInstance;
    //     private RichTextBoxV2 _mainRichTextBoxV2Ref;
    //
    //     private FontCommand(){}
    //
    //     public new static FontCommand GetCommandObj()
    //     {
    //         return _singletonInstance ?? (_singletonInstance = new FontCommand());
    //     }
    //    
    //     public override void Execute()
    //     {
    //         try
    //         {
    //             FontDialog fontDialog = new FontDialog();
    //             DialogResult fontResult = fontDialog.ShowDialog();
    //
    //             if (fontResult == DialogResult.OK)
    //             {
    //                 _mainRichTextBoxV2Ref.SelectionFont = fontDialog.Font;
    //             }
    //         }
    //         catch (Exception ex) {
    //             Utilities.HandleException(ex);
    //         }
    //     }
    //
    //     public override void SetTarget(IRichTextBoxV2 mainTextBox)
    //     {
    //         _mainRichTextBoxV2Ref = (RichTextBoxV2) mainTextBox;
    //       
    //     }
    // }
    /// <summary>
    /// Această clasă are ca scop identarea conținutului unui RichTextBox.
    /// </summary>
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
            try
            {
                ExecuteFormat();
            }
            catch(Exception ex)
            {
                Utilities.HandleException(ex);
            }
        }

        public override void SetTarget(IRichTextBoxV2 mainTextBox)
        {
            _mainRichTextBoxV2Ref =(RichTextBoxV2)mainTextBox;

        }

        /// <summary>
        /// Acesată funcție returnează un string care conține doar taburi. Numărul de taburi este dat de variabila _tabNum. 
        /// </summary>
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

        /// <summary>
        /// Această funcție face o formatare pe textul curent pentru a realiza identarea in funcție de "{", "}" si de ";".
        /// Sunt luate in considerare cazurile in care ";" se află după un for() sau cand ";" se află între ghilimele.
        /// </summary>
        private void ExecuteFormat()
        {
            RichTextBoxV2 richTextBox = _mainRichTextBoxV2Ref;
            richTextBox.Text = richTextBox.Text.Replace("\t", " ");
            richTextBox.Text = (Regex.Replace(richTextBox.Text, @"\s+", " ")).Replace("\n\n", "\n");
            
            for (int i = 0; i < 4; i++)
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





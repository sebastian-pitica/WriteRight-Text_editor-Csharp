using CustomControls;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

/**************************************************************************
 *                                                                        *
 *  File:        Utilities.cs                                             *
 *  Copyright:   (c) 2023, Caulea Vasile, Pitica Sebastian                *
 *  Description: Fișierul conține clasele cu caracter general utilizate   *
 *  în cadrul proiectului, separate pentru a putea fi folosit pe tot      *
 *  cuprinsul celorlalte module sau pentru că nu pot fi asociate cu alt   *
 *  modul concret.                                                        *
 *  Designed by: Pitica Sebastian                                         *
 *  Updated by: Pitica Sebastian                                          *
 *                                                                        *
 **************************************************************************/

namespace CommonsModule
{
    #region <creator>Caulea Vasile</creator>
    #region <updated>Pitica Sebastian</updated>
    
    /// <summary>
    /// Clasa utilizata pentru uz general
    /// </summary>
    public static class Utilities
    {
        public const string Ready = "Ready";
        public const string Loading = "Loading";

        public static List<string> FileFilters = new List<string>()
        {
            "C/C++ Files (*.c, *.cpp)|*.c;*.cpp",
            "Text Files(*.txt)|*.txt",
            "All Files(*.*)|*.*"
        };

        /// <summary>
        /// Creaza un TabPage care contine un element TextEditorControl.
        /// </summary>
        /// <param name="tabName">textul care va fi afisat in TabPage</param>
        /// <returns>TabPage</returns>
        public static TabPage CreateTab(in string tabName)
        {
            TabPage tabPage = new TabPage(tabName);
            TextEditorControl textEditor = new TextEditorControl();
            tabPage.Controls.Add(textEditor);

            if (UtilitiesFormat.IsDarkmode)
            {
                tabPage.BackColor =  ColorTranslator.FromHtml("#24292E");
                tabPage.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                ((RichTextBoxV2)textEditor.RichTextBoxEditor).BackColor = ColorTranslator.FromHtml("#24292E");
                ((RichTextBoxV2)textEditor.RichTextBoxEditor).ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                textEditor.BackColor = ColorTranslator.FromHtml("#C8D3DA");

                textEditor.RichTextBoxNumbering.BackColor = ColorTranslator.FromHtml("#24292E");
                textEditor.RichTextBoxNumbering.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
            }
            
            return tabPage;
        }
        
        public static void WriteFile(in string filePath, in string text)
        {
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(text);
            streamWriter.Close();
        } 
        
        public static RichTextBoxV2 GetRichTextBoxV2FromTabControl(in TabControl tabControl)
        {
            TabPage selectedTabPage = tabControl.SelectedTab;
            TextEditorControl reference = (TextEditorControl)selectedTabPage.Controls[0];
            return (reference.RichTextBoxEditor as RichTextBoxV2);
        }

        public static TextEditorControl GetTextEditorControlFromTabControl(in TabControl tabControl)
        {
            TabPage selectedTabPage = tabControl.SelectedTab;
            TextEditorControl reference = (TextEditorControl)selectedTabPage.Controls[0];
            return reference;
        }

        public static string GetFileNameFromTabControl(in TabControl tabControl)
        {
            RichTextBoxV2 reference = GetRichTextBoxV2FromTabControl(tabControl);
            return reference.FilePath ?? tabControl.SelectedTab.Text;
        }

        public static void HandleException(Exception ex)
        {
            MessageBox.Show(ex.ToString(),"- EXCEPTION -", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    #endregion
    #endregion

     public static class UtilitiesFormat
    {
        private static bool _isXmlChanged;
        public static bool IsDarkmode = false;
        private static string _typesColor = "Blue";
        private static string _expressionColor = "Purple";
        private static string _operatorsColor = "Blue";
        private static string _preprocesorColor = "Brown";
        private static string _stringColor = "Gray";
        private static string _commentColor = "Green";
        private static readonly string[] Types = { "void", "int", "string", "char", "float", "double", "asm", "auto", "bool", "class", "concept", "const", "enum", "explicit", "export", "extern", "friend", "inline", "long", "mutable", "private", "protected", "public", "register", "short", "signed", "static", "struct", "template", "union", "unsigned", "virtual", "volatile" };
        private static readonly string[] Expressions = { "and", "and_eq", "break", "case", "catch", "continue", "default", "delete", "do", "dynamic_cast", "else", "false", "for", "goto", "if", "namespace", "new", "not", "not_eq", "nullptr", "operator", "or", "or_eq", "return", "sizeof", "switch", "this", "throw", "true", "try", "typedef", "using", "while", "xor", "xor_eq" };
        private static readonly string[] Operators = { "+", "-", "%", "/", "*", "=", "<", ">" };
        private static readonly string[] Preprocessors = { "#define", "#elif", "#else", "#endif", "#error", "#if", "#ifdef", "#ifndef", "#import", "#include", "#line", "#pragma", "#undef", "#using" };

        private static void InitColors()
        {
            if (_isXmlChanged)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("../../../../colors.xml");
                if (doc.SelectSingleNode("//default")?.InnerText == "true")
                {
                    _typesColor = "Blue";
                    _expressionColor = "Red";
                    _operatorsColor = "Blue";
                    _preprocesorColor = "Brown";
                    _stringColor = "Gray";
                    _commentColor = "Green";
                }
                else
                {
                    _typesColor = (doc.SelectSingleNode("//type")?.InnerText == "default") ? "Blue" : doc.SelectSingleNode("//type")?.InnerText;
                    _expressionColor = (doc.SelectSingleNode("//expression")?.InnerText == "default") ? "Red" : doc.SelectSingleNode("//expression")?.InnerText;
                    _operatorsColor = (doc.SelectSingleNode("//operator")?.InnerText == "default") ? "Blue" : doc.SelectSingleNode("//operator")?.InnerText;
                    _preprocesorColor = (doc.SelectSingleNode("//preprocesor")?.InnerText == "default") ? "Brown" : doc.SelectSingleNode("//preprocesor")?.InnerText;
                    _stringColor = (doc.SelectSingleNode("//string")?.InnerText == "default") ? "Gray" : doc.SelectSingleNode("//string")?.InnerText;
                    _commentColor = (doc.SelectSingleNode("//comment")?.InnerText == "default") ? "Green" : doc.SelectSingleNode("//comment")?.InnerText;
                }
                _isXmlChanged = false;
            }
        }

        private static void ColorWordsList(RichTextBox richTextBox, List<string> lista, Color wordsColor)
        {
            foreach (string keyword in lista)
            {
                int index = 0;
                while (index < richTextBox.Text.Length)
                {
                    index = richTextBox.Find(keyword, index, RichTextBoxFinds.WholeWord);
                    if (index == -1)
                        break;

                    richTextBox.SelectionStart = index;
                    richTextBox.SelectionLength = keyword.Length;
                    richTextBox.SelectionColor = wordsColor;
                    index += keyword.Length;
                }
            }
        }
        public static void CompleteHighlight(RichTextBox richTextBox)
        {
            InitColors();
            richTextBox.Visible = false;
            int pozitieInitiala = richTextBox.SelectionStart;
            string text = richTextBox.Text;
            string[] toBeChecked = text.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            /*types*/
            var list = new List<string> { "void", "int", "string", "char", "float", "double", "asm", "auto", "bool", "class", "concept", "const", "enum", "explicit", "export", "extern", "friend", "inline", "long", "mutable", "private", "protected", "public", "register", "short", "signed", "static", "struct", "template", "union", "unsigned", "virtual", "volatile" };
            Color send = ColorTranslator.FromHtml(_typesColor);
            ColorWordsList(richTextBox, list, send);

            /*expressions*/
            list = new List<string> { "and", "and_eq", "break", "case", "catch", "continue", "default", "delete", "do", "dynamic_cast", "else", "false", "for", "goto", "if", "namespace", "new", "not", "not_eq", "nullptr", "operator", "or", "or_eq", "return", "sizeof", "switch", "this", "throw", "true", "try", "typedef", "using", "while", "xor", "xor_eq" };
            send = ColorTranslator.FromHtml(_expressionColor);
            ColorWordsList(richTextBox, list, send);

            /*operators*/
            list = new List<string> { "+", "-", "%", "/", "&", "*", "=", "<", ">" };
            Color opColor = ColorTranslator.FromHtml(_operatorsColor);
            foreach (string keyword in list)
            {
                int indexOp = 0;
                while (indexOp < richTextBox.Text.Length)
                {
                    indexOp = richTextBox.Find(keyword, indexOp, RichTextBoxFinds.MatchCase);
                    if (indexOp == -1)
                        break;

                    richTextBox.SelectionStart = indexOp;
                    richTextBox.SelectionLength = keyword.Length;
                    richTextBox.SelectionColor = opColor;
                    indexOp += keyword.Length;
                }
            }

            /*preprocesor*/
            list = new List<string> { "#define", "#elif", "#else", "#endif", "#error", "#if", "#ifdef", "#ifndef", "#import", "#include", "#line", "#pragma", "#undef", "#using" };
            send = ColorTranslator.FromHtml(_preprocesorColor);
            ColorWordsList(richTextBox, list, send);

            /*strings*/
            string pattern = "(?<!\\\\)\".*?(?<!\\\\)\"";
            Regex regex = new Regex(pattern);
            Color wordsColor = ColorTranslator.FromHtml(_stringColor);
            int index = 0;
            while (index < richTextBox.Text.Length)
            {
                Match match = regex.Match(richTextBox.Text, index);
                if (!match.Success)
                    break;

                int start = match.Index;
                int length = match.Length;
                richTextBox.SelectionStart = start;
                richTextBox.SelectionLength = length;
                richTextBox.SelectionColor = wordsColor;
                index = start + length;
            }

            /*line comments*/
            Color colorCom = ColorTranslator.FromHtml(_commentColor);
            for (int i = 0; i < richTextBox.Lines.Count(); i++)
            {
                if (richTextBox.Lines[i].Contains("//"))
                {
                    int firstIndex = richTextBox.GetFirstCharIndexFromLine(i);
                    int lastIndex = richTextBox.GetFirstCharIndexFromLine(i + 1) - 1;
                    richTextBox.Select(firstIndex + richTextBox.Lines[i].IndexOf("//"), lastIndex - firstIndex -  richTextBox.Lines[i].IndexOf("//"));
                    richTextBox.SelectionColor = colorCom;
                    richTextBox.DeselectAll();
                }
            }

            /*multiline comments*/
            int repeat = 0;
            int startComment = 0;
            int endComment = 0;
            while (startComment != -1 || endComment != -1)
            {
                startComment = richTextBox.Text.IndexOf("/*", repeat);
                endComment = richTextBox.Text.IndexOf("*/", repeat);
                if (startComment == -1 || startComment==0) break;
                if (richTextBox.Text[startComment - 1] == '\"' && richTextBox.Text[startComment + 2] == '\"')
                {
                    repeat = startComment + 1;
                    continue;
                }
                if (richTextBox.Text[endComment - 1] == '\"' && richTextBox.Text[endComment + 2] == '\"')
                {
                    repeat = endComment + 1;
                    continue;
                }
                if (startComment < endComment)
                {
                    richTextBox.Select(startComment, endComment - startComment + 2);
                    richTextBox.SelectionColor = colorCom;
                    richTextBox.DeselectAll();
                    repeat = endComment;
                }
                else if (startComment > endComment)
                {
                    if (repeat == startComment)
                    {
                        richTextBox.Select(startComment, richTextBox.Text.Length - startComment);
                        richTextBox.SelectionColor = colorCom;
                        richTextBox.DeselectAll();
                        break;
                    }
                    repeat = startComment;
                }
            }
            richTextBox.SelectionStart = pozitieInitiala;
            richTextBox.Visible = true;
        }
        public static void LiniarHighLighting(RichTextBoxV2 richTextBox)
        {

            InitColors();
            int initialPos = richTextBox.SelectionStart;
            int currentLineIndex = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart);

            string currentLine = "";
            if (richTextBox != null && richTextBox.SelectionStart < richTextBox.TextLength)
            {
                while (currentLineIndex >= richTextBox.Lines.Length)
                {
                    richTextBox.Text += Environment.NewLine;
                }
                currentLine = richTextBox.Lines[currentLineIndex];
            }

            if (IsSelectionInCommentBlock(richTextBox))
            {
                int selectionStart = richTextBox.SelectionStart;
                int startIndexBlock = richTextBox.Text.LastIndexOf("/*", selectionStart);
                int endIndexBlock = richTextBox.Text.IndexOf("*/", selectionStart);

                richTextBox.Select(startIndexBlock, endIndexBlock - startIndexBlock + 2);
                richTextBox.SelectionColor = ColorTranslator.FromHtml(_commentColor); ;
                richTextBox.DeselectAll();
                richTextBox.SelectionStart = initialPos;
                return;
            }

            int lineStartIndex = richTextBox.GetFirstCharIndexOfCurrentLine();
            int lineEndIndex = richTextBox.GetFirstCharIndexFromLine(richTextBox.GetLineFromCharIndex(lineStartIndex) + 1);

            int startIndex = 0;
            if (lineEndIndex == -1) lineEndIndex = richTextBox.Text.Length;
            currentLine = richTextBox.Text.Substring(lineStartIndex, lineEndIndex - lineStartIndex);

            if (currentLine.Contains("/*"))
            {
                currentLine = richTextBox.Text.Substring(lineStartIndex, currentLine.IndexOf("/*"));
            }
            else if (currentLine.Contains("*/"))
            {
                string comment = "";
                for (int i = 0; i < currentLine.IndexOf("*/"); i++)
                {
                    comment += "_";
                }
                currentLine = comment + richTextBox.Text.Substring(lineStartIndex + currentLine.IndexOf("*/"), lineEndIndex - lineStartIndex - currentLine.IndexOf("*/"));
            }

          
            foreach (string word in currentLine.Replace("\n", " ").Replace("\t"," ").Split(' '))
            {
                if (Array.IndexOf(Types, word) >= 0)
                {
                    startIndex = lineStartIndex + currentLine.IndexOf(word);
                    richTextBox.Select(startIndex, word.Length);
                    richTextBox.SelectionColor = ColorTranslator.FromHtml(_typesColor);
                    richTextBox.DeselectAll();
                }
                else if (Array.IndexOf(Expressions, word) >= 0)
                {
                    startIndex = lineStartIndex + currentLine.IndexOf(word);
                    richTextBox.Select(startIndex, word.Length);
                    richTextBox.SelectionColor = ColorTranslator.FromHtml(_expressionColor);
                    richTextBox.DeselectAll();
                }
                else if (Operators.Any(s => currentLine.Contains(s)))  
                {
                    foreach (string keyword in Operators)
                    {
                        int indexOp = 0;
                        while (indexOp < currentLine.Length)
                        {
                            indexOp = currentLine.IndexOf(keyword, indexOp);
                            if (indexOp == -1)
                                break;

                            startIndex = lineStartIndex + indexOp;
                            richTextBox.Select(startIndex, 1);
                            richTextBox.SelectionColor = ColorTranslator.FromHtml(_operatorsColor);
                            richTextBox.DeselectAll();
                            indexOp++;
                        }
                    }
                 
                }
                else if (Array.IndexOf(Preprocessors, word) >= 0)
                {
                    startIndex = lineStartIndex + currentLine.IndexOf(word);
                    richTextBox.Select(startIndex, word.Length);
                    richTextBox.SelectionColor = ColorTranslator.FromHtml(_preprocesorColor);
                    richTextBox.DeselectAll();
                }
                //else if (word.Length > 2 && word[0] == '\"' && word[word.Length - 1] == '\"')
                //{
                //    startIndex = lineStartIndex + currentLine.IndexOf(word);
                //    richTextBox.Select(startIndex, word.Length);
                //    richTextBox.SelectionColor = ColorTranslator.FromHtml(stringColor);
                //    richTextBox.DeselectAll();

                //}

                
                else
                {
                    if (currentLine.Contains("*/")) { continue; }
                    startIndex = lineStartIndex + currentLine.IndexOf(word);
                    richTextBox.Select(startIndex, word.Length);
                    richTextBox.SelectionColor = Color.Black;
                    richTextBox.DeselectAll();

                    if (Regex.Matches(currentLine, "(?<!\\\\)\".*?(?<!\\\\)\"").Count > 0)
                    {
                        MatchCollection matches = Regex.Matches(currentLine, "(?<!\\\\)\".*?(?<!\\\\)\"");
                        foreach (Match matchy in matches)
                        {
                            startIndex = lineStartIndex + currentLine.IndexOf(matchy.Value);
                            richTextBox.Select(startIndex, matchy.Length);
                            richTextBox.SelectionColor = ColorTranslator.FromHtml(_stringColor);
                        }
                        richTextBox.DeselectAll();
                        richTextBox.SelectionStart = initialPos;
                    }

                    if (currentLine.Contains("//"))
                    {
                        startIndex = lineStartIndex + currentLine.IndexOf("//");
                        richTextBox.Select(startIndex, lineEndIndex - lineStartIndex - currentLine.IndexOf("//"));
                        richTextBox.SelectionColor = ColorTranslator.FromHtml(_commentColor);
                        richTextBox.DeselectAll();
                        richTextBox.SelectionStart = initialPos;
                        continue;
                    }
                }
                
            }
            richTextBox.SelectionStart = initialPos;
        }
        private static bool IsSelectionInCommentBlock(RichTextBox richTextBox)
        {
            int selectionStart = richTextBox.SelectionStart;
            string text = richTextBox.Text;

            int startIndex = text.LastIndexOf("/*", selectionStart);
            int endIndex = text.IndexOf("*/", selectionStart);
            int frontStart = text.IndexOf("/*", selectionStart);
            if (frontStart < endIndex && frontStart != -1) { return false; }

            return startIndex >= 0 && endIndex >= 0 && endIndex > startIndex;
        }
        /// <summary>
        /// Adauga taburi in functie de pozitia cursorului relativ la { }
        /// </summary>
        /// <param name="richTextBox"></param>
        public static void EnterTab(RichTextBox richTextBox)
        {
            int currentPos = richTextBox.SelectionStart;
            string text = richTextBox.Text;
            int braceCount = 0;

            for (int i = 0; i < currentPos; i++)
            {
                if (text[i] == '{')
                {
                    braceCount++;
                }
                else if (text[i] == '}')
                {
                    braceCount--;
                }
            }

            int position = 0;
            string tabs = "";
            while (braceCount > 0)
            {
                tabs += "\t";
                position += 4;
                braceCount--;
            }
            richTextBox.Select(richTextBox.SelectionStart, 0);
            if (tabs == "") { return; }
            Clipboard.SetText(tabs);
            richTextBox.Paste();
        }

        /// <summary>
        ///  Modifica continutul din clipboard cu fontul si culoarea actuala, anuland copy-paste-ul traditional
        /// </summary>
        /// <param name="richTextBox"></param>
        public static void FormatPaste(RichTextBox richTextBox)
        {
            int initialPos = richTextBox.SelectionStart;
            if (Clipboard.ContainsText() && Clipboard.GetText() != "")
            {
                Font font = richTextBox.Font;
                string clipboardText = Clipboard.GetText();

                int selectionStart = richTextBox.SelectionStart;
                int selectionLength = richTextBox.SelectionLength;
                richTextBox.Text = richTextBox.Text.Remove(selectionStart, selectionLength).Insert(selectionStart, clipboardText);

                richTextBox.Select(selectionStart, clipboardText.Length);
                richTextBox.SelectionFont = font;
                richTextBox.DeselectAll();
                initialPos = selectionStart + selectionLength;
                richTextBox.SelectionStart = initialPos;
                richTextBox.SelectionLength = 0;
            }
        }

        public static void CommentUncomment(RichTextBox richTextBox)
        {
            Color currColor = richTextBox.ForeColor;

            if (richTextBox.SelectedText.Contains("/*") && richTextBox.SelectedText.Contains("*/"))
            {
                richTextBox.SelectionColor = currColor;
                richTextBox.SelectedText = richTextBox.SelectedText.Replace("*/", "").Replace("/*", "");
                return;
            }
            richTextBox.SelectionColor = Color.Green;
            richTextBox.SelectedText = "/*" + richTextBox.SelectedText + "*/";
        }
    }
}

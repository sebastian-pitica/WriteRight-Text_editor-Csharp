using CustomControls;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace CommonsModule
{
    /// <summary>
    /// 
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        /// <creator></creator>
        public static TabPage CreateTab(in string tabName)
        {
            TabPage tabPage = new TabPage(tabName);
            TextEditorControl textEditor = new TextEditorControl();
            tabPage.Controls.Add(textEditor);
            return tabPage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="text"></param>
        /// <creator></creator>
        public static void WriteFile(in string filePath, in string text)
        {
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(text);
            streamWriter.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabControl"></param>
        /// <returns></returns>
        /// <creator></creator>
        public static RichTextBoxV2 GetRichTextBoxV2FTabControl(in TabControl tabControl)
        {
            TabPage selectedTabPage = tabControl.SelectedTab;
            TextEditorControl reference = (TextEditorControl)selectedTabPage.Controls[0];
            return reference.RichTextBoxEditor;
        }

        public static TextEditorControl GetTextEditorControlFTabControl(in TabControl tabControl)
        {
            TabPage selectedTabPage = tabControl.SelectedTab;
            TextEditorControl reference = (TextEditorControl)selectedTabPage.Controls[0];
            return reference;
        }

        public static string GetFileNameFromTabControl(in TabControl tabControl)
        {
            RichTextBoxV2 reference = GetRichTextBoxV2FTabControl(tabControl);
            return reference.FilePath ?? tabControl.SelectedTab.Text;
        }
    }
    public static class UtilitiesFormat
    {
        public static bool isXMLChanged = false;
        public static bool isDarkmode = false;
        static string typesColor = "Blue";
        static string expressionColor = "Purple";
        static string operatorsColor = "Blue";
        static string preprocesorColor = "Brown";
        static string stringColor = "Gray";
        static string commentColor = "Green";
        static readonly string[] types = { "void", "int", "string", "char", "float", "double", "asm", "auto", "bool", "class", "concept", "const", "enum", "explicit", "export", "extern", "friend", "inline", "long", "mutable", "private", "protected", "public", "register", "short", "signed", "static", "struct", "template", "union", "unsigned", "virtual", "volatile" };
        static readonly string[] expressions = { "and", "and_eq", "break", "case", "catch", "continue", "default", "delete", "do", "dynamic_cast", "else", "false", "for", "goto", "if", "namespace", "new", "not", "not_eq", "nullptr", "operator", "or", "or_eq", "return", "sizeof", "switch", "this", "throw", "true", "try", "typedef", "using", "while", "xor", "xor_eq" };
        static readonly string[] operators = { "+", "-", "%", "/", "*", "=", "<", ">" };
        static readonly string[] preprocessors = { "#define", "#elif", "#else", "#endif", "#error", "#if", "#ifdef", "#ifndef", "#import", "#include", "#line", "#pragma", "#undef", "#using" };
        internal static void InitColors()
        {
            if (isXMLChanged)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("../../../../colors.xml");
                if (doc.SelectSingleNode("//default").InnerText == "true")
                {
                    typesColor = "Blue";
                    expressionColor = "Red";
                    operatorsColor = "Blue";
                    preprocesorColor = "Brown";
                    stringColor = "Gray";
                    commentColor = "Green";
                }
                else
                {
                    typesColor = (doc.SelectSingleNode("//type").InnerText == "default") ? "Blue" : doc.SelectSingleNode("//type").InnerText;
                    expressionColor = (doc.SelectSingleNode("//expression").InnerText == "default") ? "Red" : doc.SelectSingleNode("//expression").InnerText; ;
                    operatorsColor = (doc.SelectSingleNode("//operator").InnerText == "default") ? "Blue" : doc.SelectSingleNode("//operator").InnerText; ;
                    preprocesorColor = (doc.SelectSingleNode("//preprocesor").InnerText == "default") ? "Brown" : doc.SelectSingleNode("//preprocesor").InnerText; ;
                    stringColor = (doc.SelectSingleNode("//string").InnerText == "default") ? "Gray" : doc.SelectSingleNode("//string").InnerText; ;
                    commentColor = (doc.SelectSingleNode("//comment").InnerText == "default") ? "Green" : doc.SelectSingleNode("//comment").InnerText; ;
                }
                isXMLChanged = false;
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
            Color send = ColorTranslator.FromHtml(typesColor);
            ColorWordsList(richTextBox, list, send);

            /*expressions*/
            list = new List<string> { "and", "and_eq", "break", "case", "catch", "continue", "default", "delete", "do", "dynamic_cast", "else", "false", "for", "goto", "if", "namespace", "new", "not", "not_eq", "nullptr", "operator", "or", "or_eq", "return", "sizeof", "switch", "this", "throw", "true", "try", "typedef", "using", "while", "xor", "xor_eq" };
            send = ColorTranslator.FromHtml(expressionColor);
            ColorWordsList(richTextBox, list, send);

            /*operators*/
            list = new List<string> { "+", "-", "%", "/", "&", "*", "=", "<", ">" };
            Color opColor = ColorTranslator.FromHtml(operatorsColor);
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
            send = ColorTranslator.FromHtml(preprocesorColor);
            ColorWordsList(richTextBox, list, send);

            /*strings*/
            string pattern = "\\\"([^\\\"\\r\\n]*)\\\"";
            Regex regex = new Regex(pattern);
            Color wordsColor = ColorTranslator.FromHtml(stringColor); ;
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
            Color colorCom = ColorTranslator.FromHtml(commentColor);
            for (int i = 0; i < richTextBox.Lines.Count(); i++)
            {
                if (richTextBox.Lines[i].Contains("//"))
                {
                    int firstIndex = richTextBox.GetFirstCharIndexFromLine(i);
                    int lastIndex = richTextBox.GetFirstCharIndexFromLine(i + 1) - 1;
                    richTextBox.Select(firstIndex + richTextBox.Lines[i].IndexOf("//"), lastIndex - firstIndex - richTextBox.Lines[i].IndexOf("//"));
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
                if (startComment == -1) break;
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
                richTextBox.SelectionColor = ColorTranslator.FromHtml(commentColor); ;
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


            foreach (string word in currentLine.Replace("\n", " ").Split(' '))
            {
                if (Array.IndexOf(types, word) >= 0)
                {
                    startIndex = lineStartIndex + currentLine.IndexOf(word);
                    richTextBox.Select(startIndex, word.Length);
                    richTextBox.SelectionColor = ColorTranslator.FromHtml(typesColor);
                    richTextBox.DeselectAll();
                }
                else if (Array.IndexOf(expressions, word) >= 0)
                {
                    startIndex = lineStartIndex + currentLine.IndexOf(word);
                    richTextBox.Select(startIndex, word.Length);
                    richTextBox.SelectionColor = ColorTranslator.FromHtml(expressionColor);
                    richTextBox.DeselectAll();
                }
                else if (operators.Contains(word))
                {
                    startIndex = lineStartIndex + currentLine.IndexOf(word);
                    richTextBox.Select(startIndex, word.Length);
                    richTextBox.SelectionColor = ColorTranslator.FromHtml(operatorsColor);
                    richTextBox.DeselectAll();
                }
                else if (Array.IndexOf(preprocessors, word) >= 0)
                {
                    startIndex = lineStartIndex + currentLine.IndexOf(word);
                    richTextBox.Select(startIndex, word.Length);
                    richTextBox.SelectionColor = ColorTranslator.FromHtml(preprocesorColor);
                    richTextBox.DeselectAll();
                }
                else if (word.Length > 2 && word[0] == '\"' && word[word.Length - 1] == '\"')
                {
                    startIndex = lineStartIndex + currentLine.IndexOf(word);
                    richTextBox.Select(startIndex, word.Length);
                    richTextBox.SelectionColor = ColorTranslator.FromHtml(stringColor);
                    richTextBox.DeselectAll();

                }
                else
                {
                    if (currentLine.Contains("*/")) { continue; }
                    startIndex = lineStartIndex + currentLine.IndexOf(word);
                    richTextBox.Select(startIndex, word.Length);
                    richTextBox.SelectionColor = Color.Black;
                    richTextBox.DeselectAll();

                    if (currentLine.Contains("//"))
                    {
                        startIndex = lineStartIndex + currentLine.IndexOf("//");
                        richTextBox.Select(startIndex, lineEndIndex - lineStartIndex - currentLine.IndexOf("//"));
                        richTextBox.SelectionColor = ColorTranslator.FromHtml(commentColor);
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

using CustomControls;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design;

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
    /// <summary>
    /// 
    /// </summary>
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
        static readonly string[] operators = { "+", "-", "%", "/", "*", "=", "<", ">","&", };
        static readonly string[] preprocessors = { "#define", "#elif", "#else", "#endif", "#error", "#if", "#ifdef", "#ifndef", "#import", "#include", "#line", "#pragma", "#undef", "#using" };
       /// <summary>
       /// Daca s-au facut modificari in fisierul colors.xml, se face update la variabilele specifice culorilor 
       /// </summary>
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
        /// <summary>
        /// Coloreaza lista de cuvinte data ca argument regasita in textbox cu culoarea wordsColor
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="words"></param>
        /// <param name="wordsColor"></param>
        private static void ColorWordsFromRichTextBox(RichTextBox richTextBox, string[] words, Color wordsColor)
        {
            foreach (string keyword in words)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="richTextBox"></param>
        private static void ColorOperatorsFromRichTextBox(RichTextBox richTextBox) {
            Color opColor = ColorTranslator.FromHtml(operatorsColor);
            foreach (string keyword in operators)
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="richTextBox"></param>
        private static void ColorStringsFromRichTextBox(RichTextBox richTextBox)
        {
            int index = 0;
            Color stringsColor = ColorTranslator.FromHtml(stringColor); ;
            Regex regex = new Regex("(?<!\\\\)\".*?(?<!\\\\)\"");
            while (index < richTextBox.Text.Length)
            {
                Match match = regex.Match(richTextBox.Text, index);
                if (!match.Success)
                    break;

                int start = match.Index;
                int length = match.Length;
                richTextBox.SelectionStart = start;
                richTextBox.SelectionLength = length;
                richTextBox.SelectionColor = stringsColor;
                index = start + length;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="richTextBox"></param>
        private static void ColorLineCommentsFromRichTextBox(RichTextBox richTextBox)
        {
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
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="richTextBox"></param>
        private static void ColorMultilineCommentsFromRichTextBox(RichTextBox richTextBox)
        {
            int repeat = 0;
            int startComment = 0;
            int endComment = 0;
            Color colorCom = ColorTranslator.FromHtml(commentColor);
            while (startComment != -1 || endComment != -1)
            {
                startComment = richTextBox.Text.IndexOf("/*", repeat);
                endComment = richTextBox.Text.IndexOf("*/", repeat);
                if (startComment == -1 || startComment == 0) break;
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
                    richTextBox.Select(endComment+2,1);
                    richTextBox.SelectionColor = new Color();
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
        }

        /// <summary>
        /// Se realizeaza highlight pe toate cuvintele cheie: tipuri, expresii, operatori, preprocesor, strings,
        /// comentarii line si multiline. Cat timp se fac aceste procesari, richtextbox devine invizibil.
        /// </summary>
        /// <param name="richTextBox"></param>
        public static void CompleteHighlight(RichTextBox richTextBox)
        {
            InitColors();
            richTextBox.Visible = false;
            int pozitieInitiala = richTextBox.SelectionStart;

            /*types*/
            Color sendColor = ColorTranslator.FromHtml(typesColor);
            ColorWordsFromRichTextBox(richTextBox, types, sendColor);

            /*expressions*/
            sendColor = ColorTranslator.FromHtml(expressionColor);
            ColorWordsFromRichTextBox(richTextBox, expressions, sendColor);

            /*preprocesor*/
            sendColor = ColorTranslator.FromHtml(preprocesorColor);
            ColorWordsFromRichTextBox(richTextBox, preprocessors, sendColor);

            /*operators*/
            ColorOperatorsFromRichTextBox(richTextBox);
           
            /*strings*/
            ColorStringsFromRichTextBox(richTextBox);

            /*line comments*/
            ColorLineCommentsFromRichTextBox(richTextBox);

            /*multiline comments*/
            ColorMultilineCommentsFromRichTextBox(richTextBox);

            richTextBox.SelectionStart = pozitieInitiala;
            richTextBox.Visible = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="richTextBox"></param>
        private static void ColorCommentBlock(RichTextBox richTextBox) {
            int selectionStart = richTextBox.SelectionStart;
            int startIndexBlock = richTextBox.Text.LastIndexOf("/*", selectionStart);
            int endIndexBlock = richTextBox.Text.IndexOf("*/", selectionStart);
            richTextBox.Select(startIndexBlock, endIndexBlock - startIndexBlock + 2);
            richTextBox.SelectionColor = ColorTranslator.FromHtml(commentColor); ;
            richTextBox.DeselectAll();
            richTextBox.Select(endIndexBlock+2, 1);
            richTextBox.SelectionColor = new Color() ;
            richTextBox.DeselectAll();

            richTextBox.SelectionStart = selectionStart;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="lineStartIndex"></param>
        /// <param name="lineEndIndex"></param>
        /// <param name="currentLine"></param>
        private static void CompleteCurrentLine(in RichTextBoxV2 richTextBox, in int lineStartIndex, in int lineEndIndex, ref  string currentLine) {
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
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="lineStartIndex"></param>
        /// <param name="lineEndIndex"></param>
        /// <param name="word"></param>
        /// <param name="currentLine"></param>
        /// <param name="color"></param>
        private static void ColorWordFromCurrentLine(RichTextBox richTextBox, in int lineStartIndex, in int lineEndIndex, in string word, in string currentLine,in string color) {
            var startIndex = lineStartIndex + currentLine.IndexOf(word);
            richTextBox.Select(startIndex, word.Length);
            richTextBox.SelectionColor = ColorTranslator.FromHtml(color);
            richTextBox.DeselectAll();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="lineStartIndex"></param>
        /// <param name="lineEndIndex"></param>
        /// <param name="currentLine"></param>
        private static void ColorOperatorsFromCurrentLine(RichTextBox richTextBox, in int lineStartIndex, in int lineEndIndex, in string currentLine)
        {
            foreach (string op in operators)
            {
                int indexOp = 0;
                while (indexOp < currentLine.Length)
                {
                    indexOp = currentLine.IndexOf(op, indexOp);
                    if (indexOp == -1)
                        break;

                    if ((op == "*" && indexOp > 0 && (currentLine[indexOp - 1] == '/' || currentLine[indexOp + 1] == '/')) || (op == "*" && indexOp == 0 && currentLine[indexOp + 1] == '/'))
                    {
                        indexOp++;
                        continue;
                    }

                    if ((op == "/" && indexOp > 0 && (currentLine[indexOp - 1] == '*' || currentLine[indexOp + 1] == '*')) || (op == "/" && indexOp == 0 && currentLine[indexOp + 1] == '*'))
                    {
                        indexOp++;
                        continue;
                    }

                    var startIndex = lineStartIndex + indexOp;
                    richTextBox.Select(startIndex, 1);
                    richTextBox.SelectionColor = ColorTranslator.FromHtml(operatorsColor);
                    richTextBox.DeselectAll();
                    indexOp++;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="lineStartIndex"></param>
        /// <param name="lineEndIndex"></param>
        /// <param name="currentLine"></param>
        private static void ColorStringsFromCurrentLine(RichTextBox richTextBox, in int lineStartIndex, in int lineEndIndex, in string currentLine)
        {
            MatchCollection matches = Regex.Matches(currentLine, "(?<!\\\\)\".*?(?<!\\\\)\"");
            foreach (Match matchy in matches)
            {
                var startIndex = lineStartIndex + currentLine.IndexOf(matchy.Value);
                richTextBox.Select(startIndex, matchy.Length);
                richTextBox.SelectionColor = ColorTranslator.FromHtml(stringColor);
            }
            richTextBox.DeselectAll();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="lineStartIndex"></param>
        /// <param name="lineEndIndex"></param>
        /// <param name="currentLine"></param>
        private static void ColorLineCommentFromCurrentLine(RichTextBox richTextBox, in int lineStartIndex, in int lineEndIndex, in string currentLine) {
            var startIndex = lineStartIndex + currentLine.IndexOf("//");
            richTextBox.Select(startIndex, lineEndIndex - lineStartIndex - currentLine.IndexOf("//"));
            richTextBox.SelectionColor = ColorTranslator.FromHtml(commentColor);
            richTextBox.DeselectAll();
        }

        /// <summary>
        /// Realizeaza highlight pe toate tpurile de cuvinte cheie de pe linia curenta
        /// </summary>
        /// <param name="richTextBox"></param>
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
                ColorCommentBlock(richTextBox);
                return;
            }

            int lineStartIndex = richTextBox.GetFirstCharIndexOfCurrentLine();
            int lineEndIndex = richTextBox.GetFirstCharIndexFromLine(richTextBox.GetLineFromCharIndex(lineStartIndex) + 1);
            if (lineEndIndex == -1) lineEndIndex = richTextBox.Text.Length; 

            CompleteCurrentLine(in richTextBox,in lineStartIndex,in lineEndIndex,ref currentLine);


            int startIndex = 0;
            foreach (string word in currentLine.Replace("\n", " ").Replace("\t"," ").Split(' '))
            {
                if (Array.IndexOf(types, word) >= 0)
                {
                    ColorWordFromCurrentLine(richTextBox, in lineStartIndex, in lineEndIndex, in word, in currentLine,in typesColor);
                }
                else if (Array.IndexOf(expressions, word) >= 0)
                {
                    ColorWordFromCurrentLine(richTextBox, in lineStartIndex, in lineEndIndex, in word, in currentLine,in expressionColor);
                }
                else if (Array.IndexOf(preprocessors, word) >= 0)
                {
                    ColorWordFromCurrentLine(richTextBox, in lineStartIndex, in lineEndIndex, in word, in currentLine,in preprocesorColor);
                }
                else
                {
                    if (word.EndsWith("*/"))  continue; 
                    startIndex = lineStartIndex + currentLine.IndexOf(word);
                    richTextBox.Select(startIndex, word.Length);
                    richTextBox.SelectionColor = new Color();
                    richTextBox.DeselectAll();

                    if (Regex.Matches(currentLine, "(?<!\\\\)\".*?(?<!\\\\)\"").Count > 0)
                    {
                        ColorStringsFromCurrentLine(richTextBox, in lineStartIndex, in lineEndIndex, in currentLine);
                        richTextBox.SelectionStart = initialPos;
                    }
                }
                if (operators.Any(s => currentLine.Contains(s)))
                {
                    ColorOperatorsFromCurrentLine(richTextBox, in  lineStartIndex, in  lineEndIndex, in  currentLine);
                }
                if (currentLine.Contains("//"))
                {
                    ColorLineCommentFromCurrentLine(richTextBox, in lineStartIndex, in lineEndIndex, in currentLine);
                    richTextBox.SelectionStart = initialPos;
                }
            }
            richTextBox.SelectionStart = initialPos;
        }

        /// <summary>
        /// Verifica daca Selection este intr-un multiline commment block
        /// </summary>
        /// <param name="richTextBox"></param>
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

        /// <summary>
        /// Comenteaza cu /* */ textul selectat si decomenteaza textul selectat si pozitionat intre /* */
        /// </summary>
        /// <param name="richTextBox"></param>
        public static void CommentUncomment(RichTextBox richTextBox)
        {
            Color currColor = richTextBox.ForeColor;

            if (richTextBox.SelectedText.Contains("/*") && richTextBox.SelectedText.Contains("*/"))
            {
                richTextBox.SelectionColor = currColor;
                richTextBox.SelectedText = richTextBox.SelectedText.Replace("*/", "").Replace("/*", "");
                return;
            }
            int firstIndex = richTextBox.SelectionStart;
            int lastIndex = richTextBox.SelectionStart + richTextBox.SelectionLength + 4;
            richTextBox.SelectionColor = ColorTranslator.FromHtml(commentColor);
            richTextBox.SelectedText = "/*" + richTextBox.SelectedText + "*/";
            richTextBox.DeselectAll();
            richTextBox.Select(firstIndex - 1, 1);
            richTextBox.SelectionColor = new Color();
            richTextBox.DeselectAll();
            richTextBox.Select(lastIndex, 1);
            richTextBox.SelectionColor = new Color();
            richTextBox.DeselectAll();
        }
    }
}

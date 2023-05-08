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
 *  Copyright:   (c) 2023, Caulea Vasile, Pitica Sebastian, Matei Rares   *
 *  Description: Fișierul conține clasele cu caracter general utilizate   *
 *  în cadrul proiectului, separate pentru a putea fi folosit pe tot      *
 *  cuprinsul celorlalte module sau pentru că nu pot fi asociate cu alt   *
 *  modul concret.                                                        *
 *  Designed by: Pitica Sebastian                                         *
 *  Updated by: Pitica Sebastian, Matei Rares                             *
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
            "C/C++/C# Files (*.c, *.cpp, *.cs)|*.c;*.cpp;*.cs",
            "Text Files(*.txt)|*.txt",
            "All Files(*.*)|*.*"
        };

        #region <updated>Matei Rares</updated>
        /// <summary>
        /// Creaza un TabPage care contine un element TextEditorControl.
        /// </summary>
        /// <param name="tabName">textul care va fi afisat in TabPage</param>
        /// <returns>TabPage</returns>
        public static TabPage CreateTab(in string tabName)
        {
            TabPage tabPage = new TabPage(tabName)
            {
                BorderStyle = BorderStyle.None
            };
            TextEditorControl textEditor = new TextEditorControl();
            tabPage.Controls.Add(textEditor);

            if (UtilitiesFormat.isDarkmode)
            {
                tabPage.BackColor = ColorTranslator.FromHtml("#24292E");
                tabPage.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                ((RichTextBoxV2)textEditor.RichTextBoxEditor).BackColor = ColorTranslator.FromHtml("#24292E");
                ((RichTextBoxV2)textEditor.RichTextBoxEditor).ForeColor = ColorTranslator.FromHtml("#C8D3DA");
                textEditor.BackColor = ColorTranslator.FromHtml("#C8D3DA");

                textEditor.RichTextBoxNumbering.BackColor = ColorTranslator.FromHtml("#24292E");
                textEditor.RichTextBoxNumbering.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
            }

            return tabPage;
        }
        #endregion
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

        /// <summary>
        /// Obtine calea/numele fisierului deschis in tab-ul curent
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="isFilePath"></param>
        /// <returns>Un string care reprezinta calea/numele fisierului</returns>
        public static string GetFileNameFromTabControl(in TabControl tabControl, bool isFilePath)
        {
            RichTextBoxV2 reference = GetRichTextBoxV2FromTabControl(tabControl);
            string fileName = reference.FileName ?? tabControl.SelectedTab.Text.Replace("* ", "");
            string filePath = reference.FilePath ?? fileName;
            return isFilePath ? filePath : fileName;
        }

        public static void HandleException(Exception ex)
        {
            MessageBox.Show(ex.ToString(), "- EXCEPTION -", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    #endregion
    #endregion

    #region <creator>Matei Rares</creator>
      /// <summary>
    /// Această clasă este utilizată pentru a face highlight pe cuvintele cheie dintr-un fisier C/C++/C#.
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
        static readonly string[] types = { "void", "int", "string", "char", "float", "double", "asm", "auto", "bool", "class", "concept", "const", "enum", "explicit", "export", "extern", "friend", "inline", "long", "mutable", "private", "private:", "protected", "protected:", "public", "public:", "register", "short", "signed", "static", "struct", "template", "union", "unsigned", "virtual", "volatile" };
        static readonly string[] expressions = { "and", "and_eq", "break", "case", "catch", "continue", "default", "delete", "do", "dynamic_cast", "else", "false", "for", "goto", "if", "namespace", "new", "not", "not_eq", "nullptr", "operator", "or", "or_eq", "return", "sizeof", "switch", "this", "throw", "true", "try", "typedef", "using", "while", "xor", "xor_eq" };
        static readonly string[] operators = { "+", "-", "%", "/", "*", "=", "<", ">", "&", };
        static readonly string[] preprocessors = { "#define", "#elif", "#else", "#endif", "#error", "#if", "#ifdef", "#ifndef", "#import", "#include", "#line", "#pragma", "#undef", "#using" };

        /// <summary>
        /// Această funcție verifică dacă s-au făcut modificări în fișierul colors.xml, în caz că s-au făcut,
        /// se face update la variabilele specifice culorilor.
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
        /// Această funcție colorează cuvintele din vector, găsite în text, în culoarea trimisă ca argument.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezinta Controlul în a cărui text se caută și se colorează cuvintele</param>
        /// <param name="words">Parametrul reprezintă vectorul de cuvinte căutate in text</param>
        /// <param name="wordsColor">Paremetrul reprezintă culoarea pe care o vor avea cuvintele din text</param>
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
        /// Coloreaza operatorii dintr-un RichTextBox în culoarea specifică operatorilor.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezinta Controlul în a cărui text se caută și se colorează operatorii</param>
        private static void ColorOperatorsFromRichTextBox(RichTextBox richTextBox)
        {
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
        /// Coloreaza operatorii dintr-un RichTextBox în culoarea specifică operatorilor.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezinta Controlul în a cărui text se caută și se colorează stringurile</param>
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
        /// Coloreaza comentariile liniare (care incep cu "//") dintr-un RichTextBox în culoarea specifică comentariilor liniare.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezinta Controlul în a cărui text se caută și se colorează comentariile liniare</param>
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
        /// Coloreaza tot continutul aflat intre caracterele "/*" si "*/" dintr-un RichTextBox în culoarea specifică comentariilor multiple.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezinta Controlul în a cărui text se caută și se colorează comentariile multiple</param>
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
                    richTextBox.Select(endComment + 2, 1);
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
        /// Funcția realizează highlight pe toate tipuri speciale de cuvinte: tipuri, expresii, operatori, preprocesor, strings,
        /// comentarii line si multiline. Cât timp se fac aceste procesari, richtextbox-ul devine invizibil.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezinta Controlul în a cărui text se caută și se colorează comentariile multiple</param>
        public static void CompleteHighlight(RichTextBox richTextBox)
        {
            try
            {
                InitColors();
            }
            catch (Exception ex) {
                Utilities.HandleException(ex);
            }

            richTextBox.Visible = false;
            int pozitieInitiala = richTextBox.SelectionStart;

            try
            {
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
            }
            catch (Exception ex) {
                Utilities.HandleException(ex);
            }

            richTextBox.SelectionStart = pozitieInitiala;
            richTextBox.Visible = true;
        }
        /// <summary>
        /// Aceasta funcție colorează comment block-ul în care se află Selection.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezintă Control-ul în a cărui text se caută și se coloreaza comment block-ul</param>
        private static void ColorCommentBlock(RichTextBox richTextBox)
        {
            int selectionStart = richTextBox.SelectionStart;
            int startIndexBlock = richTextBox.Text.LastIndexOf("/*", selectionStart);
            int endIndexBlock = richTextBox.Text.IndexOf("*/", selectionStart);
            richTextBox.Select(startIndexBlock, endIndexBlock - startIndexBlock + 2);
            richTextBox.SelectionColor = ColorTranslator.FromHtml(commentColor); ;
            richTextBox.DeselectAll();
            richTextBox.Select(endIndexBlock + 2, 1);
            richTextBox.SelectionColor = new Color();
            richTextBox.DeselectAll();

            richTextBox.SelectionStart = selectionStart;
        }
        /// <summary>
        /// Modifica linia curenta pentru a se poate face si alte procesari pe liniile cu comentarii.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezintă Controlul in a carui text se fac procesarile</param>
        /// <param name="lineStartIndex">Parametrul reprezintă indexul de start a liniei curente din textul RichTextBox-ului.</param>
        /// <param name="lineEndIndex">Parametrul reprezintă indexul de sfârșit a liniei curente din textul RichTextBox-ului.</param>
        /// <param name="currentLine">Parametrul reprezinta conținutul liniei curente</param>
        private static void CompleteCurrentLine(in RichTextBoxV2 richTextBox, in int lineStartIndex, in int lineEndIndex, ref string currentLine)
        {
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
        /// Coloreaza cuvantul regasit pe linia curenta dintr-un RichTextBox, indecsii sunt folositi la gasirea liniei curente.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezintă Controlul in a carui text se fac procesarile</param>
        /// <param name="lineStartIndex">Parametrul reprezintă indexul de start a liniei curente din textul RichTextBox-ului.</param>
        /// <param name="lineEndIndex">Parametrul reprezintă indexul de sfârșit a liniei curente din textul RichTextBox-ului.</param>
        /// <param name="word">Parametrul reprezintă cuvântul căutat in text</param>
        /// <param name="currentLine">Parametrul reprezinta conținutul liniei curente</param>
        /// <param name="color">Parametrul reprezintă culoarea pe care o va avea cuvantul cautat</param>
        private static void ColorWordFromCurrentLine(RichTextBox richTextBox, in int lineStartIndex, in int lineEndIndex, in string word, in string currentLine, in string color)
        {
            var startIndex = lineStartIndex + currentLine.IndexOf(word);
            richTextBox.Select(startIndex, word.Length);
            richTextBox.SelectionColor = ColorTranslator.FromHtml(color);
            richTextBox.DeselectAll();
        }
        /// <summary>
        /// Coloreaza operatorii aflati pe linia curenta dintr-un RichTextBox, indecsii sunt folositi la gasirea liniei curente. 
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezintă Controlul in a carui text se fac procesarile</param>
        /// <param name="lineStartIndex">Parametrul reprezintă indexul de start a liniei curente din textul RichTextBox-ului.</param>
        /// <param name="lineEndIndex">Parametrul reprezintă indexul de sfârșit a liniei curente din textul RichTextBox-ului.</param>
        /// <param name="currentLine">Parametrul reprezinta conținutul liniei curente</param>
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
        /// Coloreaza continutul aflat intre ghilimele aflate pe linia curenta dintr-un RichTextBox, indecsii sunt folositi la gasirea liniei curente.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezintă Controlul in a carui text se fac procesarile</param>
        /// <param name="lineStartIndex">Parametrul reprezintă indexul de start a liniei curente din textul RichTextBox-ului.</param>
        /// <param name="lineEndIndex">Parametrul reprezintă indexul de sfârșit a liniei curente din textul RichTextBox-ului.</param>
        /// <param name="currentLine">Parametrul reprezinta conținutul liniei curente</param>
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
        /// Coloreaza continutul aflat dupa "//" de pe linia curenta dintr-un RichTextBox, indecsii sunt folositi la gasirea liniei curente. 
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezintă Controlul in a carui text se fac procesarile</param>
        /// <param name="lineStartIndex">Parametrul reprezintă indexul de start a liniei curente din textul RichTextBox-ului.</param>
        /// <param name="lineEndIndex">Parametrul reprezintă indexul de sfârșit a liniei curente din textul RichTextBox-ului.</param>
        /// <param name="currentLine">Parametrul reprezinta conținutul liniei curente</param>
        private static void ColorLineCommentFromCurrentLine(RichTextBox richTextBox, in int lineStartIndex, in int lineEndIndex, in string currentLine)
        {
            var startIndex = lineStartIndex + currentLine.IndexOf("//");
            richTextBox.Select(startIndex, lineEndIndex - lineStartIndex - currentLine.IndexOf("//"));
            richTextBox.SelectionColor = ColorTranslator.FromHtml(commentColor);
            richTextBox.DeselectAll();
        }

        /// <summary>
        /// Se realizeaza highlight pe toate tipuri speciale de cuvinte aflate pe linia curenta: tipuri, expresii,
        /// operatori, preprocesor, strings,comentarii line si multiline.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezintă Controlul in a carui text se fac procesarile</param>
        public static void LiniarHighLighting(RichTextBoxV2 richTextBox)
        {
            try
            {
                InitColors();
            }
            catch (Exception ex)
            {
                Utilities.HandleException(ex);
            }
            int initialPos = richTextBox.SelectionStart;
            int currentLineIndex = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart);

            string currentLine = "";
            try
            {

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

                CompleteCurrentLine(in richTextBox, in lineStartIndex, in lineEndIndex, ref currentLine);


                int startIndex = 0;
                foreach (string word in currentLine.Replace("\n", " ").Replace("\t", " ").Split(' '))
                {
                    if (Array.IndexOf(types, word) >= 0)
                    {
                        ColorWordFromCurrentLine(richTextBox, in lineStartIndex, in lineEndIndex, in word, in currentLine, in typesColor);
                    }
                    else if (Array.IndexOf(expressions, word) >= 0)
                    {
                        ColorWordFromCurrentLine(richTextBox, in lineStartIndex, in lineEndIndex, in word, in currentLine, in expressionColor);
                    }
                    else if (Array.IndexOf(preprocessors, word) >= 0)
                    {
                        ColorWordFromCurrentLine(richTextBox, in lineStartIndex, in lineEndIndex, in word, in currentLine, in preprocesorColor);
                    }
                    else
                    {
                        if (word.EndsWith("*/")) continue;
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
                        ColorOperatorsFromCurrentLine(richTextBox, in lineStartIndex, in lineEndIndex, in currentLine);
                    }
                    if (currentLine.Contains("//"))
                    {
                        ColorLineCommentFromCurrentLine(richTextBox, in lineStartIndex, in lineEndIndex, in currentLine);
                        richTextBox.SelectionStart = initialPos;
                    }
                }
            }
            catch (Exception ex) {
                Utilities.HandleException(ex);
            }
            richTextBox.SelectionStart = initialPos;
        }

        /// <summary>
        /// Verifica daca Selection este intr-un commment block
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezintă Controlul in a carui text se fac procesarile</param>
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
        /// Adauga taburi in functie de pozitia cursorului relativ la "{" si "}"
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezintă Controlul in a carui text se fac procesarile</param>
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
        /// Modifica continutul din clipboard cu fontul si culoarea actuala.
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezintă Controlul in a carui text se fac procesarile</param>
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
        /// Comenteaza cu "/"* respectiv "*/" textul selectat si decomenteaza textul selectat si pozitionat intre "/*" si "*/"
        /// </summary>
        /// <param name="richTextBox">Parametrul reprezintă Controlul in a carui text se fac procesarile</param>
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
    #endregion
}

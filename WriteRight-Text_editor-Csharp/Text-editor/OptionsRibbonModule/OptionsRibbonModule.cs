using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using CommonsModule;
using Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OptionsRibbonModule
{
    public class SyntaxCheckerCommand : IMainTextBoxCommand
    {
        private static SyntaxCheckerCommand _singletonInstance = null;
        private RichTextBoxV2 _mainTextBoxRef;
        private SyntaxCheckerCommand()
        {

        }

        public static new SyntaxCheckerCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new SyntaxCheckerCommand();
            }
            return _singletonInstance;
        }

        public override async void Execute()
        {
            switch (_mainTextBoxRef.FileType)
            {
                case ".c":
                case ".cpp":
                    //string fileContent = _mainTextBoxRef.Text;
                    //// Crearea unui obiect ANTLR InputStream pe baza conținutului fișierului
                    //AntlrInputStream inputStream = new AntlrInputStream(fileContent);

                    //// Crearea unui obiect CPP14Lexer pe baza stream-ului de intrare ANTLR
                    //CPP14Lexer lexer = new CPP14Lexer(inputStream);

                    //// Crearea unui obiect CommonTokenStream pe baza lexer-ului CPP14Lexer
                    //CommonTokenStream tokenStream = new CommonTokenStream(lexer);

                    //// Crearea unui obiect CPP14Parser pe baza stream-ului de token ANTLR
                    //CPP14Parser parser = new CPP14Parser(tokenStream);

                    //// Adăugarea unui error listener pentru a captura erorile de sintaxă
                    //SyntaxErrorListener errorListener = new SyntaxErrorListener();
                    //parser.AddErrorListener(errorListener);

                    //// Pornirea procesului de parsare a fișierului
                    //IParseTree tree = parser.translationunit();

                    string codSursa = _mainTextBoxRef.Text;

                    string endpoint = "https://godbolt.org/api/compiler/g63/compile";

                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    byte[] codSursaBytes = Encoding.UTF8.GetBytes(codSursa);
                    var content = new ByteArrayContent(codSursaBytes);
                    content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                    // Crearea cererii HTTP
                    var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                    {
                        Content = content
                    };

                    var raspuns = await httpClient.SendAsync(request);

                    if (raspuns.IsSuccessStatusCode)
                    {
                        var continutRaspuns = await raspuns.Content.ReadAsStringAsync();

                        continutRaspuns = Regex.Replace(continutRaspuns, @"\\u001b\[[0-9;]*[mGKHF]", string.Empty);

                        var raspunsJson = System.Text.Json.JsonSerializer.Deserialize<dynamic>(continutRaspuns);

                        // Verificarea daca compilarea a esuat si afisarea mesajelor de eroare
                        if (raspunsJson.GetProperty("code").GetInt32() != 0)
                        {
                            string s = "";
                            foreach (var mesaj in raspunsJson.GetProperty("stderr").EnumerateArray())
                            {
                                s += mesaj.GetProperty("text").GetString();
                            }
                            MessageBox.Show(s);
                        }
                        else
                        {
                            Console.WriteLine("Sintaxa corecta!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Eroare de comunicare cu serverul Compiler Explorer.");
                    }
                    break;
                default:
                    break;
            }
        }

        public override void SetTarget(RichTextBoxV2 mainTextBox)
        {
            _mainTextBoxRef = mainTextBox;
        }
    }

    public class SyntaxErrorListener : BaseErrorListener
    {
        public bool HasErrors { get; private set; }

        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            HasErrors = true;
            Console.WriteLine($"Eroare de sintaxă la linia {line}, poziția {charPositionInLine}: {msg}");
        }
    }
}

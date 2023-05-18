using CustomControls;
using FormatRibbonModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using TextEditor;
using CommonsModule;
using System.Drawing;

/**************************************************************************
 *                                                                        *
 *  File:        UnitTestFormatMenu.cs                                    *
 *  Copyright:   (c) 2023, Matei Rares                                    *
 *  Description: Fișierul conține funcții de test pentru funcționalitățile*
 *  moduluilui Format                                                     *               
 *                                                                        * 
 **************************************************************************/

namespace UnitTestProjectFormatModule
{
    [TestClass]
    public class UnitTestFormatModule
    {
        private readonly FormMainWindow _formMainWindow = new FormMainWindow();
        private RichTextBoxV2 _mainTextBox;
        private string _defaultText = "#include <iostream>\r\nusing namespace std ;\r\nclass Azkeban{\r\n    public:\r\n        enum nu_stiu{da,nu};\r\n\r\n    private:\r\n        virtual ~Azkeban(){};\r\n        friend int calcan();\r\n}\r\n\r\nstatic double apel(int* parameter1, int& parameter2){}\r\n\r\nint main(){\r\n    int test=0;\r\n    double test1=0.0;\r\n    float test2=2f;\r\n    float test3=0x23;\r\n    float test4=0b23;\r\n    char nu_conteaza=test1 + test2 ;\r\n\r\n    apel(test,&test1);\r\n\r\n    cout<<\"Sa asfaltu \\n  ce de-a culori\";\r\n    cin>>test;\r\n\t\r\n    if(test){\r\n        //Aicea acoladele is portocalii\r\n        //Comentariu gri, nu vreau \r\n    }\r\n\r\n}";
        private string _expectedResult = "#include <iostream> using namespace std ;\n class Azkeban\n{\n\t public: enum nu_stiu\n\t{\n\t\tda,nu\n\t};\n\t private: virtual ~Azkeban()\n\t{\n\t};\n\t friend int calcan();\n}\n static double apel(int* parameter1, int& parameter2)\n{\n}\n int main()\n{\n\t int test=0;\n\t double test1=0.0;\n\t float test2=2f;\n\t float test3=0x23;\n\t float test4=0b23;\n\t char nu_conteaza=test1 + test2 ;\n\t apel(test,&test1);\n\t cout<<\"Sa asfaltu \\n ce de-a culori\";\n\t cin>>test;\n\t if(test)\n\t{\n\t\t //Aicea acoladele is portocalii //Comentariu gri, nu vreau \n\t}\n}\n";
        
        [TestInitialize]
        public void Setup()
        {
            FieldInfo mainTextBoxField = _formMainWindow.GetType().GetField("_richTextBoxMainV2", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mainTextBoxField != null) _mainTextBox = (RichTextBoxV2)mainTextBoxField.GetValue(_formMainWindow);
        
        }

        [TestMethod]
        public void TestMethodColorForm()
        {
            bool isDark = UtilitiesFormat.IsDarkMode;
            ThemeCommand command = ThemeCommand.GetCommandObj();
            Color currentBack=_formMainWindow.BackColor;
            command.SetTarget(_formMainWindow);
            command.Execute();
           

            Assert.AreEqual(!isDark, UtilitiesFormat.IsDarkMode);
            Assert.AreNotEqual(currentBack, _formMainWindow.BackColor);
        }

        [TestMethod]
        public void TestMethodSyntaxForm()
        {
            bool isChanged = UtilitiesFormat.IsXmlChanged;
            SyntaxHighlightCommand command = SyntaxHighlightCommand.GetCommandObj();
            command.SetTarget(_mainTextBox);
            command.Execute();

            Assert.AreNotEqual(isChanged, UtilitiesFormat.IsXmlChanged);
        }

        [TestMethod]
        public void TestMethodFormatForm()
        {
            _mainTextBox.Text = _defaultText;
            FormatDocument command = FormatDocument.GetCommandObj();
            command.SetTarget(_mainTextBox);
            command.Execute();
            Assert.AreEqual(_expectedResult, _mainTextBox.Text);
        }
    }
}

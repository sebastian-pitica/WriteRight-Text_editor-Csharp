using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using CommonsModule;
using System.Threading;
using System.Runtime.CompilerServices;

namespace FormatRibbonModule
{
    public class ColorCommand : ITotalCommand
    {

        internal class DarkTitleBarClass
        {
            [DllImport("dwmapi.dll")]
            private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

            public static void SetDarkTitleBar(IntPtr handle)
            {
                int preference = 1;
                
                    DwmSetWindowAttribute(handle, 20, ref preference, sizeof(int));
                
            }
        }
        private static ColorCommand _singletonInstance = null;

        internal Form _mainFormWindowRef;
        internal RichTextBoxV2 _mainRichTextBoxV2Ref;
        internal StatusStrip _mainStatusStripRef;
        internal MenuStrip _mainMenuStripRef;
        internal RichTextBox _mainRichTextBoxNumbersRef;
                                                      
        internal Label _mainLabel2Ref;
        internal TextBox _mainTextBoxLinesNrRef;
        internal Label _mainLabel1Ref;
        internal TextBox _mainTextBoxWordsNrRef;
        internal Button _mainButtonZoomOutRef;
        internal Button _mainButtonZoomInRef;


        internal List<Control> _controls= new List<Control>();

        public static bool isDarkmode=false;
      
        private ColorCommand()
        {

        }

        public static new ColorCommand GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new ColorCommand();
            }
            return _singletonInstance;
        }
   

        public override void Execute()
        {

            if (!isDarkmode)
            {
                DarkTitleBarClass.SetDarkTitleBar(_mainFormWindowRef.Handle);

                //text
                _mainRichTextBoxV2Ref.baseComponent.BackColor = ColorTranslator.FromHtml("#24292E");
                _mainRichTextBoxV2Ref.baseComponent.ForeColor = ColorTranslator.FromHtml("#E1E4E8");
                //menu
                _mainMenuStripRef.BackColor = ColorTranslator.FromHtml("#1F2428");
                //statusbar
                _mainStatusStripRef.BackColor= ColorTranslator.FromHtml("#24292E");
                _mainStatusStripRef.ForeColor = ColorTranslator.FromHtml("#C8D3DA");
 
                _mainLabel2Ref.BackColor = ColorTranslator.FromHtml("#24292E"); ;
                _mainTextBoxLinesNrRef.BackColor = ColorTranslator.FromHtml("#24292E"); ;
                _mainLabel1Ref.BackColor = ColorTranslator.FromHtml("#24292E"); ;
                _mainTextBoxWordsNrRef.BackColor = ColorTranslator.FromHtml("#24292E"); ;

                _mainLabel2Ref.ForeColor = ColorTranslator.FromHtml("#C8D3DA"); ;
                _mainTextBoxLinesNrRef.ForeColor = ColorTranslator.FromHtml("#C8D3DA"); ;
                _mainLabel1Ref.ForeColor = ColorTranslator.FromHtml("#C8D3DA"); ;
                _mainTextBoxWordsNrRef.ForeColor = ColorTranslator.FromHtml("#C8D3DA"); ;

                _mainButtonZoomOutRef.BackColor = ColorTranslator.FromHtml("#24292E"); ;
                _mainButtonZoomInRef.BackColor = ColorTranslator.FromHtml("#24292E"); ;
                _mainButtonZoomOutRef.ForeColor = ColorTranslator.FromHtml("#C8D3DA"); ;
                _mainButtonZoomInRef.ForeColor = ColorTranslator.FromHtml("#C8D3DA"); ;
                _mainButtonZoomOutRef.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#E1E4E8"); 
                _mainButtonZoomInRef.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#E1E4E8"); 

                _mainFormWindowRef.BackColor= ColorTranslator.FromHtml("#1F2428");

                _mainRichTextBoxNumbersRef.BackColor = ColorTranslator.FromHtml("#1F2428");   
                _mainRichTextBoxNumbersRef.ForeColor = ColorTranslator.FromHtml("#E1E4E8");


                foreach (ToolStripMenuItem item in _mainMenuStripRef.Items)
                {

                    item.BackColor = ColorTranslator.FromHtml("#1F2428");
                    item.ForeColor = ColorTranslator.FromHtml("#D7DADE");
                  

                    foreach (ToolStripItem subItem in item.DropDownItems)
                    {
                        subItem.BackColor = ColorTranslator.FromHtml("#1F2428");
                        subItem.ForeColor = ColorTranslator.FromHtml("#D7DADE");
                    }
                }

                isDarkmode = true;
            }
            else {
               

                foreach (Control control in this._controls)
                { 
                control.BackColor= new System.Drawing.Color(); ;
                    control.ForeColor=new System.Drawing.Color(); ;

                }

                _mainFormWindowRef.BackColor = new System.Drawing.Color(); ;
             
                foreach (ToolStripMenuItem item in _mainMenuStripRef.Items)
                {
                    item.BackColor = new System.Drawing.Color(); ;
                    item.ForeColor = new System.Drawing.Color(); ;
                    foreach (ToolStripItem subItem in item.DropDownItems)
                    {
                        subItem.BackColor = new System.Drawing.Color(); ;
                        subItem.ForeColor =new System.Drawing.Color();; 
                    }
                }
                isDarkmode = false;
            }
        }

        public override void SetTargets(Form mainWindow,Control.ControlCollection controls)
        {
            
            _mainFormWindowRef = mainWindow;
            _mainRichTextBoxNumbersRef = (RichTextBox)controls[0];
            _mainLabel2Ref = (Label)controls[1];
            _mainTextBoxLinesNrRef = (TextBox)controls[2];
            _mainLabel1Ref = (Label)controls[3];
            _mainTextBoxWordsNrRef = (TextBox)controls[4];
            _mainButtonZoomOutRef = (Button)controls[5];
            _mainButtonZoomInRef = (Button)controls[6];
            _mainStatusStripRef = (StatusStrip)controls[7];
   
            _mainMenuStripRef = (MenuStrip)controls[9];
            _mainRichTextBoxV2Ref = (RichTextBoxV2)controls[10];

            for (int i = 0; i < 11; i++) {
                _controls.Add(controls[i]);
            
            }
        }

      
    }


    public class FontCommand : IMainTextBoxCommand
    {
        private static FontCommand _singletonInstance = null;
        internal RichTextBoxV2 _mainRichTextBoxV2Ref;

        private FontCommand()
        {

        }

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
            
            ColorDialog colorDialog = new ColorDialog();
            DialogResult colorResult = DialogResult.Cancel;
            Thread colorThread = new Thread(() => {
                colorResult = colorDialog.ShowDialog();
            });
            colorThread.SetApartmentState(ApartmentState.STA);
            colorThread.Start();

            FontDialog fontDialog = new FontDialog();
           
            DialogResult fontResult = fontDialog.ShowDialog();
           
            
            while (colorThread.IsAlive)
            {
                Application.DoEvents();
                if (fontResult == DialogResult.OK || fontResult != DialogResult.OK) {
                    try
                    {
                        colorThread.Abort();
                    }
                    catch (ThreadAbortException ex)
                    {
                       
                    }
                }
            }
            
            if (fontResult == DialogResult.OK && colorResult == DialogResult.OK)
            {
                _mainRichTextBoxV2Ref.baseComponent.SelectionFont = fontDialog.Font;
                _mainRichTextBoxV2Ref.baseComponent.SelectionColor = colorDialog.Color;
            }            
        }

        public override void SetTarget(RichTextBoxV2 mainTextBox)
        {
            _mainRichTextBoxV2Ref = mainTextBox;
          
        }
    }

}





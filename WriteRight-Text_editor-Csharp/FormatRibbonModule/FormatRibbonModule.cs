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
    public class ColorCommand : IMainWindowCommand
    {

        
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        public static void SetDarkTitleBar(IntPtr handle)
        {
            int preference = 1;
                
                DwmSetWindowAttribute(handle, 20, ref preference, sizeof(int));
                
        }
        
        private static ColorCommand _singletonInstance = null;
        internal Form _mainFormWindowRef;
        internal List<Control> _controls= new List<Control>();
        static public bool isDarkmode=false;
      
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
                       /* case "RichTextBoxV2":

                            _controls[i].baseComponent.BackColor = ColorTranslator.FromHtml("#24292E");
                            _controls[i].baseComponent.ForeColor = ColorTranslator.FromHtml("#E1E4E8");
                            break;*/
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
                    if (_controls[i] is MenuStrip) {
                        foreach (ToolStripMenuItem item in ((MenuStrip)_controls[i]).Items)
                        {
                            item.BackColor = new Color(); ;
                            item.ForeColor = new Color(); ;
                            foreach (ToolStripItem subItem in item.DropDownItems)
                            {
                                subItem.BackColor = new Color(); ;
                                subItem.ForeColor = new Color(); ;
                            }
                        }

                    }
                
                }
                
                isDarkmode = false;
            }
        }

        public override void SetTarget(Form mainWindow)
        {
            
            _mainFormWindowRef = mainWindow;
            /*
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
            */

            for (int i = 0; i < mainWindow.Controls.Count; i++) {
                _controls.Add(mainWindow.Controls[i]);
            
            }
        }

      
    }
    public class ColorPreferences : IMainWindowCommand
    {
        internal enum preference { Red, Yellow, Purple, Green, Blue };
        private static ColorPreferences _singletonInstance = null;
        internal Form _mainFormWindowRef;
        internal List<Control> _controls = new List<Control>();
        internal bool isDarkmode = false;


        private ColorPreferences(){}
        public static new ColorPreferences GetCommandObj()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new ColorPreferences();
            }
            return _singletonInstance;
        }

        public override void Execute()
        {
            //get color
            int color = 0;
            DrawColors(color);
        }

        private void DrawColors(int preference) { 
        
            
        
        }

        public override void SetTarget(Form mainWindow)
        {
            throw new NotImplementedException();
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
                _mainRichTextBoxV2Ref.baseComponent.SelectionFont = fontDialog.Font;
               // _mainRichTextBoxV2Ref.baseComponent.SelectionColor = colorDialog.Color;
            }            
        }

        public override void SetTarget(RichTextBoxV2 mainTextBox)
        {
            _mainRichTextBoxV2Ref = mainTextBox;
          
        }
    }

}





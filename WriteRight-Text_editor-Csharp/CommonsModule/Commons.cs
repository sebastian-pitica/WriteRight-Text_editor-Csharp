using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**************************************************************************
 *                                                                        *
 *  File:        Commons.cs                                               *
 *  Copyright:   (c) 2023, Pitica Sebastian                               *
 *  Description: Fișierul conține clasele cu caracter general utilizate   *
 *  în cadrul proiectului, separate pentru a putea fi folosit pe tot      *
 *  cuprinsul celorlalte module sau pentru că nu pot fi asociate cu alt   *
 *  modul concret.                                                        *
 *                                                                        *
 **************************************************************************/

namespace CommonsModule
{
    /// <summary>
    /// Clase ce reprezintă un wrap-up peste richTextBox. 
    /// Utilizată din necesitatea implementării unor funcționalități ce 
    /// aveau nevoie de adaugarea unor caracteristici noi peste clasa de bază.
    /// </summary>
    /// <creator>Sebastian</creator>
    
    public class RichTextBoxV2
    {
    
        public RichTextBox baseComponent { get; set; }
        private Boolean _isSaved;
        private string _progLangInTextbox;

        public RichTextBoxV2()
        { 
        }
    }


}

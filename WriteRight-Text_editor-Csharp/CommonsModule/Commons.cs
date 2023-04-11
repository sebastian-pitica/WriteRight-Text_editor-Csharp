using System.IO;
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
    /// <updated>Vasile</updated>
    public class RichTextBoxV2 : RichTextBox
    {
        private bool _isSaved;
        private string _filePath;

        public RichTextBoxV2() : base()
        {
            Dock = DockStyle.Fill;
            _isSaved = true;
            this.TextChanged += ControlTextChanged;
        }

        private void ControlTextChanged(object sender, System.EventArgs e)
        {
            _isSaved = false;
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
            }
        }

        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; }
        }

        public string FileName
        {
            get
            {
                return Path.GetFileName(_filePath);
            }
        }

        public string FileType
        {
            get
            {
                return Path.GetExtension(_filePath);
            }
        }
    }
}

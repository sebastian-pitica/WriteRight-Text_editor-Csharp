using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonsModule
{
    public class Zoom 
    {
        public static void ZoomIn( RichTextBox textBox)
        {

            FontStyle newFontStyle = textBox.Font.Style;
            textBox.Font = new Font(textBox.Font.FontFamily, textBox.Font.Size + 5, newFontStyle);
        }
        public static void ZoomOut( RichTextBox textBox)
        {
            FontStyle newFontStyle = textBox.Font.Style;
            if (textBox.Font.Size - 5 > 0)
            {
                textBox.Font = new Font(textBox.Font.FontFamily, textBox.Font.Size - 5, newFontStyle);
            }


        }

    }
}

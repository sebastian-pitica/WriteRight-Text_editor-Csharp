using System.IO;
using System.Windows.Forms;

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
            RichTextBoxV2 richTextBoxV2 = new RichTextBoxV2();
            tabPage.Controls.Add(richTextBoxV2);
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
            RichTextBoxV2 reference = (RichTextBoxV2)selectedTabPage.Controls[0];
            return reference;
        }

        public static string GetFileNameFromTabControl(in TabControl tabControl)
        {
            RichTextBoxV2 reference = GetRichTextBoxV2FTabControl(tabControl);
            return reference.FilePath ?? tabControl.SelectedTab.Text;
        }

    }
}

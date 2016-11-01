using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmlouvaWord
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Argumenty 0 - soubor XML s daty
            //           1 - název souboru šablony 
            //           2 - cesta pro uložení smluv
            //           3 - connection string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < args.Length; i++)
            {
                sb.AppendLine(args[i]);
            }

            MessageBox.Show(sb.ToString());
        }
    }
}

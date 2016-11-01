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
            var parameters = new Parameters(args);
            var valueProvider = new TestValueProvider();
            var saveFileNameProvider = new BasicSaveFileNameProvider(parameters);
            var wrapper = new WordWrapper(valueProvider, saveFileNameProvider, parameters);
            wrapper.Process();

            if (wrapper.MissingFields.Count != 0)
            {
                string fieldNames = string.Join(", ", wrapper.MissingFields);
                string msg = "Pro některá pole v dokumentu nebyla nalezena hodnota. Názvy polí: " + fieldNames;

            }
        }
    }
}

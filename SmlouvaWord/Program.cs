using System;
using WinkhausCR.Bugs;

namespace SmlouvaWord
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Instance.Log(ex);
            }
        }
    }
}

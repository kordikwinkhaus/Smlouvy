﻿using System;
using System.Windows.Forms;
using WinkhausCR.Bugs;

namespace SmlouvaWord
{
    static class Program
    {
        private const string TITLE = "Export smlouvy";

        [STAThread]
        static void Main()
        {
            try
            {
                var parameters = new Parameters(Environment.CommandLine);
                var xmlValueProvider = new XmlValueProvider(parameters);
                var sqlValueProvider = new SqlValueProvider(parameters, xmlValueProvider);
                var saveFileNameProvider = new BasicSaveFileNameProvider(parameters);
                var wrapper = new WordWrapper(sqlValueProvider, saveFileNameProvider, parameters);
                wrapper.Process();

                if (wrapper.MissingFields.Count != 0)
                {
                    string fieldNames = string.Join(", ", wrapper.MissingFields);
                    string msg = "Pro některá pole v dokumentu nebyla nalezena hodnota.\r\nNázvy polí: " + fieldNames + ".";
                    MessageBox.Show(msg, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ModelException mex)
            {
                MessageBox.Show(mex.Message, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastala chyba během vytváření smlouvy. Zkontrolujte prosím chybový log.", TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Instance.Log(ex);
            }
        }
    }
}

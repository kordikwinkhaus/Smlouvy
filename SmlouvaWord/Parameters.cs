using System;

namespace SmlouvaWord
{
    internal class Parameters
    {
        internal Parameters(string commandLine)
        {
            string[] args = ParseCommandLine(commandLine);

            this.XmlDataPath = args[0];
            this.TemplatePath = args[1];
            this.TargetDirPath = args[2];
            this.ConnectionString = args[3];

            string customTargetDirPath = Properties.Settings.Default.TargetDirPath;
            if (!string.IsNullOrWhiteSpace(customTargetDirPath))
            {
                this.TargetDirPath = customTargetDirPath;
            }
        }

        private static string[] ParseCommandLine(string commandLine)
        {
            string[] parts = commandLine.Split(new string[] { ".exe" }, StringSplitOptions.None);
            if (parts[1].StartsWith("\""))
            {
                parts[1] = parts[1].Substring(1);
            }
            parts[1] = parts[1].TrimStart();
            string[] args = parts[1].Split(new string[] { "\" \"" }, StringSplitOptions.None);

            if (args.Length < 4)
            {
                System.Windows.Forms.MessageBox.Show("Nedostatečný počet argumentů.");
                //throw new ArgumentException("Nedostatečný počet argumentů.");
            }

            var firstArg = args[0];
            if (firstArg.StartsWith("\""))
            {
                args[0] = firstArg.Substring(1);
            }
            var lastArg = args[3];
            if (lastArg.EndsWith("\""))
            {
                args[3] = lastArg.Substring(0, lastArg.Length - 1);
            }

            return args;
        }

        internal string XmlDataPath { get; private set; }

        internal string TemplatePath { get; private set; }

        internal string TargetDirPath { get; private set; }

        internal string ConnectionString { get; private set; }
    }
}

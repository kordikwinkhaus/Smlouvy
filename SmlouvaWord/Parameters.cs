using System;

namespace SmlouvaWord
{
    class Parameters
    {
        internal Parameters(string[] args)
        {
            if (args.Length > 4) throw new ArgumentException("Nedostatečný počet argumentů.");

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

        internal string XmlDataPath { get; private set; }

        internal string TemplatePath { get; private set; }

        internal string TargetDirPath { get; private set; }

        internal string ConnectionString { get; private set; }
    }
}

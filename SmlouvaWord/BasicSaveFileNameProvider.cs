using System;
using System.IO;

namespace SmlouvaWord
{
    internal class BasicSaveFileNameProvider : ISaveFileNameProvider
    {
        private readonly Parameters _parameters;

        internal BasicSaveFileNameProvider(Parameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            _parameters = parameters;
        }

        public string GetSaveFileName()
        {
            var xmlFileInfo = new FileInfo(_parameters.XmlDataPath);
            var templateFileInfo = new FileInfo(_parameters.TemplatePath);
            string baseFileName = xmlFileInfo.Name.Replace(xmlFileInfo.Extension, "") + templateFileInfo.Extension;
            baseFileName = baseFileName.Replace("/", "_")
                                       .Replace("\\", "_")
                                       .Replace(" ", "_")
                                       .Replace("&", "_");

            return Path.Combine(_parameters.TargetDirPath, baseFileName);
        }
    }
}

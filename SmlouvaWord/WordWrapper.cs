using System;
using System.Collections.Generic;
using System.IO;

namespace SmlouvaWord
{
    class WordWrapper
    {
        private const int wdPropertyTitle = 1;

        private readonly IValueProvider _provider;
        private readonly Parameters _parameters;
        private dynamic _word;
        private dynamic _doc;

        internal WordWrapper(IValueProvider provider, Parameters parameters)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            _provider = provider;
            _parameters = parameters;
        }

        internal List<string> MissingFields { get; private set; } = new List<string>();

        internal void Process()
        {
            try
            {
                RunWord();
                OpenTemplate();
                ProcessFields();
                SaveDocument();
            }
            finally
            {
                ShowWord();
            }
        }

        private void RunWord()
        {
            Type wdAppType = Type.GetTypeFromProgID("Word.Application");
            _word = Activator.CreateInstance(wdAppType);
#if (!DEBUG)
            _word.Visible = false;
#else
            _word.Visible = true;
#endif
        }

        private void OpenTemplate()
        {
            _doc = _word.Documents.Add(_parameters.TemplatePath);
            var xmlFileInfo = new FileInfo(_parameters.XmlDataPath);
            string baseFileName = xmlFileInfo.Name.Replace(xmlFileInfo.Extension, "");
            _doc.BuiltInDocumentProperties[wdPropertyTitle].Value = baseFileName;
        }

        private void ProcessFields()
        {
            dynamic fields = _doc.FormFields;
            List<string> missingFields = new List<string>();

            foreach (var field in fields)
            {
                string name = field.Name;
                string result;
                if (_provider.GetValue(name, out result))
                {
                    SetFieldValue(field, result);
                }
                else
                {
                    this.MissingFields.Add(name);
                }
            }

            fields.Update();
        }

        private void SetFieldValue(dynamic field, string value)
        {
            field.Range.InsertAfter(value);
            field.Delete();
        }

        private void SaveDocument()
        {
            var xmlFileInfo = new FileInfo(_parameters.XmlDataPath);
            var templateFileInfo = new FileInfo(_parameters.TemplatePath);
            string baseFileName = xmlFileInfo.Name.Replace(xmlFileInfo.Extension, "") + templateFileInfo.Extension;
            baseFileName = baseFileName.Replace("/", "_")
                                       .Replace("\\", "_")
                                       .Replace(" ", "_")
                                       .Replace("&", "_");

            string saveFileName = Path.Combine(_parameters.TargetDirPath, baseFileName);
            _doc.SaveAs(saveFileName);

            var saveFileInfo = new FileInfo(saveFileName);
            saveFileInfo.Attributes = FileAttributes.Archive;
        }

        private void ShowWord()
        {
            if (_word != null)
            {
                _word.Visible = true;
                _word.Activate();
            }
        }
    }
}

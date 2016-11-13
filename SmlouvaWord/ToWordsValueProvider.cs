using System.Globalization;
using WinkhausCR.NtwLib;

namespace SmlouvaWord
{
    internal class ToWordsValueProvider : IValueProvider
    {
        private readonly IValueProvider _inner;
        private readonly IToWordsConverter _toWordsConverter;

        internal ToWordsValueProvider(IValueProvider inner, IToWordsConverter toWordsConverter)
        {
            _inner = inner;
            _toWordsConverter = toWordsConverter;
        }

        public bool GetValue(string name, out string result)
        {
            if (name.EndsWith("SLOVY"))
            {
                string basename = name.Replace("SLOVY", "");
                string strnumber;
                if (_inner.GetValue(basename, out strnumber))
                {
                    decimal number = decimal.Parse(strnumber, CultureInfo.CurrentCulture);
                    result = _toWordsConverter.Convert(number);
                    return true;
                }
                result = null;
                return false;
            }
            else
            {
                return _inner.GetValue(name, out result);
            }
        }
    }
}

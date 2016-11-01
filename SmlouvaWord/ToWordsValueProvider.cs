using System.Globalization;

namespace SmlouvaWord
{
    internal class ToWordsValueProvider : IValueProvider
    {
        private readonly IValueProvider _inner;

        internal ToWordsValueProvider(IValueProvider inner)
        {
            _inner = inner;
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
                    result = number.ToWords();
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

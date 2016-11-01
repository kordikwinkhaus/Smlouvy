using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NumberToWordsLib
{
    public class NumberToWordsConverter
    {
        public NumberToWordsConverter(CultureInfo culture)
        {
        }

        public NumberToWordsConverter(CultureInfo culture, string currency)
        {
        }

        public string Convert(decimal number)
        {
            StringBuilder result = new StringBuilder();

            string numberString = number.ToString(CultureInfo.InvariantCulture);
            int decPoint = numberString.IndexOf('.');
            int[] mainDigits = numberString.Substring(0, decPoint)
                                           .Where(c => char.IsDigit(c))
                                           .Select(c => int.Parse(c.ToString()))
                                           .ToArray();
            int[] decimalDigits = numberString.Substring(decPoint + 1)
                                              .Where(c => char.IsDigit(c))
                                              .Select(c => int.Parse(c.ToString()))
                                              .ToArray();
                                            


            return result.ToString();
        }
    }
}

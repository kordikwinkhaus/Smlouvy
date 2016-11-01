using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NumberToWordsLib
{
    public class NumberToWordsConverter
    {
        static string[] s_smallunits = new string[] { "", "jeden", "dva", "tři", "čtyři", "pět", "šest", "sedm", "osm", "devět" };
        static string[] s_units = new string[] { "", "jedna", "dvě", "tři", "čtyři", "pět", "šest", "sedm", "osm", "devět" };
        static string[] s_teens = new string[] { "deset", "jedenáct", "dvanáct", "třináct", "čtrnáct", "patnáct", "šestnáct", "sedmnáct", "osmnáct", "devatenáct" };
        static string[] s_dozens = new string[] { "", "", "dvacet", "třicet", "čtyřicet", "padesát", "šedesát", "sedmdesát", "osmdesát", "devadesát" };
        static string[][] s_hundreds = new string[][] {
            new string[0],
            new string[] { "jedno", "sto" },
            new string[] { "dvě", "stě" },
            new string[] { "tři", "sta" },
            new string[] { "čtyři", "sta" },
            new string[] { "pět", "set" },
            new string[] { "šest", "set" },
            new string[] { "sedm", "set" },
            new string[] { "osm", "set" },
            new string[] { "devět", "set" }
        };
        static string[] s_thousand_ext = new string[] { "tisíc", "tisíc", "tisíce", "tisíce", "tisíce", "tisíc", "tisíc", "tisíc", "tisíc", "tisíc" };
        static string[] s_thousand_units = new string[] { "", "jeden", "dva", "tři", "čtyři", "pět", "šest", "sedm", "osm", "devět" };
        static string[] s_millions_ext = new string[] { "milionů", "milion", "miliony", "miliony", "miliony", "milionů", "milionů", "milionů", "milionů", "milionů" };
        static string[] s_millions_units = s_thousand_units;

        private string separator = " ";

        public NumberToWordsConverter(CultureInfo culture)
        {
        }

        public NumberToWordsConverter(CultureInfo culture, string currency)
        {
        }

        public string Convert(decimal number)
        {
            StringBuilder result = new StringBuilder();
            List<string> stack = new List<string>();

            string numberString = number.ToString(CultureInfo.InvariantCulture);
            int decPoint = numberString.IndexOf('.');
            if (decPoint == -1)
            {
                decPoint = numberString.Length;
            }

            int[] mainDigits = numberString.Substring(0, decPoint)
                                           .Where(c => char.IsDigit(c))
                                           .Select(c => int.Parse(c.ToString()))
                                           .Reverse()
                                           .ToArray();
            int[] decimalDigits = numberString.Substring(decPoint)
                                              .Where(c => char.IsDigit(c))
                                              .Select(c => int.Parse(c.ToString()))
                                              .ToArray();

            Extend(ConvertGroup(mainDigits, stack, 6, s_millions_units), stack, s_millions_ext);
            Extend(ConvertGroup(mainDigits, stack, 3, s_thousand_units), stack, s_thousand_ext);
            ConvertGroup(mainDigits, stack, 0, s_units);

            string currencyWords = " korun českých";
            // TODO: ověřit bez desetinné části
            if (mainDigits[0] == 1 && mainDigits.Length == 1)
            {
                currencyWords = " koruna česká";
            }
            else if (number < 5)
            {
                currencyWords = " koruny české";
            }
            result.Append(string.Join(separator, stack));
            result.Append(currencyWords);

            if (decimalDigits.Length != 0)
            {
                List<string> decimalStack = new List<string>();
                int[] decimalGroup = new int[3];
                for (int i = 0; i < 2 && i < decimalDigits.Length; i++)
                {
                    decimalGroup[i + 1] = decimalDigits[i];
                }
                if (ConvertGroup(decimalGroup.Reverse().ToArray(), decimalStack, 0, s_smallunits).Item1)
                {
                    result.Append(" ");
                    result.Append(string.Join(separator, decimalStack));
                }
                string smallCurrencyWords = " haléřů";
                int smallMoney = decimalGroup[1] * 10 + decimalGroup[2];
                if (smallMoney == 1)
                {
                    smallCurrencyWords = " haléř";
                }
                else if (smallMoney < 5)
                {
                    smallCurrencyWords = " haléře";
                }
                result.Append(smallCurrencyWords);
            }
            
            return result.ToString();
        }

        private Tuple<bool, int[]> ConvertGroup(int[] mainDigits, List<string> stack, int groupStartAt, string[] units)
        {
            int[] group = new int[3];
            for (int i = 0; i < group.Length; i++)
            {
                int groupIndex = groupStartAt + i;
                if (mainDigits.Length > groupIndex)
                {
                    group[i] = mainDigits[groupIndex];
                }
            }

            int originalCount = stack.Count;

            if (group[2] != 0)
            {
                stack.AddRange(s_hundreds[group[2]]);
            }
            if (group[1] == 1)
            {
                stack.Add(s_teens[group[0]]);
            }
            else
            {
                if (group[1] != 0)
                {
                    stack.Add(s_dozens[group[1]]);
                }
                if (group[0] != 0)
                {
                    stack.Add(units[group[0]]);
                }
            }

            return new Tuple<bool, int[]>(originalCount != stack.Count, group);
        }

        private void Extend(Tuple<bool, int[]> data, List<string> result, string[] ext)
        {
            if (data.Item1)
            {
                int[] group = data.Item2;
                if (group[1] != 0)
                {
                    result.Add(ext[0]);
                }
                else
                {
                    result.Add(ext[group[0]]);
                }
            }
        }
    }
}

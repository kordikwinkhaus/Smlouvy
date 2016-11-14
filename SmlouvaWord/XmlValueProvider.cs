using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace SmlouvaWord
{
    internal class XmlValueProvider : IValueProvider
    {
        private readonly XmlDocument _doc;
        private readonly Dictionary<string, Func<XmlDocument, string>> _tokens;

        internal XmlValueProvider(Parameters parameters)
        {
            _doc = new XmlDocument();
            _doc.Load(parameters.XmlDataPath);
            _tokens = new Dictionary<string, Func<XmlDocument, string>>
            {
                { "ID_DOKUMENTU", doc => GetXPathValue(doc, "/Umowa/@IndeksDokumentu") },
                { "CISLO_SMLOUVY", doc => GetXPathValue(doc, "/Umowa/@Numer") },
                { "CISLO_ZAKAZKY", doc => GetXPathValue(doc, "/Umowa/@NumerZlecenia") },
                { "OBJ_JMENO", doc => GetXPathValue(doc, "/Umowa/Zamawiający/@Nazwa") },
                { "OBJ_ADRESA", doc => GetXPathValue(doc, "/Umowa/Zamawiający/@Adres") },
                { "ICO", doc => GetXPathValue(doc, "/Umowa/Zamawiający/@NIP") },
                { "DIC", doc => GetXPathValue(doc, "/Umowa/Zamawiający/@Regon") },
                { "ADRESA_MONTAZE", doc => GetXPathValue(doc, "/Umowa/Zamawiający/AdresObiektu") },

                { "CENA_CELKEM", doc => FormatNumber(GetXPathValue(doc, "/Umowa/Faktura/Podsumowanie/PodsumowanieFaktury/WartoscBrutto"), "N2", 0) },
                { "CENA_VYROBKY_DOPLNKY", doc => GetCenaVyrobkyDoplnky(doc) },
                { "CENA_PRACE_SLUZBY", doc => GetCenaPraceSluzby(doc) },

                { "ZALOHA_PROCENTA", doc => FormatNumber(GetXPathValue(doc, "/Umowa/Zaliczka/@Procent"), "0.#") },
                { "ZALOHA_CASTKA", doc => FormatNumber(GetXPathValue(doc, "/Umowa/Zaliczka/@Kwota"), "N2", 0) },

                { "DPH_PROCENTA", doc => GetDphProcenta(doc) },
                { "DPH_CASTKA", doc => GetDphCastka(doc) },
            };
        }

        private string GetCenaVyrobkyDoplnky(XmlDocument doc)
        {
            string sOkna = GetXPathValue(doc, "/Umowa/Stolarka/@Netto");
            string sDoplnky = GetXPathValue(doc, "/Umowa/Dodatki/@Netto");

            decimal okna = decimal.Parse(sOkna, NumberStyles.Number, CultureInfo.InvariantCulture);
            decimal doplnky = decimal.Parse(sDoplnky, NumberStyles.Number, CultureInfo.InvariantCulture);

            decimal vyrobky = okna + doplnky;

            return vyrobky.ToString("N2");
        }

        private string GetCenaPraceSluzby(XmlDocument doc)
        {
            string[] nodeNames = new string[] { "Demontaż", "Montaż", "Wykończenie", "Transport", "Likwidacja" };
            decimal total = 0;
            foreach (var nodeName in nodeNames)
            {
                string sNodeVal = GetXPathValue(doc, "/Umowa/" + nodeName + "/@Netto");
                decimal nodeVal = decimal.Parse(sNodeVal, NumberStyles.Number, CultureInfo.InvariantCulture);
                total += nodeVal;
            }

            return total.ToString("N2");
        }

        private string GetDphCastka(XmlDocument doc)
        {
            decimal total = 0;

            foreach (XmlElement elem in doc.SelectNodes("/Umowa/Faktura/Podsumowanie/PodsumowanieStawki/KwotaVAT"))
            {
                string kwota = elem.InnerText;
                decimal castka = decimal.Parse(kwota, NumberStyles.Number, CultureInfo.InvariantCulture);
                total += castka;
            }

            return total.ToString("N2");
        }

        private string GetDphProcenta(XmlDocument doc)
        {
            List<string> sazby = new List<string>();

            foreach (XmlElement elem in doc.SelectNodes("/Umowa/Faktura/Podsumowanie/PodsumowanieStawki/StawkaVAT"))
            {
                sazby.Add(elem.InnerText);
            }

            return string.Join(", ", sazby);
        }

        private string GetXPathValue(XmlDocument doc, string xpath)
        {
            var node = _doc.SelectSingleNode(xpath);
            var elem = node as XmlElement;
            if (elem != null)
            {
                return elem.InnerText;
            }
            else
            {
                return node.Value;
            }
        }

        private string FormatNumber(string value, string format, int? decimals = null)
        {
            decimal number;
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out number))
            {
                if (decimals.HasValue)
                {
                    number = Math.Round(number, decimals.Value, MidpointRounding.AwayFromZero);
                }
                return number.ToString(format);
            }

            return value;
        }

        public bool GetValue(string name, out string result)
        {
            Func<XmlDocument, string> func;
            if (_tokens.TryGetValue(name, out func))
            {
                result = func(_doc);
                return true;
            }
            result = null;
            return false;
        }
    }
}

using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberToWordsLib;

namespace SmlouvaWord.Tests
{
    [TestClass]
    public class NumberToWordsTest
    {
        [TestMethod]
        public void ToWordsTest()
        {
            var converter = new NumberToWordsConverter(new CultureInfo("cs"), "CZK");

            Test(converter, 1m, "jedna koruna česká");
            Test(converter, 1.55m, "jedna koruna česká padesát pět haléřů");
            Test(converter, 2145.78m, "dva tisíce jedno sto čtyřicet pět korun českých sedmdesát osm haléřů");
            Test(converter, 12315917.12m, "dvanáct milionů tři sta patnáct tisíc devět set sedmnáct korun českých dvanáct haléřů");
        }

        private void Test(NumberToWordsConverter converter, decimal number, string expectedResult)
        {
            Assert.AreEqual(expectedResult, converter.Convert(number));
        }
    }
}

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
            Test(converter, 2m, "dvě koruny české");
            Test(converter, 5m, "pět korun českých");
            Test(converter, 10m, "deset korun českých");
            Test(converter, 11m, "jedenáct korun českých");
            Test(converter, 12m, "dvanáct korun českých");
            Test(converter, 20m, "dvacet korun českých");
            Test(converter, 21m, "dvacet jedna korun českých");
            Test(converter, 55m, "padesát pět korun českých");
            Test(converter, 99m, "devadesát devět korun českých");
            Test(converter, 100m, "jedno sto korun českých");
            Test(converter, 101m, "jedno sto jedna korun českých");
            Test(converter, 111m, "jedno sto jedenáct korun českých");
            Test(converter, 130m, "jedno sto třicet korun českých");
            Test(converter, 201m, "dvě stě jedna korun českých");
            Test(converter, 999m, "devět set devadesát devět korun českých");
            Test(converter, 1000m, "jeden tisíc korun českých");
            Test(converter, 2000m, "dva tisíce korun českých");
            Test(converter, 10000m, "deset tisíc korun českých");
            Test(converter, 15000m, "patnáct tisíc korun českých");
            Test(converter, 52000m, "padesát dva tisíc korun českých");
            Test(converter, 987654m, "devět set osmdesát sedm tisíc šest set padesát čtyři korun českých");
            Test(converter, 1000000m, "jeden milion korun českých");
            Test(converter, 4002001m, "čtyři miliony dva tisíce jedna korun českých");
            Test(converter, 987654321m, "devět set osmdesát sedm milionů šest set padesát čtyři tisíc tři sta dvacet jedna korun českých");
            Test(converter, 1.55m, "jedna koruna česká padesát pět haléřů");
            Test(converter, 2.01m, "dvě koruny české jeden haléř");
            Test(converter, 2.02m, "dvě koruny české dva haléře");
            Test(converter, 2.05m, "dvě koruny české pět haléřů");
            Test(converter, 2.10m, "dvě koruny české deset haléřů");
            Test(converter, 12315917.12m, "dvanáct milionů tři sta patnáct tisíc devět set sedmnáct korun českých dvanáct haléřů");
        }

        private void Test(NumberToWordsConverter converter, decimal number, string expectedResult)
        {
            Assert.AreEqual(expectedResult, converter.Convert(number));
        }
    }
}

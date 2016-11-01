using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmlouvaWord
{
    class TestValueProvider : IValueProvider
    {
        public TestValueProvider()
        {
        }

        public bool GetValue(string name, out string result)
        {
            result = name;
            return true;
        }
    }
}

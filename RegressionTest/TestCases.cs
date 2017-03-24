using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RegressionTest
{
    public class TestCases
    {
        [Fact]
        public void TestShoudFail()
        {
            string isTest = "Test";
            Assert.Equal(isTest, "Test1");
        }

        [Fact]
        public void TestShouldPass()
        {
            string isEmpty = string.Empty;

            Assert.Equal(isEmpty, String.Empty);
        }

    }
}

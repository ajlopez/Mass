namespace Mass.Core.Tests.Functions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PrintFunctionTests
    {
        [TestMethod]
        public void PrintInteger()
        {
            StringWriter writer = new StringWriter();
            PrintFunction function = new PrintFunction(writer);

            Assert.IsNull(function.Apply(new object[] { 123 }));

            Assert.AreEqual("123", writer.ToString());
        }

        [TestMethod]
        public void PrintIntegerWithNullSelf()
        {
            StringWriter writer = new StringWriter();
            PrintFunction function = new PrintFunction(writer);

            Assert.IsNull(function.Apply(null, new object[] { 123 }));

            Assert.AreEqual("123", writer.ToString());
        }

        [TestMethod]
        public void PrintTwoIntegers()
        {
            StringWriter writer = new StringWriter();
            PrintFunction function = new PrintFunction(writer);

            Assert.IsNull(function.Apply(new object[] { 123, 456 }));

            Assert.AreEqual("123456", writer.ToString());
        }
    }
}

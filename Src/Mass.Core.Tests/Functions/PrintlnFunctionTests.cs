namespace Mass.Core.Tests.Functions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Functions;

    [TestClass]
    public class PrintlnFunctionTests
    {
        [TestMethod]
        public void PrintlnInteger()
        {
            StringWriter writer = new StringWriter();
            PrintlnFunction function = new PrintlnFunction(writer);

            Assert.IsNull(function.Apply(new object[] { 123 }));

            Assert.AreEqual("123\r\n", writer.ToString());
        }

        [TestMethod]
        public void PrintlnTwoIntegers()
        {
            StringWriter writer = new StringWriter();
            PrintlnFunction function = new PrintlnFunction(writer);

            Assert.IsNull(function.Apply(new object[] { 123, 456 }));

            Assert.AreEqual("123\r\n456\r\n", writer.ToString());
        }
    }
}

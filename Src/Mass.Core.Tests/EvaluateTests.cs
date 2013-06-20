namespace Mass.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Compiler;
    using Mass.Core.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EvaluateTests
    {
        [TestMethod]
        public void AddNumbers()
        {
            Assert.AreEqual(1 + 2, Evaluate("1 + 2"));
            Assert.AreEqual(1.2 + 3.4, Evaluate("1.2 + 3.4"));
        }

        private static object Evaluate(string text)
        {
            Parser parser = new Parser(text);
            IExpression expression = parser.ParseExpression();
            Assert.IsNull(parser.ParseExpression());
            return expression.Evaluate(null);
        }
    }
}

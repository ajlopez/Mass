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

        [TestMethod]
        public void AddAndSubtractNumbers()
        {
            Assert.AreEqual(1 + 2 - 5, Evaluate("1 + 2 - 5"));
        }

        [TestMethod]
        public void AddAndDivideNumbers()
        {
            Assert.AreEqual(1 + 2 / 5, Evaluate("1 + 2 / 5"));
        }

        [TestMethod]
        public void AddNullAndNumber()
        {
            Assert.AreEqual(2, Evaluate("null + 2"));
        }

        [TestMethod]
        public void AddNumberAndNull()
        {
            Assert.AreEqual(2, Evaluate("2 + null"));
        }

        [TestMethod]
        public void ConcatenateStrings()
        {
            Assert.AreEqual("Hello world", Evaluate("\"Hello \" + \"world\""));
        }

        [TestMethod]
        public void ConcatenateStringAndNumber()
        {
            Assert.AreEqual("Hello 123", Evaluate("\"Hello \" + 123"));
        }

        [TestMethod]
        public void ConcatenateNumberAndString()
        {
            Assert.AreEqual("123456", Evaluate("123 + \"456\""));
        }

        [TestMethod]
        public void ConcatenateNullAndString()
        {
            Assert.AreEqual("Hello", Evaluate("null + \"Hello\""));
        }

        [TestMethod]
        public void ConcatenateStringAndNull()
        {
            Assert.AreEqual("Hello", Evaluate("\"Hello\" + null"));
        }

        [TestMethod]
        public void ConcatenateNullAndNull()
        {
            Assert.AreEqual(string.Empty, Evaluate("null + null"));
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

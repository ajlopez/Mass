namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Compiler;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FunctionExpressionTests
    {
        [TestMethod]
        public void EvaluateFunctionExpression()
        {
            Parser parser = new Parser("a+b");
            FunctionExpression expr = new FunctionExpression(new string[] { "a", "b" }, parser.ParseCommand());

            var result = expr.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IFunction));

            var function = (IFunction)result;

            Assert.AreEqual(3, function.Apply(null, new object[] { 1, 2 }));
        }

        [TestMethod]
        public void Equals()
        {
            ICommand body1 = (new Parser("a+b")).ParseCommand();
            ICommand body2 = (new Parser("a+c")).ParseCommand();
            IList<string> parameters1 = new string[] { "a", "b" };
            IList<string> parameters2 = new string[] { "a", "c" };
            IList<string> parameters3 = new string[] { "a" };

            IExpression expr1 = new FunctionExpression(parameters1, body1);
            IExpression expr2 = new FunctionExpression(parameters1, body2);
            IExpression expr3 = new FunctionExpression(parameters2, body1);
            IExpression expr4 = new FunctionExpression(parameters3, body1);
            IExpression expr5 = new FunctionExpression(parameters1, body1);

            Assert.IsTrue(expr1.Equals(expr5));
            Assert.IsTrue(expr5.Equals(expr1));
            Assert.AreEqual(expr1.GetHashCode(), expr5.GetHashCode());

            Assert.IsFalse(expr1.Equals(null));
            Assert.IsFalse(expr1.Equals(123));

            Assert.IsFalse(expr1.Equals(expr2));
            Assert.IsFalse(expr2.Equals(expr1));
            Assert.IsFalse(expr1.Equals(expr3));
            Assert.IsFalse(expr3.Equals(expr1));
            Assert.IsFalse(expr1.Equals(expr4));
            Assert.IsFalse(expr4.Equals(expr1));
        }
    }
}

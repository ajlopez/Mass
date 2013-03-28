namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ArrayExpressionTests
    {
        [TestMethod]
        public void EvaluateArrayExpression()
        {
            ArrayExpression expr = new ArrayExpression(new IExpression[] { new ConstantExpression(1), new ConstantExpression(2), new ConstantExpression(3) });

            var result = expr.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<object>));

            var list = (IList<object>)result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void Equals()
        {
            ArrayExpression expr1 = new ArrayExpression(new IExpression[] { new ConstantExpression(1) });
            ArrayExpression expr2 = new ArrayExpression(new IExpression[] { new ConstantExpression(2) });
            ArrayExpression expr3 = new ArrayExpression(new IExpression[] { new ConstantExpression(2), new ConstantExpression(3) });
            ArrayExpression expr4 = new ArrayExpression(new IExpression[] { new ConstantExpression(1) });

            Assert.IsTrue(expr1.Equals(expr4));
            Assert.IsTrue(expr4.Equals(expr1));
            Assert.AreEqual(expr1.GetHashCode(), expr4.GetHashCode());

            Assert.IsFalse(expr1.Equals(null));
            Assert.IsFalse(expr1.Equals(123));

            Assert.IsFalse(expr1.Equals(expr2));
            Assert.IsFalse(expr2.Equals(expr1));
            Assert.IsFalse(expr1.Equals(expr3));
            Assert.IsFalse(expr3.Equals(expr1));
        }
    }
}

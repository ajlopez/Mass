namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Expressions;

    [TestClass]
    public class BinaryArithmeticExpressionTests
    {
        [TestMethod]
        public void AddTwoIntegers()
        {
            BinaryArithmeticExpression expr = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Add);

            Assert.AreEqual(3, expr.Evaluate(null));
        }

        [TestMethod]
        public void Equals()
        {
            BinaryArithmeticExpression expr1 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Add);
            BinaryArithmeticExpression expr2 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(3), ArithmeticOperator.Add);
            BinaryArithmeticExpression expr3 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Add);
            BinaryArithmeticExpression expr4 = new BinaryArithmeticExpression(new ConstantExpression(2), new ConstantExpression(2), ArithmeticOperator.Add);

            Assert.IsTrue(expr1.Equals(expr3));
            Assert.IsTrue(expr3.Equals(expr1));
            Assert.AreEqual(expr1.GetHashCode(), expr3.GetHashCode());

            Assert.IsFalse(expr1.Equals(null));
            Assert.IsFalse(expr1.Equals("foo"));
            Assert.IsFalse(expr1.Equals(expr2));
            Assert.IsFalse(expr2.Equals(expr1));
            Assert.IsFalse(expr1.Equals(expr4));
            Assert.IsFalse(expr4.Equals(expr1));
        }
    }
}

namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void AddEquals()
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

        [TestMethod]
        public void SubtractTwoIntegers()
        {
            BinaryArithmeticExpression expr = new BinaryArithmeticExpression(new ConstantExpression(2), new ConstantExpression(1), ArithmeticOperator.Subtract);
            Assert.IsNotNull(expr.LeftExpression);
            Assert.IsNotNull(expr.RightExpression);
            Assert.AreEqual(1, expr.Evaluate(null));
        }

        [TestMethod]
        public void SubtractEquals()
        {
            BinaryArithmeticExpression expr1 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Subtract);
            BinaryArithmeticExpression expr2 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(3), ArithmeticOperator.Subtract);
            BinaryArithmeticExpression expr3 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Subtract);
            BinaryArithmeticExpression expr4 = new BinaryArithmeticExpression(new ConstantExpression(2), new ConstantExpression(2), ArithmeticOperator.Subtract);

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

        [TestMethod]
        public void MultiplyTwoIntegers()
        {
            BinaryArithmeticExpression expr = new BinaryArithmeticExpression(new ConstantExpression(3), new ConstantExpression(2), ArithmeticOperator.Multiply);

            Assert.AreEqual(6, expr.Evaluate(null));
        }

        [TestMethod]
        public void MultiplyEquals()
        {
            BinaryArithmeticExpression expr1 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Multiply);
            BinaryArithmeticExpression expr2 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(3), ArithmeticOperator.Multiply);
            BinaryArithmeticExpression expr3 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Multiply);
            BinaryArithmeticExpression expr4 = new BinaryArithmeticExpression(new ConstantExpression(2), new ConstantExpression(2), ArithmeticOperator.Multiply);

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

        [TestMethod]
        public void DivideTwoIntegers()
        {
            BinaryArithmeticExpression expr = new BinaryArithmeticExpression(new ConstantExpression(6), new ConstantExpression(2), ArithmeticOperator.Divide);

            Assert.AreEqual(3, expr.Evaluate(null));
        }

        [TestMethod]
        public void DivideTwoIntegersAsReal()
        {
            BinaryArithmeticExpression expr = new BinaryArithmeticExpression(new ConstantExpression(5), new ConstantExpression(2), ArithmeticOperator.Divide);

            Assert.AreEqual(2.5, expr.Evaluate(null));
        }

        [TestMethod]
        public void DivideEquals()
        {
            BinaryArithmeticExpression expr1 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Divide);
            BinaryArithmeticExpression expr2 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(3), ArithmeticOperator.Divide);
            BinaryArithmeticExpression expr3 = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Divide);
            BinaryArithmeticExpression expr4 = new BinaryArithmeticExpression(new ConstantExpression(2), new ConstantExpression(2), ArithmeticOperator.Divide);

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

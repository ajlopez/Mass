namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Mass.Core.Tests.Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QualifiedNameExpressionTests
    {
        [TestMethod]
        public void EvaluateUndefinedType()
        {
            QualifiedNameExpression expr = new QualifiedNameExpression("bar.foo");

            try
            {
                Assert.IsNull(expr.Evaluate(null));
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                Assert.AreEqual("unknown type 'bar.foo'", ex.Message);
            }
        }

        [TestMethod]
        public void EvaluateNativeType()
        {
            QualifiedNameExpression expr = new QualifiedNameExpression("Mass.Core.Tests.Classes.Person");

            Assert.AreSame(typeof(Person), expr.Evaluate(null));
        }

        [TestMethod]
        public void Equals()
        {
            QualifiedNameExpression expr1 = new QualifiedNameExpression("one");
            QualifiedNameExpression expr2 = new QualifiedNameExpression("two");
            QualifiedNameExpression expr3 = new QualifiedNameExpression("one");

            Assert.IsTrue(expr1.Equals(expr3));
            Assert.IsTrue(expr3.Equals(expr1));
            Assert.AreEqual(expr1.GetHashCode(), expr3.GetHashCode());

            Assert.IsFalse(expr1.Equals(null));
            Assert.IsFalse(expr1.Equals(123));
            Assert.IsFalse(expr1.Equals(expr2));
            Assert.IsFalse(expr2.Equals(expr1));
        }
    }
}

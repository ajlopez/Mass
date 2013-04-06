namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DynamicObjectExpressionTests
    {
        [TestMethod]
        public void EvaluateDynamicObjectExpression()
        {
            DynamicObjectExpression expr = new DynamicObjectExpression(new AssignCommand[] { new AssignCommand("one", new ConstantExpression(1)), new AssignCommand("two", new ConstantExpression(2)) });

            var result = expr.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var obj = (DynamicObject)result;

            Assert.AreEqual(1, obj.Get("one"));
            Assert.AreEqual(2, obj.Get("two"));
        }

        [TestMethod]
        public void Equals()
        {
            DynamicObjectExpression expr1 = new DynamicObjectExpression(new AssignCommand[] { new AssignCommand("a", new ConstantExpression(1)) });
            DynamicObjectExpression expr2 = new DynamicObjectExpression(new AssignCommand[] { new AssignCommand("b", new ConstantExpression(2)) });
            DynamicObjectExpression expr3 = new DynamicObjectExpression(new AssignCommand[] { new AssignCommand("a", new ConstantExpression(2)), new AssignCommand("b", new ConstantExpression(3)) });
            DynamicObjectExpression expr4 = new DynamicObjectExpression(new AssignCommand[] { new AssignCommand("a", new ConstantExpression(1)) });

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

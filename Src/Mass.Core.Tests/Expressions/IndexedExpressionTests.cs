namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Language;

    [TestClass]
    public class IndexedExpressionTests
    {
        [TestMethod]
        public void EvaluateArrayElement()
        {
            IndexedExpression expr = new IndexedExpression(new ConstantExpression(new int[] { 1, 2, 3 }), new IExpression[] { new ConstantExpression(1) });
            Context context = new Context();

            var result = expr.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void EvaluateListElement()
        {
            IndexedExpression expr = new IndexedExpression(new ConstantExpression(new List<int>() { 1, 2, 3 }), new IExpression[] { new ConstantExpression(2) });
            Context context = new Context();

            var result = expr.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void EvaluateDynamicObjectElement()
        {
            DynamicObject dobj = new DynamicObject();
            dobj.SetValue("one", 1);
            IndexedExpression expr = new IndexedExpression(new ConstantExpression(dobj), new IExpression[] { new ConstantExpression("one") });
            Context context = new Context();

            var result = expr.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Equals()
        {
            IndexedExpression expr1 = new IndexedExpression(new ConstantExpression("one"), new List<IExpression>() { new ConstantExpression(1) });
            IndexedExpression expr2 = new IndexedExpression(new ConstantExpression("two"), new List<IExpression>() { new ConstantExpression(1) });
            IndexedExpression expr3 = new IndexedExpression(new ConstantExpression("one"), new List<IExpression>() { new ConstantExpression(1), new ConstantExpression(2) });
            IndexedExpression expr4 = new IndexedExpression(new ConstantExpression("one"), new List<IExpression>() { new ConstantExpression(2) });
            IndexedExpression expr5 = new IndexedExpression(new ConstantExpression("one"), new List<IExpression>() { new ConstantExpression(1) });

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

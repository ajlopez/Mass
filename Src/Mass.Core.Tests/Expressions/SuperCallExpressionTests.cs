namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SuperCallExpressionTests
    {
        [TestMethod]
        public void CallSuperinitialize()
        {
            Machine machine = new Machine();
            var dobj = (DynamicObject)machine.ExecuteText("class Animal\ndefine initialize(name)\nself.name=name\nend\nend\nclass Dog extends Animal\nend\nnew Dog(\"Foo\")");
            SuperCallExpression expr = new SuperCallExpression(new IExpression[] { new ConstantExpression("Nero") });
            Context context = new Context(machine.RootContext);
            context.Set("self", dobj);

            expr.Evaluate(context);

            Assert.AreEqual("Nero", dobj.Get("name"));
        }

        [TestMethod]
        public void Equals()
        {
            SuperCallExpression expr1 = new SuperCallExpression(new IExpression[] { new ConstantExpression(1) });
            SuperCallExpression expr2 = new SuperCallExpression(new IExpression[] { new ConstantExpression(2) });
            SuperCallExpression expr3 = new SuperCallExpression(new IExpression[] { new ConstantExpression(2), new ConstantExpression(3) });
            SuperCallExpression expr4 = new SuperCallExpression(new IExpression[] { new ConstantExpression(1) });

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

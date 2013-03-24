namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Mass.Core.Compiler;
    using Mass.Core.Commands;

    [TestClass]
    public class CallDotExpressionTests
    {
        [TestMethod]
        public void CallObjectMethod()
        {
            DynamicObject obj = new DynamicObject();
            obj.SetValue("width", 100);
            obj.SetValue("height", 10);
            ICommand body = (new Parser("self.width * self.height")).ParseCommand();
            Context context = new Context();
            context.SetValue("obj", obj);

            obj.SetValue("area", new DefinedFunction(body, new string[] { "self" }, null));

            CallDotExpression expr = new CallDotExpression(new DotExpression(new NameExpression("obj"), "area"), new IExpression[] { });

            var result = expr.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual(1000, result);
        }

        [TestMethod]
        public void Equals()
        {
            CallDotExpression expr1 = new CallDotExpression(new DotExpression(new NameExpression("obj"), "println"), new IExpression[] { new ConstantExpression(1) });
            CallDotExpression expr2 = new CallDotExpression(new DotExpression(new NameExpression("obj"), "print"), new IExpression[] { new ConstantExpression(1) });
            CallDotExpression expr3 = new CallDotExpression(new DotExpression(new NameExpression("obj"), "println"), new IExpression[] { new ConstantExpression(2) });
            CallDotExpression expr4 = new CallDotExpression(new DotExpression(new NameExpression("obj"), "println"), new IExpression[] { new ConstantExpression(2), new ConstantExpression(3) });
            CallDotExpression expr5 = new CallDotExpression(new DotExpression(new NameExpression("obj"), "println"), new IExpression[] { new ConstantExpression(1) });

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

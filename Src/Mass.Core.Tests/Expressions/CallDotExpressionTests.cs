namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Compiler;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mass.Core.Tests.Classes;

    [TestClass]
    public class CallDotExpressionTests
    {
        [TestMethod]
        public void CallDynamicObjectMethod()
        {
            DynamicObject obj = new DynamicObject();
            obj.Set("width", 100);
            obj.Set("height", 10);
            ICommand body = (new Parser("self.width * self.height")).ParseCommand();
            Context context = new Context();
            context.Set("obj", obj);

            obj.Set("area", new DefinedFunction(body, new string[] { }, null));

            CallDotExpression expr = new CallDotExpression(new DotExpression(new NameExpression("obj"), "area"), new IExpression[] { });

            var result = expr.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual(1000, result);
        }

        [TestMethod]
        public void CallNativeObjectMethod()
        {
            Person person = new Person() { FirstName = "Adam", LastName = "TheFirst" };

            CallDotExpression expr = new CallDotExpression(new DotExpression(new ConstantExpression(person), "GetName"), new IExpression[] { });

            var result = expr.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.AreEqual("TheFirst, Adam", result);
        }

        [TestMethod]
        public void CallNativeListAddMethod()
        {
            IList<object> list = new List<object>();

            CallDotExpression expr = new CallDotExpression(new DotExpression(new ConstantExpression(list), "Add"), new IExpression[] { new ConstantExpression(1) });

            var result = expr.Evaluate(null);

            Assert.IsNull(result);

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(1, list[0]);
        }

        [TestMethod]
        public void CallNativeListAddMethodInLowerCase()
        {
            IList<object> list = new List<object>();

            CallDotExpression expr = new CallDotExpression(new DotExpression(new ConstantExpression(list), "add"), new IExpression[] { new ConstantExpression(1) });

            var result = expr.Evaluate(null);

            Assert.IsNull(result);

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(1, list[0]);
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

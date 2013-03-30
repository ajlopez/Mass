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
    public class CallExpressionTests
    {
        [TestMethod]
        public void CallPrintlnInteger()
        {
            StringWriter writer = new StringWriter();
            CallExpression expr = new CallExpression("puts", new IExpression[] { new ConstantExpression(123) });
            Machine machine = new Machine();
            PrintlnFunction puts = new PrintlnFunction(writer);
            machine.RootContext.Set("puts", puts);

            Assert.IsNull(expr.Evaluate(machine.RootContext));
            Assert.AreEqual("123\r\n", writer.ToString());
        }

        [TestMethod]
        public void Equals()
        {
            CallExpression expr1 = new CallExpression("println", new IExpression[] { new ConstantExpression(1) });
            CallExpression expr2 = new CallExpression("print", new IExpression[] { new ConstantExpression(1) });
            CallExpression expr3 = new CallExpression("println", new IExpression[] { new ConstantExpression(2) });
            CallExpression expr4 = new CallExpression("println", new IExpression[] { new ConstantExpression(2), new ConstantExpression(3) });
            CallExpression expr5 = new CallExpression("println", new IExpression[] { new ConstantExpression(1) });

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

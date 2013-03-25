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
    public class NewExpressionTests
    {
        [TestMethod]
        public void EvaluateNewExpression()
        {
            DefinedClass dclass = new DefinedClass("Dog");
            NewExpression expr = new NewExpression(new ConstantExpression(dclass), new IExpression[] { });

            var result = expr.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var obj = (DynamicObject)result;

            Assert.AreSame(dclass, obj.Class);
        }

        [TestMethod]
        public void EvaluateNewExpressionWithInitialize()
        {
            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            PrintlnFunction function = new PrintlnFunction(writer);
            machine.RootContext.SetValue("println", function);
            machine.ExecuteText("class Dog\ndefine initialize(a, b)\nprintln(a)\nprintln(b)\nend\nend\n");
            DefinedClass dclass = (DefinedClass)machine.RootContext.GetValue("Dog");
            NewExpression expr = new NewExpression(new ConstantExpression(dclass), new IExpression[] { new ConstantExpression(1), new ConstantExpression(2) });

            var result = expr.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var obj = (DynamicObject)result;

            Assert.AreSame(dclass, obj.Class);
            Assert.AreEqual("1\r\n2\r\n", writer.ToString());
        }

        [TestMethod]
        public void Equals()
        {
            NewExpression expr1 = new NewExpression(new NameExpression("Dog"), new IExpression[] { new ConstantExpression(1) });
            NewExpression expr2 = new NewExpression(new NameExpression("Animal"), new IExpression[] { new ConstantExpression(1) });
            NewExpression expr3 = new NewExpression(new NameExpression("Dog"), new IExpression[] { new ConstantExpression(2) });
            NewExpression expr4 = new NewExpression(new NameExpression("Dog"), new IExpression[] { new ConstantExpression(2), new ConstantExpression(3) });
            NewExpression expr5 = new NewExpression(new NameExpression("Dog"), new IExpression[] { new ConstantExpression(1) });

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

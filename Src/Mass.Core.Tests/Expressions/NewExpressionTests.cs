namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Expressions;
    using Mass.Core.Language;
    using System.IO;
    using Mass.Core.Functions;

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
    }
}

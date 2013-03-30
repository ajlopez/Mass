namespace Mass.Core.Tests.Functions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DefinedFunctionTests
    {
        [TestMethod]
        public void DefineAndExecuteSimplePrintln()
        {
            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            PrintlnFunction puts = new PrintlnFunction(writer);
            machine.RootContext.Set("puts", puts);

            DefinedFunction function = new DefinedFunction(new ExpressionCommand(new CallExpression("puts", new IExpression[] { new ConstantExpression(123) })), new string[] { }, machine.RootContext);

            Assert.IsNull(function.Apply(new object[] { }));
            Assert.AreEqual("123\r\n", writer.ToString());
        }

        [TestMethod]
        public void DefineAndExecuteFunctionWithParameters()
        {
            Context context = new Context();

            DefinedFunction function = new DefinedFunction(new ExpressionCommand(new BinaryArithmeticExpression(new NameExpression("a"), new NameExpression("b"), ArithmeticOperator.Add)), new string[] { "a", "b" }, context);

            var result = function.Apply(new object[] { 1, 2 });

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result);
        }
    }
}

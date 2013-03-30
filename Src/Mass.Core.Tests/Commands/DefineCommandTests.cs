namespace Mass.Core.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DefineCommandTests
    {
        [TestMethod]
        public void DefineSimpleFunction()
        {
            Context context = new Context();
            DefineCommand cmd = new DefineCommand("foo", new string[0], new ExpressionCommand(new CallExpression("puts", new IExpression[] { new ConstantExpression(123) })));

            var result = cmd.Execute(context);

            Assert.IsNull(result);

            var value = context.Find("foo");
            Assert.IsInstanceOfType(value, typeof(DefinedFunction));
        }

        [TestMethod]
        public void Equals()
        {
            DefineCommand cmd1 = new DefineCommand("foo", new string[0], new ExpressionCommand(new ConstantExpression(1)));
            DefineCommand cmd2 = new DefineCommand("bar", new string[0], new ExpressionCommand(new ConstantExpression(1)));
            DefineCommand cmd3 = new DefineCommand("foo", new string[0], new ExpressionCommand(new ConstantExpression(2)));
            DefineCommand cmd4 = new DefineCommand("foo", new string[0], new ExpressionCommand(new ConstantExpression(1)));

            Assert.IsTrue(cmd1.Equals(cmd4));
            Assert.IsTrue(cmd4.Equals(cmd1));
            Assert.AreEqual(cmd1.GetHashCode(), cmd4.GetHashCode());

            Assert.IsFalse(cmd1.Equals(null));
            Assert.IsFalse(cmd1.Equals(123));

            Assert.IsFalse(cmd1.Equals(cmd2));
            Assert.IsFalse(cmd2.Equals(cmd1));
            Assert.IsFalse(cmd1.Equals(cmd3));
            Assert.IsFalse(cmd3.Equals(cmd1));
        }

        [TestMethod]
        public void EqualsWithParameters()
        {
            DefineCommand cmd1 = new DefineCommand("foo", new string[] { "c" }, new ExpressionCommand(new ConstantExpression(1)));
            DefineCommand cmd2 = new DefineCommand("foo", new string[] { "a" }, new ExpressionCommand(new ConstantExpression(1)));
            DefineCommand cmd3 = new DefineCommand("foo", new string[] { "a", "b" }, new ExpressionCommand(new ConstantExpression(1)));
            DefineCommand cmd4 = new DefineCommand("foo", new string[] { "c" }, new ExpressionCommand(new ConstantExpression(1)));

            Assert.IsTrue(cmd1.Equals(cmd4));
            Assert.IsTrue(cmd4.Equals(cmd1));
            Assert.AreEqual(cmd1.GetHashCode(), cmd4.GetHashCode());

            Assert.IsFalse(cmd1.Equals(null));
            Assert.IsFalse(cmd1.Equals(123));

            Assert.IsFalse(cmd1.Equals(cmd2));
            Assert.IsFalse(cmd2.Equals(cmd1));
            Assert.IsFalse(cmd1.Equals(cmd3));
            Assert.IsFalse(cmd3.Equals(cmd1));
        }
    }
}

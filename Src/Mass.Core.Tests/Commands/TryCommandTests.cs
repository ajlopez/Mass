namespace Mass.Core.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Compiler;
    using Mass.Core.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TryCommandTests
    {
        [TestMethod]
        public void ExecuteSimpleTry()
        {
            Parser cmdparser = new Parser("a = a + 1");
            ICommand body = cmdparser.ParseCommand();

            Context context = new Context();
            context.Set("a", 1);

            TryCommand cmd = new TryCommand(body);

            Assert.IsNull(cmd.Execute(context));

            Assert.AreEqual(2, context.Get("a"));
        }

        [TestMethod]
        public void Equals()
        {
            TryCommand cmd1 = new TryCommand(new AssignCommand("one", new ConstantExpression(1)));
            TryCommand cmd2 = new TryCommand(new AssignCommand("one", new ConstantExpression(2)));
            TryCommand cmd3 = new TryCommand(new AssignCommand("one", new ConstantExpression(1)));

            Assert.IsTrue(cmd1.Equals(cmd3));
            Assert.IsTrue(cmd3.Equals(cmd1));
            Assert.AreEqual(cmd1.GetHashCode(), cmd3.GetHashCode());

            Assert.IsFalse(cmd1.Equals(null));
            Assert.IsFalse(cmd1.Equals(123));
            Assert.IsFalse(cmd1.Equals(cmd2));
            Assert.IsFalse(cmd2.Equals(cmd1));
        }
    }
}

namespace Mass.Core.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ReturnCommandTests
    {
        [TestMethod]
        public void ExecuteReturnCommand()
        {
            Context context = new Context();
            ReturnCommand cmd = new ReturnCommand(new ConstantExpression(1));

            Assert.AreEqual(1, cmd.Execute(context));
            Assert.IsTrue(context.HasReturnValue());
            Assert.AreEqual(1, context.GetReturnValue());
        }

        [TestMethod]
        public void Equals()
        {
            ReturnCommand cmd1 = new ReturnCommand(new ConstantExpression(1));
            ReturnCommand cmd2 = new ReturnCommand(new ConstantExpression(2));
            ReturnCommand cmd3 = new ReturnCommand(null);
            ReturnCommand cmd4 = new ReturnCommand(new ConstantExpression(1));

            Assert.IsTrue(cmd1.Equals(cmd4));
            Assert.IsTrue(cmd4.Equals(cmd1));
            Assert.AreEqual(cmd1.GetHashCode(), cmd4.GetHashCode());
            Assert.AreNotEqual(0, cmd3.GetHashCode());

            Assert.IsFalse(cmd1.Equals(null));
            Assert.IsFalse(cmd1.Equals(123));
            Assert.IsFalse(cmd1.Equals(cmd2));
            Assert.IsFalse(cmd2.Equals(cmd1));
            Assert.IsFalse(cmd1.Equals(cmd3));
            Assert.IsFalse(cmd3.Equals(cmd1));
        }
    }
}

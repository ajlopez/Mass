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
    public class IfCommandTests
    {
        [TestMethod]
        public void ExecuteSimpleIfWhenConditionIsTrue()
        {
            IfCommand cmd = new IfCommand(new ConstantExpression(true), new AssignCommand("one", new ConstantExpression(1)));
            Context context = new Context();
            Assert.AreEqual(1, cmd.Execute(context));
            Assert.AreEqual(1, context.Get("one"));
        }

        [TestMethod]
        public void ExecuteSimpleIfWhenConditionIsFalse()
        {
            IfCommand cmd = new IfCommand(new ConstantExpression(false), new AssignCommand("one", new ConstantExpression(1)));
            Context context = new Context();
            Assert.IsNull(cmd.Execute(context));
            Assert.IsNull(context.Get("one"));
        }

        [TestMethod]
        public void ExecuteSimpleIfWhenConditionIsNull()
        {
            IfCommand cmd = new IfCommand(new ConstantExpression(null), new AssignCommand("one", new ConstantExpression(1)));
            Context context = new Context();
            Assert.IsNull(cmd.Execute(context));
            Assert.IsNull(context.Get("one"));
        }

        [TestMethod]
        public void ExecuteSimpleIfWithElseWhenConditionIsNull()
        {
            IfCommand cmd = new IfCommand(new ConstantExpression(null), new AssignCommand("one", new ConstantExpression(1)), new AssignCommand("two", new ConstantExpression(2)));
            Context context = new Context();
            Assert.IsNotNull(cmd.Execute(context));
            Assert.IsNull(context.Get("one"));
            Assert.AreEqual(2, context.Get("two"));
        }

        [TestMethod]
        public void ExecuteSimpleIfWhenConditionIsZero()
        {
            IfCommand cmd = new IfCommand(new ConstantExpression(0), new AssignCommand("one", new ConstantExpression(1)));
            Context context = new Context();
            Assert.AreEqual(1, cmd.Execute(context));
            Assert.AreEqual(1, context.Get("one"));
        }

        [TestMethod]
        public void Equals()
        {
            IfCommand cmd1 = new IfCommand(new ConstantExpression(1), new AssignCommand("one", new ConstantExpression(1)));
            IfCommand cmd2 = new IfCommand(new ConstantExpression(2), new AssignCommand("one", new ConstantExpression(1)));
            IfCommand cmd3 = new IfCommand(new ConstantExpression(1), new AssignCommand("one", new ConstantExpression(2)));
            IfCommand cmd4 = new IfCommand(new ConstantExpression(1), new AssignCommand("one", new ConstantExpression(1)), new AssignCommand("two", new ConstantExpression(2)));
            IfCommand cmd5 = new IfCommand(new ConstantExpression(1), new AssignCommand("one", new ConstantExpression(1)));

            Assert.IsTrue(cmd1.Equals(cmd5));
            Assert.IsTrue(cmd5.Equals(cmd1));
            Assert.AreEqual(cmd1.GetHashCode(), cmd5.GetHashCode());
            Assert.AreNotEqual(cmd1.GetHashCode(), cmd4.GetHashCode());

            Assert.IsFalse(cmd1.Equals(null));
            Assert.IsFalse(cmd1.Equals(123));
            Assert.IsFalse(cmd1.Equals(cmd2));
            Assert.IsFalse(cmd2.Equals(cmd1));
            Assert.IsFalse(cmd1.Equals(cmd3));
            Assert.IsFalse(cmd3.Equals(cmd1));
            Assert.IsFalse(cmd1.Equals(cmd4));
            Assert.IsFalse(cmd4.Equals(cmd1));
        }
    }
}

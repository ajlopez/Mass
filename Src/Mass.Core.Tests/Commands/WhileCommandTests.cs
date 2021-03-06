﻿namespace Mass.Core.Tests.Commands
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
    public class WhileCommandTests
    {
        [TestMethod]
        public void ExecuteSimpleWhileWhenConditionIsTrue()
        {
            Parser cmdparser = new Parser("a = a + 1");
            ICommand body = cmdparser.ParseCommand();
            Parser exprparser = new Parser("a < 6");
            IExpression expr = exprparser.ParseExpression();

            Context context = new Context();
            context.Set("a", 1);

            WhileCommand cmd = new WhileCommand(expr, body);

            Assert.IsNull(cmd.Execute(context));

            Assert.AreEqual(6, context.Get("a"));
        }

        [TestMethod]
        public void Equals()
        {
            WhileCommand cmd1 = new WhileCommand(new ConstantExpression(1), new AssignCommand("one", new ConstantExpression(1)));
            WhileCommand cmd2 = new WhileCommand(new ConstantExpression(2), new AssignCommand("one", new ConstantExpression(1)));
            WhileCommand cmd3 = new WhileCommand(new ConstantExpression(1), new AssignCommand("one", new ConstantExpression(2)));
            WhileCommand cmd4 = new WhileCommand(new ConstantExpression(1), new AssignCommand("one", new ConstantExpression(1)));

            Assert.IsTrue(cmd1.Equals(cmd4));
            Assert.IsTrue(cmd4.Equals(cmd1));
            Assert.AreEqual(cmd1.GetHashCode(), cmd4.GetHashCode());

            Assert.IsFalse(cmd1.Equals(null));
            Assert.IsFalse(cmd1.Equals(123));
            Assert.IsFalse(cmd1.Equals(cmd2));
            Assert.IsFalse(cmd1.Equals(cmd3));
        }
    }
}

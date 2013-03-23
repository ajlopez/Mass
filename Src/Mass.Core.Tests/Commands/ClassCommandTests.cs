﻿namespace Mass.Core.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Commands;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Mass.Core.Language;

    [TestClass]
    public class ClassCommandTests
    {
        [TestMethod]
        public void DefineSimpleClass()
        {
            Context context = new Context();
            StringWriter writer = new StringWriter();
            context.SetValue("println", new PrintlnFunction(writer));
            ClassCommand cmd = new ClassCommand("Dog", new ExpressionCommand(new CallExpression("println", new IExpression[] { new ConstantExpression(123) })));

            var result = cmd.Execute(context);

            Assert.IsNull(result);

            var value = context.GetValue("Dog");
            Assert.IsInstanceOfType(value, typeof(DefinedClass));
            Assert.AreEqual(value, context.GetValue("Dog"));
            Assert.AreEqual("123\r\n", writer.ToString());
        }

        [TestMethod]
        public void RedefineSimpleClass()
        {
            Context context = new Context();
            StringWriter writer = new StringWriter();
            context.SetValue("println", new PrintlnFunction(writer));
            ClassCommand cmd = new ClassCommand("Dog", new ExpressionCommand(new CallExpression("println", new IExpression[] { new ConstantExpression(123) })));

            cmd.Execute(context);

            var initial = context.GetValue("Dog");

            var result = cmd.Execute(context);

            Assert.IsNull(result);

            var value = context.GetValue("Dog");
            Assert.IsInstanceOfType(value, typeof(DefinedClass));
            Assert.AreEqual(value, context.GetValue("Dog"));
            Assert.AreSame(initial, value);
            Assert.AreEqual("123\r\n123\r\n", writer.ToString());
        }

        [TestMethod]
        public void Equals()
        {
            ClassCommand cmd1 = new ClassCommand("foo", new ExpressionCommand(new ConstantExpression(1)));
            ClassCommand cmd2 = new ClassCommand("bar", new ExpressionCommand(new ConstantExpression(1)));
            ClassCommand cmd3 = new ClassCommand("foo", new ExpressionCommand(new ConstantExpression(2)));
            ClassCommand cmd4 = new ClassCommand("foo", new ExpressionCommand(new ConstantExpression(1)));

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

namespace Mass.Core.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Compiler;
    using Mass.Core.Expressions;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ClassCommandTests
    {
        [TestMethod]
        public void DefineSimpleClass()
        {
            Context context = new Context();
            StringWriter writer = new StringWriter();
            context.Set("println", new PrintlnFunction(writer));
            ClassCommand cmd = new ClassCommand("Dog", new ExpressionCommand(new CallExpression("println", new IExpression[] { new ConstantExpression(123) })));

            var result = cmd.Execute(context);

            Assert.IsNull(result);

            var value = context.Get("Dog");
            Assert.IsInstanceOfType(value, typeof(DefinedClass));
            Assert.AreEqual("123\r\n", writer.ToString());
        }

        [TestMethod]
        public void DefineSimpleClassWithSuperclass()
        {
            Context context = new Context();
            ClassCommand classcmd = new ClassCommand("Animal", new ExpressionCommand(new ConstantExpression(1)));
            ClassCommand cmd = new ClassCommand("Dog", "Animal", new ExpressionCommand(new ConstantExpression(1)));

            classcmd.Execute(context);
            var result = cmd.Execute(context);

            Assert.IsNull(result);

            var value = context.Get("Dog");
            Assert.IsInstanceOfType(value, typeof(DefinedClass));

            var dclass = (DefinedClass)value;
            Assert.AreEqual("Dog", dclass.Name);
            Assert.IsNotNull(dclass.Superclass);
            Assert.AreEqual("Animal", dclass.Superclass.Name);
        }

        [TestMethod]
        public void DefineSimpleClassWithMethod()
        {
            Context context = new Context();
            Parser parser = new Parser("define foo(a,b)\na+b\nend");
            ClassCommand cmd = new ClassCommand("Dog", parser.ParseCommand());

            var result = cmd.Execute(context);

            Assert.IsNull(result);

            var value = context.Get("Dog");
            Assert.IsInstanceOfType(value, typeof(DefinedClass));

            var dclass = (DefinedClass)value;

            Assert.IsNotNull(dclass.GetInstanceMethod("foo"));
        }

        [TestMethod]
        public void RedefineSimpleClass()
        {
            Context context = new Context();
            StringWriter writer = new StringWriter();
            context.Set("println", new PrintlnFunction(writer));
            ClassCommand cmd = new ClassCommand("Dog", new ExpressionCommand(new CallExpression("println", new IExpression[] { new ConstantExpression(123) })));

            cmd.Execute(context);

            var initial = context.Get("Dog");

            var result = cmd.Execute(context);

            Assert.IsNull(result);

            var value = context.Get("Dog");
            Assert.IsInstanceOfType(value, typeof(DefinedClass));
            Assert.AreEqual(value, context.Get("Dog"));
            Assert.AreSame(initial, value);
            Assert.AreEqual("123\r\n123\r\n", writer.ToString());
        }

        [TestMethod]
        public void Equals()
        {
            ClassCommand cmd1 = new ClassCommand("foo", new ExpressionCommand(new ConstantExpression(1)));
            ClassCommand cmd2 = new ClassCommand("bar", new ExpressionCommand(new ConstantExpression(1)));
            ClassCommand cmd3 = new ClassCommand("foo", new ExpressionCommand(new ConstantExpression(2)));
            ClassCommand cmd4 = new ClassCommand("foo", "bar", new ExpressionCommand(new ConstantExpression(1)));
            ClassCommand cmd5 = new ClassCommand("foo", new ExpressionCommand(new ConstantExpression(1)));

            Assert.IsTrue(cmd1.Equals(cmd5));
            Assert.IsTrue(cmd5.Equals(cmd1));
            Assert.AreEqual(cmd1.GetHashCode(), cmd5.GetHashCode());

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

namespace Mass.Core.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Expressions;
    using Mass.Core.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AssignDotCommandTests
    {
        [TestMethod]
        public void AssignValue()
        {
            AssignDotCommand cmd = new AssignDotCommand(new DotExpression(new NameExpression("obj"), "age"), new ConstantExpression(1));
            Context context = new Context();
            DynamicObject obj = new DynamicObject();
            context.SetValue("obj", obj);

            var result = cmd.Execute(context);

            Assert.AreEqual(1, result);
            Assert.AreEqual(1, obj.GetValue("age"));
        }

        [TestMethod]
        public void Equals()
        {
            AssignDotCommand cmd1 = new AssignDotCommand(new DotExpression(new NameExpression("a"), "b"), new ConstantExpression(1));
            AssignDotCommand cmd2 = new AssignDotCommand(new DotExpression(new NameExpression("a"), "b"), new ConstantExpression(2));
            AssignDotCommand cmd3 = new AssignDotCommand(new DotExpression(new NameExpression("a"), "c"), new ConstantExpression(1));
            AssignDotCommand cmd4 = new AssignDotCommand(new DotExpression(new NameExpression("a"), "b"), new ConstantExpression(1));

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

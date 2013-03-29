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
    public class AssignIndexedCommandTests
    {
        [TestMethod]
        public void AssignValueToDynamicObject()
        {
            AssignIndexedCommand cmd = new AssignIndexedCommand(new IndexedExpression(new NameExpression("obj"), new IExpression[] { new ConstantExpression("age") }), new ConstantExpression(800));
            Context context = new Context();
            DynamicObject obj = new DynamicObject();
            context.SetValue("obj", obj);

            var result = cmd.Execute(context);

            Assert.IsNull(result);
            Assert.AreEqual(800, obj.GetValue("age"));
        }

        [TestMethod]
        public void AssignValueToNativeArray()
        {
            AssignIndexedCommand cmd = new AssignIndexedCommand(new IndexedExpression(new NameExpression("array"), new IExpression[] { new ConstantExpression(0) }), new ConstantExpression(1));
            Context context = new Context();
            var array = new int[1];
            context.SetValue("array", array);

            var result = cmd.Execute(context);

            Assert.IsNull(result);
            Assert.AreEqual(1, array[0]);
        }

        [TestMethod]
        public void Equals()
        {
            AssignIndexedCommand cmd1 = new AssignIndexedCommand(new IndexedExpression(new NameExpression("obj"), new IExpression[] { new ConstantExpression("age") }), new ConstantExpression(1));
            AssignIndexedCommand cmd2 = new AssignIndexedCommand(new IndexedExpression(new NameExpression("obj"), new IExpression[] { new ConstantExpression("age") }), new ConstantExpression(2));
            AssignIndexedCommand cmd3 = new AssignIndexedCommand(new IndexedExpression(new NameExpression("obj"), new IExpression[] { new ConstantExpression("name") }), new ConstantExpression(1));
            AssignIndexedCommand cmd4 = new AssignIndexedCommand(new IndexedExpression(new NameExpression("obj"), new IExpression[] { new ConstantExpression("age") }), new ConstantExpression(1));

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

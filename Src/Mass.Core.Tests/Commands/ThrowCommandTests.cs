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
    public class ThrowCommandTests
    {
        [TestMethod]
        public void ExecuteSimpleThrow()
        {
            ThrowCommand cmd = new ThrowCommand(new ConstantExpression("Error"));

            try
            {
                cmd.Execute(null);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Error", ex.Message);
            }
        }

        [TestMethod]
        public void ExecuteThrowException()
        {
            ThrowCommand cmd = new ThrowCommand(new ConstantExpression(new InvalidOperationException("Error")));

            try
            {
                cmd.Execute(null);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                Assert.AreEqual("Error", ex.Message);
            }
        }

        [TestMethod]
        public void Equals()
        {
            ThrowCommand cmd1 = new ThrowCommand(new ConstantExpression(1));
            ThrowCommand cmd2 = new ThrowCommand(new ConstantExpression(2));
            ThrowCommand cmd3 = new ThrowCommand(new ConstantExpression(1));

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

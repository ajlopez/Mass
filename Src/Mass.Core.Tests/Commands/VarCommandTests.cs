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
    public class VarCommandTests
    {
        [TestMethod]
        public void DefineValue()
        {
            VarCommand cmd = new VarCommand("one");
            Context context = new Context();

            var result = cmd.Execute(context);

            Assert.IsNull(result);
            Assert.IsTrue(context.HasValue("one", true));
            Assert.IsNull(context.Get("one"));
        }

        [TestMethod]
        public void Equals()
        {
            VarCommand cmd1 = new VarCommand("a");
            VarCommand cmd2 = new VarCommand("b");
            VarCommand cmd3 = new VarCommand("a");

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

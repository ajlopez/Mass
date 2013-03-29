namespace Mass.Core.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BreakCommandTests
    {
        [TestMethod]
        public void ExecuteBreak()
        {
            Context context = new Context();
            ICommand cmd = new BreakCommand();

            var result = cmd.Execute(context);

            Assert.IsNull(result);
            Assert.IsTrue(context.HasBreak());
        }

        [TestMethod]
        public void Equals()
        {
            ICommand cmd1 = new BreakCommand();
            ICommand cmd2 = new BreakCommand();

            Assert.IsTrue(cmd1.Equals(cmd2));
            Assert.IsTrue(cmd2.Equals(cmd1));

            Assert.AreEqual(cmd1.GetHashCode(), cmd2.GetHashCode());

            Assert.IsFalse(cmd1.Equals(null));
            Assert.IsFalse(cmd1.Equals("foo"));
        }
    }
}

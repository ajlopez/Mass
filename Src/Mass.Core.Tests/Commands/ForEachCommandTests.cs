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
    public class ForEachCommandTests
    {
        [TestMethod]
        public void ExecuteSimpleForEach()
        {
            ForEachCommand cmd = new ForEachCommand("k", new ConstantExpression(new int[] { 1, 2, 3 }), new AssignCommand("a", new BinaryArithmeticExpression(new NameExpression("a"), new NameExpression("k"), ArithmeticOperator.Add)));
            Context context = new Context();
            Assert.IsNull(cmd.Execute(context));
            Assert.AreEqual(6, context.Get("a"));
        }

        [TestMethod]
        public void CreateForEachWithoutVariables()
        {
            ForEachCommand cmd = new ForEachCommand("k", new ConstantExpression(1), new AssignCommand("a", new BinaryArithmeticExpression(new NameExpression("a"), new NameExpression("k"), ArithmeticOperator.Add)));
            var varnames = cmd.VarNames;

            Assert.IsNotNull(varnames);
            Assert.AreEqual(0, varnames.Count);
        }

        [TestMethod]
        public void CreateForEachWithOnlyOneVar()
        {
            ForEachCommand cmd = new ForEachCommand("k", new ConstantExpression(1), new VarCommand("a"));
            var varnames = cmd.VarNames;

            Assert.IsNotNull(varnames);
            Assert.AreEqual(1, varnames.Count);
            Assert.AreEqual("a", varnames[0]);
        }

        [TestMethod]
        public void CreateForEachWithVar()
        {
            AssignCommand cmd1 = new AssignCommand("one", new ConstantExpression(1));
            VarCommand cmd2 = new VarCommand("one");
            CompositeCommand cmd = new CompositeCommand(new ICommand[] { cmd1, cmd2 });
            ForEachCommand forcmd = new ForEachCommand("k", new ConstantExpression(1), cmd);
            var varnames = forcmd.VarNames;

            Assert.IsNotNull(varnames);
            Assert.AreEqual(1, varnames.Count);
            Assert.AreEqual("one", varnames[0]);
        }

        [TestMethod]
        public void Equals()
        {
            ForEachCommand cmd1 = new ForEachCommand("k", new ConstantExpression(1), new AssignCommand("one", new ConstantExpression(1)));
            ForEachCommand cmd2 = new ForEachCommand("k", new ConstantExpression(2), new AssignCommand("one", new ConstantExpression(1)));
            ForEachCommand cmd3 = new ForEachCommand("q", new ConstantExpression(1), new AssignCommand("one", new ConstantExpression(2)));
            ForEachCommand cmd4 = new ForEachCommand("k", new ConstantExpression(1), new AssignCommand("one", new ConstantExpression(1)));

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

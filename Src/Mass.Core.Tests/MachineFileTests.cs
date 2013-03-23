namespace Mass.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Functions;
    using Mass.Core.Language;

    [TestClass]
    [DeploymentItem("MachineFiles", "MachineFiles")]
    public class MachineFileTests
    {
        private Machine machine;

        [TestInitialize]
        public void Setup()
        {
            this.machine = new Machine();
        }

        [TestMethod]
        public void ExecuteSimpleAssignFile()
        {
            Assert.AreEqual(1, this.machine.ExecuteFile("MachineFiles\\SimpleAssign.ms"));
            Assert.AreEqual(1, this.machine.RootContext.GetValue("a"));
        }

        [TestMethod]
        public void ExecuteSimpleAssignsFile()
        {
            Assert.AreEqual(2, this.machine.ExecuteFile("MachineFiles\\SimpleAssigns.ms"));
            Assert.AreEqual(1, this.machine.RootContext.GetValue("a"));
            Assert.AreEqual(2, this.machine.RootContext.GetValue("b"));
        }

        [TestMethod]
        public void ExecuteSimplePrintlnFile()
        {
            StringWriter writer = new StringWriter();
            this.machine.RootContext.SetValue("println", new PrintlnFunction(writer));
            Assert.AreEqual(null, this.machine.ExecuteFile("MachineFiles\\SimplePrintln.ms"));
            Assert.AreEqual("hello\r\n", writer.ToString());
        }

        [TestMethod]
        public void ExecuteSimpleDefineFile()
        {
            StringWriter writer = new StringWriter();
            this.machine.RootContext.SetValue("println", new PrintlnFunction(writer));
            Assert.AreEqual(null, this.machine.ExecuteFile("MachineFiles\\SimpleDefine.ms"));
            Assert.AreEqual("1\r\n", writer.ToString());
        }

        [TestMethod]
        public void ExecuteSimpleWhileFile()
        {
            StringWriter writer = new StringWriter();
            this.machine.RootContext.SetValue("println", new PrintlnFunction(writer));
            Assert.AreEqual(null, this.machine.ExecuteFile("MachineFiles\\SimpleWhile.ms"));
            Assert.AreEqual("6\r\n", writer.ToString());
        }
    }
}

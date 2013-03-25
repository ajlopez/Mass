namespace Mass.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        public void ExecuteRectangleFile()
        {
            this.machine.ExecuteFile("MachineFiles\\Rectangle.ms");

            var rect = this.machine.RootContext.GetValue("rect");

            Assert.IsNotNull(rect);
            Assert.IsInstanceOfType(rect, typeof(DynamicObject));

            var dobj = (DynamicObject)rect;

            Assert.IsNotNull(dobj.Class);
            Assert.AreEqual("Rectangle", dobj.Class.Name);
            Assert.AreEqual(4, dobj.GetValue("width"));
            Assert.AreEqual(6, dobj.GetValue("height"));
            Assert.AreEqual(6, this.machine.RootContext.GetValue("area"));
            Assert.AreEqual(24, this.machine.RootContext.GetValue("area2"));
        }
    }
}

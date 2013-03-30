﻿namespace Mass.Core.Tests
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
            Assert.AreEqual(1, this.machine.ExecuteFile("MachineFiles\\SimpleAssign.ms", this.machine.RootContext));
            Assert.AreEqual(1, this.machine.RootContext.GetValue("a"));
        }

        [TestMethod]
        public void ExecuteSimpleAssignsFile()
        {
            Assert.AreEqual(2, this.machine.ExecuteFile("MachineFiles\\SimpleAssigns.ms", this.machine.RootContext));
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
            this.machine.ExecuteFile("MachineFiles\\Rectangle.ms", this.machine.RootContext);

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

        [TestMethod]
        public void ExecuteForFile()
        {
            this.machine.ExecuteFile("MachineFiles\\For.ms", this.machine.RootContext);

            var result = this.machine.RootContext.GetValue("total");

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void ExecuteForIfFile()
        {
            this.machine.ExecuteFile("MachineFiles\\ForIf.ms", this.machine.RootContext);

            var result = this.machine.RootContext.GetValue("total");

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void ExecuteForEachFile()
        {
            this.machine.ExecuteFile("MachineFiles\\ForEach.ms", this.machine.RootContext);

            var result = this.machine.RootContext.GetValue("total");

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void ExecuteContinueFile()
        {
            this.machine.ExecuteFile("MachineFiles\\Continue.ms", this.machine.RootContext);

            var result = this.machine.RootContext.GetValue("total");

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);

            var result2 = this.machine.RootContext.GetValue("total2");

            Assert.IsNotNull(result2);
            Assert.AreEqual(4, result2);

            var result3 = this.machine.RootContext.GetValue("total3");

            Assert.IsNotNull(result3);
            Assert.AreEqual(4, result3);
        }

        [TestMethod]
        public void ExecuteBreakFile()
        {
            this.machine.ExecuteFile("MachineFiles\\Break.ms", this.machine.RootContext);

            var result = this.machine.RootContext.GetValue("total");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);

            var result2 = this.machine.RootContext.GetValue("total2");

            Assert.IsNotNull(result2);
            Assert.AreEqual(1, result2);

            var result3 = this.machine.RootContext.GetValue("total3");

            Assert.IsNotNull(result3);
            Assert.AreEqual(1, result3);
        }

        [TestMethod]
        public void ExecuteRequireFile()
        {
            Context context = new Context(this.machine.RootContext);
            context.SetValue("require", new RequireFunction(this.machine, "MachineFiles"));
            this.machine.ExecuteFile("MachineFiles\\Require.ms", context);

            var result = context.GetValue("module");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var dobj = (DynamicObject)result;

            Assert.AreEqual(1, dobj.GetValue("one"));
            Assert.AreEqual(2, dobj.GetValue("two"));
            Assert.AreEqual(3, dobj.GetValue("three"));

            Assert.IsNull(dobj.GetValue("add"));

            var foo = dobj.GetValue("foo");

            Assert.IsNotNull(foo);
            Assert.IsInstanceOfType(foo, typeof(IFunction));
        }

        [TestMethod]
        public void ExecuteRequireModule1File()
        {
            Context context = new Context(this.machine.RootContext);
            context.SetValue("require", new RequireFunction(this.machine, "MachineFiles"));
            this.machine.ExecuteFile("MachineFiles\\RequireModule1.ms", context);

            var result = context.GetValue("module");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var dobj = (DynamicObject)result;

            Assert.AreEqual(1, dobj.GetValue("one"));
            Assert.AreEqual(2, dobj.GetValue("two"));
            Assert.AreEqual(3, dobj.GetValue("three"));

            Assert.IsNull(dobj.GetValue("add"));

            var foo = dobj.GetValue("foo");

            Assert.IsNotNull(foo);
            Assert.IsInstanceOfType(foo, typeof(IFunction));
        }
    }
}

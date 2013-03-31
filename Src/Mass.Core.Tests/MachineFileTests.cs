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
            Assert.AreEqual(1, this.machine.ExecuteFile("MachineFiles\\SimpleAssign.ms", this.machine.RootContext));
            Assert.AreEqual(1, this.machine.RootContext.Find("a"));
        }

        [TestMethod]
        public void ExecuteSimpleAssignsFile()
        {
            Assert.AreEqual(2, this.machine.ExecuteFile("MachineFiles\\SimpleAssigns.ms", this.machine.RootContext));
            Assert.AreEqual(1, this.machine.RootContext.Find("a"));
            Assert.AreEqual(2, this.machine.RootContext.Find("b"));
        }

        [TestMethod]
        public void ExecuteSimplePrintlnFile()
        {
            StringWriter writer = new StringWriter();
            this.machine.RootContext.Set("println", new PrintlnFunction(writer));
            Assert.AreEqual(null, this.machine.ExecuteFile("MachineFiles\\SimplePrintln.ms"));
            Assert.AreEqual("hello\r\n", writer.ToString());
        }

        [TestMethod]
        public void ExecuteSimpleDefineFile()
        {
            StringWriter writer = new StringWriter();
            this.machine.RootContext.Set("println", new PrintlnFunction(writer));
            Assert.AreEqual(null, this.machine.ExecuteFile("MachineFiles\\SimpleDefine.ms"));
            Assert.AreEqual("1\r\n", writer.ToString());
        }

        [TestMethod]
        public void ExecuteSimpleWhileFile()
        {
            StringWriter writer = new StringWriter();
            this.machine.RootContext.Set("println", new PrintlnFunction(writer));
            Assert.AreEqual(null, this.machine.ExecuteFile("MachineFiles\\SimpleWhile.ms"));
            Assert.AreEqual("6\r\n", writer.ToString());
        }

        [TestMethod]
        public void ExecuteRectangleFile()
        {
            this.machine.ExecuteFile("MachineFiles\\Rectangle.ms", this.machine.RootContext);

            var rect = this.machine.RootContext.Find("rect");

            Assert.IsNotNull(rect);
            Assert.IsInstanceOfType(rect, typeof(DynamicObject));

            var dobj = (DynamicObject)rect;

            Assert.IsNotNull(dobj.Class);
            Assert.AreEqual("Rectangle", dobj.Class.Name);
            Assert.AreEqual(4, dobj.Get("width"));
            Assert.AreEqual(6, dobj.Get("height"));
            Assert.AreEqual(6, this.machine.RootContext.Find("area"));
            Assert.AreEqual(24, this.machine.RootContext.Find("area2"));
        }

        [TestMethod]
        public void ExecuteLocalFile()
        {
            var result = this.machine.ExecuteFile("MachineFiles\\Local.ms", this.machine.RootContext);

            var a = this.machine.RootContext.Find("a");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result);
            Assert.IsNotNull(a);
            Assert.AreEqual(1, a);
        }

        [TestMethod]
        public void ExecuteGlobalFile()
        {
            Context context = new Context(this.machine.RootContext);
            var result = this.machine.ExecuteFile("MachineFiles\\Global.ms", context);

            var a = this.machine.RootContext.Get("a");

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);
            Assert.IsNotNull(a);
            Assert.AreEqual(2, a);
            Assert.AreEqual(3, context.Get("a"));
        }

        [TestMethod]
        public void ExecuteForFile()
        {
            this.machine.ExecuteFile("MachineFiles\\For.ms", this.machine.RootContext);

            var result = this.machine.RootContext.Find("total");

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void ExecuteForIfFile()
        {
            this.machine.ExecuteFile("MachineFiles\\ForIf.ms", this.machine.RootContext);

            var result = this.machine.RootContext.Find("total");

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void ExecuteForEachFile()
        {
            this.machine.ExecuteFile("MachineFiles\\ForEach.ms", this.machine.RootContext);

            var result = this.machine.RootContext.Find("total");

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void ExecuteContinueFile()
        {
            this.machine.ExecuteFile("MachineFiles\\Continue.ms", this.machine.RootContext);

            var result = this.machine.RootContext.Find("total");

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);

            var result2 = this.machine.RootContext.Find("total2");

            Assert.IsNotNull(result2);
            Assert.AreEqual(4, result2);

            var result3 = this.machine.RootContext.Find("total3");

            Assert.IsNotNull(result3);
            Assert.AreEqual(4, result3);
        }

        [TestMethod]
        public void ExecuteBreakFile()
        {
            this.machine.ExecuteFile("MachineFiles\\Break.ms", this.machine.RootContext);

            var result = this.machine.RootContext.Find("total");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);

            var result2 = this.machine.RootContext.Find("total2");

            Assert.IsNotNull(result2);
            Assert.AreEqual(1, result2);

            var result3 = this.machine.RootContext.Find("total3");

            Assert.IsNotNull(result3);
            Assert.AreEqual(1, result3);
        }

        [TestMethod]
        public void ExecuteRequireFile()
        {
            Context context = new Context(this.machine.RootContext);
            context.Set("require", new RequireFunction(this.machine, "MachineFiles"));
            this.machine.ExecuteFile("MachineFiles\\Require.ms", context);

            var result = context.Find("module");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var dobj = (DynamicObject)result;

            Assert.AreEqual(1, dobj.Get("one"));
            Assert.AreEqual(2, dobj.Get("two"));
            Assert.AreEqual(3, dobj.Get("three"));

            Assert.IsNull(dobj.Get("add"));

            var foo = dobj.Get("foo");

            Assert.IsNotNull(foo);
            Assert.IsInstanceOfType(foo, typeof(IFunction));
        }

        [TestMethod]
        public void ExecuteRequireModule1File()
        {
            Context context = new Context(this.machine.RootContext);
            context.Set("require", new RequireFunction(this.machine, "MachineFiles"));
            this.machine.ExecuteFile("MachineFiles\\RequireModule1.ms", context);

            AssertModule(context.Find("module"));
        }

        [TestMethod]
        public void ExecuteRequireModule2File()
        {
            Context context = new Context(this.machine.RootContext);
            context.Set("require", new RequireFunction(this.machine, "MachineFiles"));
            this.machine.ExecuteFile("MachineFiles\\RequireModule2.ms", context);

            AssertModule(context.Find("module"));
        }

        [TestMethod]
        public void ExecuteRequireModule3File()
        {
            Context context = new Context(this.machine.RootContext);
            context.Set("require", new RequireFunction(this.machine, "MachineFiles"));
            this.machine.ExecuteFile("MachineFiles\\RequireModule3.ms", context);

            AssertModule(context.Find("module"));
        }

        [TestMethod]
        public void ExecuteRequireModule4File()
        {
            Context context = new Context(this.machine.RootContext);
            context.Set("require", new RequireFunction(this.machine, "MachineFiles"));
            this.machine.ExecuteFile("MachineFiles\\RequireModule4.ms", context);

            AssertModule(context.Find("module"));
        }

        [TestMethod]
        public void ExecuteRequireModule5File()
        {
            Context context = new Context(this.machine.RootContext);
            context.Set("require", new RequireFunction(this.machine, "MachineFiles"));
            this.machine.ExecuteFile("MachineFiles\\RequireModule5.ms", context);

            AssertModule(context.Find("module"));
        }

        [TestMethod]
        public void ExecuteRequireModule6File()
        {
            Context context = new Context(this.machine.RootContext);
            context.Set("require", new RequireFunction(this.machine, "MachineFiles"));
            this.machine.ExecuteFile("MachineFiles\\RequireModule6.ms", context);

            AssertModule(context.Find("module"));
        }

        [TestMethod]
        public void ExecuteRequireModuleFileUsingCache()
        {
            Context context = new Context(this.machine.RootContext);
            context.Set("require", new RequireFunction(this.machine, "MachineFiles"));
            var original = this.machine.ExecuteFile("MachineFiles\\RequireModule1.ms", context);
            var result = this.machine.ExecuteFile("MachineFiles\\RequireModule1.ms", context, true);

            Assert.IsNotNull(result);
            Assert.AreSame(original, result);
        }

        private static void AssertModule(object result)
        {
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var dobj = (DynamicObject)result;

            Assert.AreEqual(1, dobj.Get("one"));
            Assert.AreEqual(2, dobj.Get("two"));
            Assert.AreEqual(3, dobj.Get("three"));

            Assert.IsNull(dobj.Get("add"));

            var foo = dobj.Get("foo");

            Assert.IsNotNull(foo);
            Assert.IsInstanceOfType(foo, typeof(IFunction));
        }
    }
}

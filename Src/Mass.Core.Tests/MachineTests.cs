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
    public class MachineTests
    {
        [TestMethod]
        public void HasRootContext()
        {
            Machine machine = new Machine();
            Assert.IsNotNull(machine.RootContext);
        }

        [TestMethod]
        public void PredefinedFunctions()
        {
            Machine machine = new Machine();

            var result = machine.RootContext.GetValue("println");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IFunction));

            result = machine.RootContext.GetValue("print");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IFunction));
        }

        [TestMethod]
        public void ExecuteText()
        {
            Machine machine = new Machine();
            Assert.AreEqual(2, machine.ExecuteText("a=1\nb=2"));
            Assert.AreEqual(1, machine.RootContext.GetValue("a"));
            Assert.AreEqual(2, machine.RootContext.GetValue("b"));
        }

        [TestMethod]
        public void DefineEmptyClass()
        {
            Machine machine = new Machine();
            machine.ExecuteText("class Dog\nend");

            var result = machine.RootContext.GetValue("Dog");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedClass));

            var dclass = (DefinedClass)result;

            Assert.AreEqual("Dog", dclass.Name);
        }

        [TestMethod]
        public void DefineClassWithEmptyMethod()
        {
            Machine machine = new Machine();
            machine.ExecuteText("class Dog\ndefine foo()\nend\nend");

            var result = machine.RootContext.GetValue("Dog");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedClass));

            var dclass = (DefinedClass)result;

            Assert.AreEqual("Dog", dclass.Name);
            Assert.IsNotNull(dclass.GetInstanceMethod("foo"));
        }

        [TestMethod]
        public void ExecuteReturnInCompositeCommand()
        {
            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            machine.RootContext.SetValue("println", new PrintlnFunction(writer));
            var result = machine.ExecuteText("define foo(a)\nif a\nreturn a\nend\nprintln(a)\nend\nfoo(1)");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
            Assert.IsTrue(string.IsNullOrEmpty(writer.ToString()));
        }

        [TestMethod]
        public void ExecuteSimpleForEach()
        {
            Machine machine = new Machine();
            var result = machine.ExecuteText("a=0\nfor k in [1,2,3]\n a = k + a\nend\na");

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void ExecuteSimpleFor()
        {
            Machine machine = new Machine();
            var result = machine.ExecuteText("a=0\nfor k = 1 to 3\n a = k + a\nend\na");

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void ExecuteSimpleForWithStep()
        {
            Machine machine = new Machine();
            var result = machine.ExecuteText("a=0\nfor k = 1 to 3 step 2\n a = k + a\nend\na");

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void EvaluateDynamicObject()
        {
            Machine machine = new Machine();
            var result = machine.ExecuteText("{a=1, b=2, c=3}");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var obj = (DynamicObject)result;

            Assert.AreEqual(1, obj.GetValue("a"));
            Assert.AreEqual(2, obj.GetValue("b"));
            Assert.AreEqual(3, obj.GetValue("c"));
        }

        [TestMethod]
        public void EvaluateIndexedExpression()
        {
            Machine machine = new Machine();
            var result = machine.ExecuteText("a = [1,2,3]\na[2]");

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void EvaluateIndexedDynamicObject()
        {
            Machine machine = new Machine();
            var result = machine.ExecuteText("a = { name = \"Adam\" }\na[\"name\"]");

            Assert.IsNotNull(result);
            Assert.AreEqual("Adam", result);
        }
    }
}

﻿namespace Mass.Core.Tests
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
    }
}

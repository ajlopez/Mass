namespace Mass.Core.Tests.Functions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Functions;
    using Mass.Core.Language;

    [TestClass]
    public class RequireFunctionTests
    {
        [TestMethod]
        [DeploymentItem("MachineFiles\\SimpleModule.ms")]
        public void RequireLocalFile()
        {
            Machine machine = new Machine();
            RequireFunction require = new RequireFunction(machine);

            AssertModule(require.Apply(new object[] { "SimpleModule.ms" }));
        }

        [TestMethod]
        [DeploymentItem("MachineFiles\\SimpleModule.ms")]
        public void RequireLocalFileTwice()
        {
            Machine machine = new Machine();
            RequireFunction require = new RequireFunction(machine);

            var original = require.Apply(new object[] { "SimpleModule" });
            var result = require.Apply(new object[] { "SimpleModule" });

            AssertModule(result);
            Assert.AreSame(original, result);
        }

        [TestMethod]
        [DeploymentItem("MachineFiles\\SimpleModule.ms")]
        public void RequireLocalFileWithExplicitExtension()
        {
            Machine machine = new Machine();
            RequireFunction require = new RequireFunction(machine);

            AssertModule(require.Apply(new object[] { "SimpleModule.ms" }));
        }

        [TestMethod]
        [DeploymentItem("MachineFiles\\SimpleModule.ms")]
        public void RequireLocalFileWithLocalDirectory()
        {
            Machine machine = new Machine();
            RequireFunction require = new RequireFunction(machine);

            AssertModule(require.Apply(new object[] { "./SimpleModule" }));
        }

        [TestMethod]
        public void RequireUnknownModule()
        {
            Machine machine = new Machine();
            RequireFunction require = new RequireFunction(machine);

            try
            {
                require.Apply(new object[] { "unknown" });
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                Assert.AreEqual("cannot find module 'unknown'", ex.Message);
            }
        }

        [TestMethod]
        public void RequireUnknownLocalModule()
        {
            Machine machine = new Machine();
            RequireFunction require = new RequireFunction(machine);

            try
            {
                require.Apply(new object[] { "./unknown" });
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                Assert.AreEqual("cannot find module './unknown'", ex.Message);
            }
        }

        [TestMethod]
        public void RequireUnknownAbsoluteModule()
        {
            Machine machine = new Machine();
            RequireFunction require = new RequireFunction(machine);

            try
            {
                require.Apply(new object[] { "/unknown" });
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                Assert.AreEqual("cannot find module '/unknown'", ex.Message);
            }
        }

        private static void AssertModule(object result)
        {
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

namespace Mass.Core.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Commands;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Mass.Core.Expressions;

    [TestClass]
    public class DefinedClassTests
    {
        [TestMethod]
        public void CreateDefinedClass()
        {
            DefinedClass dclass = new DefinedClass("Dog");

            Assert.AreEqual("Dog", dclass.Name);
        }

        [TestMethod]
        public void UndefinedInstanceMethodIsNull()
        {
            DefinedClass dclass = new DefinedClass("Dog");

            Assert.IsNull(dclass.GetInstanceMethod("foo"));
        }

        [TestMethod]
        public void CreateInstance()
        {
            DefinedClass dclass = new DefinedClass("Dog");
            IFunction foo = new DefinedFunction(null, null, null);
            dclass.SetInstanceMethod("foo", foo);

            var result = dclass.CreateInstance();

            Assert.IsNotNull(result);
            Assert.AreSame(dclass, result.Class);
            Assert.AreSame(foo, result.GetValue("foo"));
        }
    }
}

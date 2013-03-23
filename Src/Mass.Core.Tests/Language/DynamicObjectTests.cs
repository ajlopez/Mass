namespace Mass.Core.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Language;
    using Mass.Core.Functions;

    [TestClass]
    public class DynamicObjectTests
    {
        [TestMethod]
        public void GetUndefinedValue()
        {
            DynamicObject obj = new DynamicObject();

            Assert.IsNull(obj.GetValue("name"));
        }

        [TestMethod]
        public void SetAndGetValue()
        {
            DynamicObject obj = new DynamicObject();

            obj.SetValue("name", "Nero");

            Assert.AreEqual("Nero", obj.GetValue("name"));
        }
    }
}

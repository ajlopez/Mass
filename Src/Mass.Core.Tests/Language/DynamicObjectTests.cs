namespace Mass.Core.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DynamicObjectTests
    {
        [TestMethod]
        public void GetUndefinedValue()
        {
            DynamicObject obj = new DynamicObject();

            Assert.IsNull(obj.Get("name"));
        }

        [TestMethod]
        public void SetAndGetValue()
        {
            DynamicObject obj = new DynamicObject();

            obj.Set("name", "Nero");

            Assert.AreEqual("Nero", obj.Get("name"));
        }
    }
}

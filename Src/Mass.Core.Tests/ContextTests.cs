namespace Mass.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ContextTests
    {
        [TestMethod]
        public void GetUndefinedValueAsNull()
        {
            Context context = new Context();

            Assert.IsNull(context.Get("foo"));
        }

        [TestMethod]
        public void HasContext()
        {
            Context context = new Context();

            Assert.AreSame(context, context.Get("context"));
        }

        [TestMethod]
        public void RootContextHasNullOuter()
        {
            Context context = new Context();

            Assert.IsNull(context.Get("outer"));
        }

        [TestMethod]
        public void SetAndGetValue()
        {
            Context context = new Context();

            context.Set("one", 1);
            Assert.AreEqual(1, context.Get("one"));
        }

        [TestMethod]
        public void ParentIsOuter()
        {
            Context parent = new Context();
            Context context = new Context(parent);
            Assert.AreSame(parent, context.Get("outer"));
        }

        [TestMethod]
        public void FindValueAtParent()
        {
            Context parent = new Context();
            Context context = new Context(parent);

            parent.Set("one", 1);
            Assert.AreEqual(1, context.Get("one"));
        }

        [TestMethod]
        public void GetValueAtParent()
        {
            Context parent = new Context();
            Context context = new Context(parent);

            parent.Set("one", 1);
            Assert.AreEqual(1, context.Get("one"));
        }

        [TestMethod]
        public void GetValueAtParentIfVisible()
        {
            Context parent = new Context();
            Context context = new Context(parent, null);

            parent.Set("one", 1);
            Assert.AreEqual(1, context.Get("one"));
        }

        [TestMethod]
        public void GetNames()
        {
            Context parent = new Context();
            Context context = new Context(parent);

            parent.Set("one", 1);
            context.Set("two", 2);
            context.Set("three", 3);

            var result = context.GetNames();

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.IsTrue(result.Contains("two"));
            Assert.IsTrue(result.Contains("three"));
            Assert.IsTrue(result.Contains("context"));
            Assert.IsTrue(result.Contains("outer"));
        }

        [TestMethod]
        public void Break()
        {
            Context context = new Context();

            Assert.IsFalse(context.HasBreak());
            context.SetBreak();
            Assert.IsTrue(context.HasBreak());
            context.ClearBreak();
            Assert.IsFalse(context.HasBreak());
        }

        [TestMethod]
        public void Continue()
        {
            Context context = new Context();

            Assert.IsFalse(context.HasContinue());
            context.SetContinue();
            Assert.IsTrue(context.HasContinue());
            context.ClearContinue();
            Assert.IsFalse(context.HasContinue());
        }

        [TestMethod]
        public void HasReturn()
        {
            Context context = new Context();

            Assert.IsFalse(context.HasReturnValue());
        }

        [TestMethod]
        public void SetAndGetReturn()
        {
            Context context = new Context();

            context.SetReturnValue(1);
            Assert.IsTrue(context.HasReturnValue());
            Assert.AreEqual(1, context.GetReturnValue());
        }
    }
}

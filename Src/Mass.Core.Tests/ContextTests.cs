﻿namespace Mass.Core.Tests
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
        public void SetAndGetValue()
        {
            Context context = new Context();

            context.Set("one", 1);
            Assert.AreEqual(1, context.Get("one"));
        }

        [TestMethod]
        public void GetGlobal()
        {
            Context context = new Context();

            var result = context.Get("global");

            Assert.IsNotNull(result);
            Assert.AreSame(context, result);
        }

        [TestMethod]
        public void GetParentAsGlobal()
        {
            Context parent = new Context();
            Context context = new Context(parent);

            var result = context.Get("global");

            Assert.IsNotNull(result);
            Assert.AreSame(parent, result);
        }

        [TestMethod]
        public void FindValueAtParent()
        {
            Context parent = new Context();
            Context context = new Context(parent);

            parent.Set("one", 1);
            Assert.AreEqual(1, context.Find("one"));
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
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains("two"));
            Assert.IsTrue(result.Contains("three"));
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

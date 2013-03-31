﻿namespace Mass.Core.Tests.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Mass.Core.Tests.Classes;
    using Mass.Core.Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ObjectUtilitiesTests
    {
        [TestMethod]
        public void GetPropertyFromString()
        {
            Assert.AreEqual(3, ObjectUtilities.GetValue("foo", "Length"));
        }

        [TestMethod]
        public void GetValueUsingCall()
        {
            Assert.AreEqual("oo", ObjectUtilities.GetValue("foo", "Substring", new object[] { 1 }));
        }

        [TestMethod]
        public void IsNumber()
        {
            Assert.IsTrue(ObjectUtilities.IsNumber((byte)1));
            Assert.IsTrue(ObjectUtilities.IsNumber((short)2));
            Assert.IsTrue(ObjectUtilities.IsNumber((int)3));
            Assert.IsTrue(ObjectUtilities.IsNumber((long)4));
            Assert.IsTrue(ObjectUtilities.IsNumber((float)1.2));
            Assert.IsTrue(ObjectUtilities.IsNumber((double)2.3));

            Assert.IsFalse(ObjectUtilities.IsNumber(null));
            Assert.IsFalse(ObjectUtilities.IsNumber("foo"));
            Assert.IsFalse(ObjectUtilities.IsNumber('a'));
            Assert.IsFalse(ObjectUtilities.IsNumber(this));
        }

        [TestMethod]
        public void GetIndexedValuesFromArray()
        {
            Assert.AreEqual(2, ObjectUtilities.GetIndexedValue(new int[] { 1, 2, 3 }, 1));
        }

        [TestMethod]
        public void GetIndexedValuesFromList()
        {
            List<int> list = new List<int>();

            list.Add(1);
            list.Add(2);
            list.Add(3);

            Assert.AreEqual(1, ObjectUtilities.GetIndexedValue(list, 0));
            Assert.AreEqual(2, ObjectUtilities.GetIndexedValue(list, 1));
            Assert.AreEqual(3, ObjectUtilities.GetIndexedValue(list, 2));
        }

        [TestMethod]
        public void GetIndexedValuesFromDictionary()
        {
            Dictionary<string, int> numbers = new Dictionary<string, int>();

            numbers["one"] = 1;
            numbers["two"] = 2;
            numbers["three"] = 3;

            Assert.AreEqual(1, ObjectUtilities.GetIndexedValue(numbers, "one"));
            Assert.AreEqual(2, ObjectUtilities.GetIndexedValue(numbers, "two"));
            Assert.AreEqual(3, ObjectUtilities.GetIndexedValue(numbers, "three"));
        }

        [TestMethod]
        public void SetIndexedValuesInArrays()
        {
            int[] array = new int[2];

            ObjectUtilities.SetIndexedValue(array, 0, 1);
            ObjectUtilities.SetIndexedValue(array, 1, 2);

            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
        }

        [TestMethod]
        public void SetIndexedValuesInList()
        {
            List<int> list = new List<int>();

            ObjectUtilities.SetIndexedValue(list, 0, 1);
            ObjectUtilities.SetIndexedValue(list, 1, 2);
            ObjectUtilities.SetIndexedValue(list, 2, 3);

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void SetIndexedValuesInDictionary()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            ObjectUtilities.SetIndexedValue(dictionary, "one", 1);
            ObjectUtilities.SetIndexedValue(dictionary, "two", 2);
            ObjectUtilities.SetIndexedValue(dictionary,  "three", 3);

            Assert.AreEqual(1, dictionary["one"]);
            Assert.AreEqual(2, dictionary["two"]);
            Assert.AreEqual(3, dictionary["three"]);
        }

        [TestMethod]
        public void SetValue()
        {
            Person person = new Person();

            ObjectUtilities.SetValue(person, "FirstName", "Adam");

            Assert.AreEqual("Adam", person.FirstName);
        }

        [TestMethod]
        public void GetNamesNativeObject()
        {
            var person = new Person();
            var result = ObjectUtilities.GetNames(person);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<string>));

            var names = (IList<string>)result;

            Assert.IsTrue(names.Contains("FirstName"));
            Assert.IsTrue(names.Contains("LastName"));
            Assert.IsTrue(names.Contains("GetName"));
            Assert.IsTrue(names.Contains("NameEvent"));
        }

        private object DummyFunction(IList<object> arguments)
        {
            return null;
        }
    }
}

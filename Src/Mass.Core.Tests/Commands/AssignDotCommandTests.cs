namespace Mass.Core.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Commands;
    using Mass.Core.Expressions;
    using Mass.Core.Language;

    [TestClass]
    public class AssignDotCommandTests
    {
        [TestMethod]
        public void AssignValue()
        {
            AssignDotCommand cmd = new AssignDotCommand(new DotExpression(new NameExpression("obj"), "age"), new ConstantExpression(1));
            Context context = new Context();
            DynamicObject obj = new DynamicObject();
            context.SetValue("obj", obj);

            var result = cmd.Execute(context);

            Assert.AreEqual(1, result);
            Assert.AreEqual(1, obj.GetValue("age"));
        }
    }
}

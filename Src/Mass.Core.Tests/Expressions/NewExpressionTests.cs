namespace Mass.Core.Tests.Expressions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mass.Core.Expressions;
    using Mass.Core.Language;

    [TestClass]
    public class NewExpressionTests
    {
        [TestMethod]
        public void EvaluateNewExpression()
        {
            DefinedClass dclass = new DefinedClass("Dog");
            NewExpression expr = new NewExpression(new ConstantExpression(dclass), new IExpression[] { });

            var result = expr.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var obj = (DynamicObject)result;

            Assert.AreSame(dclass, obj.Class);
        }
    }
}

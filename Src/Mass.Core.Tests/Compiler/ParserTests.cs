namespace Mass.Core.Tests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Compiler;
    using Mass.Core.Exceptions;
    using Mass.Core.Expressions;
    using Mass.Core.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParseInteger()
        {
            Parser parser = new Parser("123");
            var expected = new ConstantExpression(123);
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseName()
        {
            Parser parser = new Parser("foo");
            var expected = new NameExpression("foo");
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseString()
        {
            Parser parser = new Parser("\"foo\"");
            var expected = new ConstantExpression("foo");
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseAddTwoIntegers()
        {
            Parser parser = new Parser("1+2");
            var expected = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Add);
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseAddTwoIntegersInParentheses()
        {
            Parser parser = new Parser("(1+2)");
            var expected = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Add);
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void RaiseIsMissingParenthesis()
        {
            Parser parser = new Parser("(1+2");

            try
            {
                parser.ParseExpression();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SyntaxError));
                Assert.AreEqual("expected ')'", ex.Message);
            }
        }

        [TestMethod]
        public void ParseSubtractTwoIntegers()
        {
            Parser parser = new Parser("1-2");
            var expected = new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Subtract);
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseSubtractThreeIntegers()
        {
            Parser parser = new Parser("1-2-3");
            var expected = new BinaryArithmeticExpression(new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Subtract), new ConstantExpression(3), ArithmeticOperator.Subtract);
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseMultiplyTwoIntegers()
        {
            Parser parser = new Parser("3*2");
            var expected = new BinaryArithmeticExpression(new ConstantExpression(3), new ConstantExpression(2), ArithmeticOperator.Multiply);
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseAddAndMultiplyIntegers()
        {
            Parser parser = new Parser("1+3*2");
            var expected = new BinaryArithmeticExpression(new ConstantExpression(1), new BinaryArithmeticExpression(new ConstantExpression(3), new ConstantExpression(2), ArithmeticOperator.Multiply), ArithmeticOperator.Add);
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseDivideTwoIntegers()
        {
            Parser parser = new Parser("3/2");
            var expected = new BinaryArithmeticExpression(new ConstantExpression(3), new ConstantExpression(2), ArithmeticOperator.Divide);
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseSubtractAndDivideIntegers()
        {
            Parser parser = new Parser("1-3/2");
            var expected = new BinaryArithmeticExpression(new ConstantExpression(1), new BinaryArithmeticExpression(new ConstantExpression(3), new ConstantExpression(2), ArithmeticOperator.Divide), ArithmeticOperator.Subtract);
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseCallExpressionSimplePrint()
        {
            Parser parser = new Parser("print(123)");
            var expected = new CallExpression("print", new IExpression[] { new ConstantExpression(123) });
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseCallExpressionPrintWithTwoArguments()
        {
            Parser parser = new Parser("print(1,2)");
            var expected = new CallExpression("print", new IExpression[] { new ConstantExpression(1), new ConstantExpression(2) });
            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseSimpleAssignCommand()
        {
            Parser parser = new Parser("a=2");
            var expected = new AssignCommand("a", new ConstantExpression(2));
            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseSimpleAssignCommandWithEndOfLine()
        {
            Parser parser = new Parser("a=2\n");
            var expected = new AssignCommand("a", new ConstantExpression(2));
            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseSimpleAssignCommandPrecededByAnEndOfLine()
        {
            Parser parser = new Parser("\na=2");
            var expected = new AssignCommand("a", new ConstantExpression(2));
            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseExpressionCommand()
        {
            Parser parser = new Parser("1+2");
            var expected = new ExpressionCommand(new BinaryArithmeticExpression(new ConstantExpression(1), new ConstantExpression(2), ArithmeticOperator.Add));
            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseSimpleNameAsExpressionCommand()
        {
            Parser parser = new Parser("a");
            var expected = new ExpressionCommand(new NameExpression("a"));
            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseSimpleNameAndNewLineAsExpressionCommand()
        {
            Parser parser = new Parser("a\n");
            var expected = new ExpressionCommand(new NameExpression("a"));
            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseSimpleIfCommand()
        {
            Parser parser = new Parser("if 1\n a=1\nend");
            var expected = new IfCommand(new ConstantExpression(1), new AssignCommand("a", new ConstantExpression(1)));
            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseIfCommandWithCompositeThenCommand()
        {
            Parser parser = new Parser("if 1\n a=1\n b=2\nend");
            var expected = new IfCommand(new ConstantExpression(1), new CompositeCommand(new ICommand[] { new AssignCommand("a", new ConstantExpression(1)), new AssignCommand("b", new ConstantExpression(2)) }));
            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        [ExpectedException(typeof(SyntaxError))]
        public void RaiseIfNoEndAtIf()
        {
            Parser parser = new Parser("if 1\n a=1\n");

            parser.ParseCommand();
        }

        [TestMethod]
        public void ParseSimpleDefineCommand()
        {
            Parser parser = new Parser("define foo()\na=1\nend");
            var expected = new DefineCommand("foo", new string[] { }, new AssignCommand("a", new ConstantExpression(1)));
            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseDefineCommandWithParametersInParentheses()
        {
            Parser parser = new Parser("define foo(a, b)\na+b\nend");
            var expected = new DefineCommand("foo", new string[] { "a", "b" }, new ExpressionCommand(new BinaryArithmeticExpression(new NameExpression("a"), new NameExpression("b"), ArithmeticOperator.Add)));
            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void RaiseIfNoEndOfCommand()
        {
            Parser parser = new Parser("println(1) println(2)");

            try
            {
                parser.ParseCommand();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SyntaxError));
                Assert.AreEqual("end of command expected", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseIfDefineHasNoName()
        {
            Parser parser = new Parser("define \na=1\nend");

            try
            {
                parser.ParseCommand();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SyntaxError));
                Assert.AreEqual("name expected", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseIfDefineIsAtEnd()
        {
            Parser parser = new Parser("define");

            try
            {
                parser.ParseCommand();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SyntaxError));
                Assert.AreEqual("name expected", ex.Message);
            }
        }

        [TestMethod]
        public void ParseCompareExpressions()
        {
            Parser parser = new Parser("1==2 1!=2 1<2 1>2 1<=2 1>=2");
            var expected = new IExpression[] 
            {
                new CompareExpression(new ConstantExpression(1), new ConstantExpression(2), CompareOperator.Equal),
                new CompareExpression(new ConstantExpression(1), new ConstantExpression(2), CompareOperator.NotEqual),
                new CompareExpression(new ConstantExpression(1), new ConstantExpression(2), CompareOperator.Less),
                new CompareExpression(new ConstantExpression(1), new ConstantExpression(2), CompareOperator.Greater),
                new CompareExpression(new ConstantExpression(1), new ConstantExpression(2), CompareOperator.LessOrEqual),
                new CompareExpression(new ConstantExpression(1), new ConstantExpression(2), CompareOperator.GreaterOrEqual)
            };

            foreach (var exp in expected)
                Assert.AreEqual(exp, parser.ParseExpression());

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseWhileCommand()
        {
            Parser cmdparser = new Parser("a = a + 1");
            ICommand body = cmdparser.ParseCommand();
            Parser exprparser = new Parser("a < 6");
            IExpression expr = exprparser.ParseExpression();

            Parser parser = new Parser("while (a<6)\na=a+1\nend");
            ICommand expected = new WhileCommand(expr, body);

            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseEmptyClass()
        {
            Parser parser = new Parser("class Dog\nend");
            ICommand expected = new ClassCommand("Dog", new CompositeCommand(new ICommand[] { }));

            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseNewObject()
        {
            Parser parser = new Parser("new Dog(\"Nero\")");
            IExpression expected = new NewExpression(new NameExpression("Dog"), new IExpression[] { new ConstantExpression("Nero") });

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseDotExpression()
        {
            Parser parser = new Parser("nero.name");
            IExpression expected = new DotExpression(new NameExpression("nero"), "name");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseDoubleDotExpression()
        {
            Parser parser = new Parser("customer.address.street");
            IExpression expected = new DotExpression(new DotExpression(new NameExpression("customer"), "address"), "street");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseCallDotExpression()
        {
            Parser parser = new Parser("rectangle.area()");
            IExpression expected = new CallDotExpression(new DotExpression(new NameExpression("rectangle"), "area"), new IExpression[] { });

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseAssignDotCommand()
        {
            Parser parser = new Parser("obj.age = 800");
            ICommand expected = new AssignDotCommand(new DotExpression(new NameExpression("obj"), "age"), new ConstantExpression(800));

            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseReturnCommand()
        {
            Parser parser = new Parser("return 1");
            ICommand expected = new ReturnCommand(new ConstantExpression(1));

            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseForEachCommand()
        {
            Parser parser = new Parser("for k in n\n a=a+k\nend");
            ICommand expected = new ForEachCommand("k", new NameExpression("n"), new AssignCommand("a", new BinaryArithmeticExpression(new NameExpression("a"), new NameExpression("k"), ArithmeticOperator.Add)));

            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Assert.IsNull(parser.ParseCommand());
        }
    }
}

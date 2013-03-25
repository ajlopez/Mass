namespace Mass.Core.Tests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Compiler;
    using Mass.Core.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void GetName()
        {
            Lexer lexer = new Lexer("name");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("name", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetNameWithDigits()
        {
            Lexer lexer = new Lexer("name123");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("name123", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetNameWithUnderscore()
        {
            Lexer lexer = new Lexer("name_123");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("name_123", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetNameWithInitialUnderscore()
        {
            Lexer lexer = new Lexer("_foo");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("_foo", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void RaiseIfUnexpectedCharacter()
        {
            Lexer lexer = new Lexer("\\");

            try
            {
                lexer.NextToken();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SyntaxError));
                Assert.AreEqual("unexpected '\\'", ex.Message);
            }
        }

        [TestMethod]
        public void GetNameWithSpaces()
        {
            Lexer lexer = new Lexer("  name   ");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("name", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void SkipComment()
        {
            Lexer lexer = new Lexer("# this is a comment");

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void SkipCommentUpToEndOfLine()
        {
            Lexer lexer = new Lexer("# this is a comment\n");

            var token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.EndOfLine, token.Type);
            Assert.AreEqual("\n", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetInteger()
        {
            Lexer lexer = new Lexer("123");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
            Assert.AreEqual(TokenType.Integer, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetIntegerWithSpaces()
        {
            Lexer lexer = new Lexer("  123   ");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
            Assert.AreEqual(TokenType.Integer, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetString()
        {
            Lexer lexer = new Lexer("\"foo\"");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Value);
            Assert.AreEqual(TokenType.String, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void PushToken()
        {
            Lexer lexer = new Lexer("\"foo\"");
            var original = lexer.NextToken();

            lexer.PushToken(original);

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreSame(original, result);
            Assert.AreEqual("foo", result.Value);
            Assert.AreEqual(TokenType.String, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void RaiseIfStringIsNotClosed()
        {
            Lexer lexer = new Lexer("\"foo");

            try
            {
                lexer.NextToken();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SyntaxError));
                Assert.AreEqual("unclosed string", ex.Message);
            }
        }

        [TestMethod]
        public void GetAssignOperator()
        {
            Lexer lexer = new Lexer("=");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("=", result.Value);
            Assert.AreEqual(TokenType.Operator, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetComparisonOperators()
        {
            Lexer lexer = new Lexer("== != < > <= >=");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("==", result.Value);
            Assert.AreEqual(TokenType.Operator, result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("!=", result.Value);
            Assert.AreEqual(TokenType.Operator, result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("<", result.Value);
            Assert.AreEqual(TokenType.Operator, result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(">", result.Value);
            Assert.AreEqual(TokenType.Operator, result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("<=", result.Value);
            Assert.AreEqual(TokenType.Operator, result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(">=", result.Value);
            Assert.AreEqual(TokenType.Operator, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetCommaAsSeparator()
        {
            Lexer lexer = new Lexer(",");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(",", result.Value);
            Assert.AreEqual(TokenType.Separator, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetDotAsSeparator()
        {
            Lexer lexer = new Lexer(".");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(".", result.Value);
            Assert.AreEqual(TokenType.Separator, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetParenthesesAsSeparators()
        {
            Lexer lexer = new Lexer("()");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("(", result.Value);
            Assert.AreEqual(TokenType.Separator, result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(")", result.Value);
            Assert.AreEqual(TokenType.Separator, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetPlusAsOperator()
        {
            Lexer lexer = new Lexer("+");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("+", result.Value);
            Assert.AreEqual(TokenType.Operator, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetEndOfLineWithNewLine()
        {
            Lexer lexer = new Lexer("\n");
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(TokenType.EndOfLine, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetFourArithmeticOperators()
        {
            Lexer lexer = new Lexer("+ - * /");

            for (int k = 0; k < 4; k++)
            {
                var result = lexer.NextToken();
                Assert.IsNotNull(result);
                Assert.AreEqual(TokenType.Operator, result.Type);
                Assert.IsNotNull(result.Value);
                Assert.AreEqual(1, result.Value.Length);
                Assert.AreEqual("+-*/"[k], result.Value[0]);
            }

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetSimpleAdd()
        {
            Lexer lexer = new Lexer("1+2");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(TokenType.Integer, result.Type);
            Assert.AreEqual("1", result.Value);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(TokenType.Operator, result.Type);
            Assert.AreEqual("+", result.Value);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(TokenType.Integer, result.Type);
            Assert.AreEqual("2", result.Value);
        }

        [TestMethod]
        public void GetSimpleAddNames()
        {
            Lexer lexer = new Lexer("one+two");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(TokenType.Name, result.Type);
            Assert.AreEqual("one", result.Value);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(TokenType.Operator, result.Type);
            Assert.AreEqual("+", result.Value);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(TokenType.Name, result.Type);
            Assert.AreEqual("two", result.Value);
        }
    }
}

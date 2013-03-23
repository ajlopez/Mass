namespace Mass.Core.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Exceptions;
    using Mass.Core.Expressions;
    using Mass.Core.Language;

    public class Parser
    {
        private static string[][] binaryoperators = new string[][] { new string[] { "==", "!=", "<", ">", "<=", ">=" }, new string[] { "+", "-" }, new string[] { "*", "/" } };
        private Lexer lexer;

        public Parser(string text)
        {
            this.lexer = new Lexer(text);
        }

        public IExpression ParseExpression()
        {
            return this.ParseBinaryExpression(0);
        }

        public ICommand ParseCommand()
        {
            Token token = this.lexer.NextToken();

            while (token != null && token.Type == TokenType.EndOfLine)
                token = this.lexer.NextToken();

            if (token == null)
                return null;

            if (token.Type == TokenType.Name)
            {
                if (token.Value == "if")
                    return this.ParseIfCommand();

                if (token.Value == "while")
                    return this.ParseWhileCommand();

                if (token.Value == "define")
                    return this.ParseDefineCommand();
            }

            this.lexer.PushToken(token);

            IExpression expr = this.ParseExpression();

            if (!(expr is NameExpression))
            {
                this.ParseEndOfCommand();
                return new ExpressionCommand(expr);
            }

            token = this.lexer.NextToken();

            if (token == null)
            {
                this.ParseEndOfCommand();
                return new ExpressionCommand(expr);
            }

            if (token.Type != TokenType.Operator || token.Value != "=")
            {
                this.lexer.PushToken(token);
                this.ParseEndOfCommand();
                return new ExpressionCommand(expr);
            }

            ICommand cmd;

            cmd = new AssignCommand(((NameExpression)expr).Name, this.ParseExpression());

            this.ParseEndOfCommand();

            return cmd;
        }

        private IfCommand ParseIfCommand()
        {
            IExpression condition = this.ParseExpression();
            this.ParseEndOfCommand();
            ICommand thencommand = this.ParseCommandList();
            this.ParseEndOfCommand();
            return new IfCommand(condition, thencommand);
        }

        private WhileCommand ParseWhileCommand()
        {
            IExpression condition = this.ParseExpression();
            this.ParseEndOfCommand();
            ICommand command = this.ParseCommandList();
            this.ParseEndOfCommand();
            return new WhileCommand(condition, command);
        }

        private DefineCommand ParseDefineCommand()
        {
            string name = this.ParseName();
            IList<string> parameters = this.ParseParameterList();
            this.ParseEndOfCommand();
            ICommand body = this.ParseCommandList();
            this.ParseEndOfCommand();
            return new DefineCommand(name, parameters, body);
        }

        private IList<string> ParseParameterList()
        {
            IList<string> parameters = new List<string>();

            this.ParseToken(TokenType.Separator, "(");

            for (string name = this.TryParseName(); name != null; name = this.ParseName())
            {
                parameters.Add(name);
                if (!this.TryParseToken(TokenType.Separator, ","))
                    break;
            }

            this.ParseToken(TokenType.Separator, ")");

            return parameters;
        }

        private IList<IExpression> ParseExpressionList()
        {
            IList<IExpression> expressions = new List<IExpression>();

            this.ParseToken(TokenType.Separator, "(");

            for (IExpression expression = this.ParseExpression(); expression != null; expression = this.ParseExpression())
            {
                expressions.Add(expression);
                if (!this.TryParseToken(TokenType.Separator, ","))
                    break;
            }

            this.ParseToken(TokenType.Separator, ")");

            return expressions;
        }

        private ICommand ParseCommandList()
        {
            Token token;
            IList<ICommand> commands = new List<ICommand>();

            for (token = this.lexer.NextToken(); token != null && (token.Type != TokenType.Name || token.Value != "end"); token = this.lexer.NextToken())
            {
                this.lexer.PushToken(token);
                commands.Add(this.ParseCommand());
            }

            this.lexer.PushToken(token);
            this.ParseName("end");
            this.ParseEndOfCommand();

            if (commands.Count == 1)
                return commands[0];

            return new CompositeCommand(commands);
        }

        private void ParseEndOfCommand()
        {
            Token token = this.lexer.NextToken();

            if (!this.IsEndOfCommand(token))
                throw new SyntaxError("end of command expected");
        }

        private bool IsEndOfCommand(Token token)
        {
            if (token == null)
                return true;

            return token.Type == TokenType.EndOfLine;
        }

        private IExpression ParseBinaryExpression(int level)
        {
            if (level >= binaryoperators.Length)
                return this.ParseTerm();

            IExpression expr = this.ParseBinaryExpression(level + 1);

            if (expr == null)
                return null;

            Token token;

            for (token = this.lexer.NextToken(); token != null && this.IsBinaryOperator(level, token); token = this.lexer.NextToken())
            {
                if (token.Value == "+")
                    expr = new BinaryArithmeticExpression(expr, this.ParseBinaryExpression(level + 1), ArithmeticOperator.Add);
                if (token.Value == "-")
                    expr = new SubtractExpression(expr, this.ParseBinaryExpression(level + 1));
                if (token.Value == "*")
                    expr = new MultiplyExpression(expr, this.ParseBinaryExpression(level + 1));
                if (token.Value == "/")
                    expr = new DivideExpression(expr, this.ParseBinaryExpression(level + 1));
                if (token.Value == "==")
                    expr = new CompareExpression(expr, this.ParseBinaryExpression(level + 1), CompareOperator.Equal);
                if (token.Value == "!=")
                    expr = new CompareExpression(expr, this.ParseBinaryExpression(level + 1), CompareOperator.NotEqual);
                if (token.Value == "<")
                    expr = new CompareExpression(expr, this.ParseBinaryExpression(level + 1), CompareOperator.Less);
                if (token.Value == ">")
                    expr = new CompareExpression(expr, this.ParseBinaryExpression(level + 1), CompareOperator.Greater);
                if (token.Value == "<=")
                    expr = new CompareExpression(expr, this.ParseBinaryExpression(level + 1), CompareOperator.LessOrEqual);
                if (token.Value == ">=")
                    expr = new CompareExpression(expr, this.ParseBinaryExpression(level + 1), CompareOperator.GreaterOrEqual);
            }

            if (token != null)
                this.lexer.PushToken(token);

            return expr;
        }

        private IExpression ParseTerm()
        {
            IExpression expr =  this.ParseSimpleTerm();

            if (expr == null)
                return expr;

            if (!(expr is NameExpression))
                return expr;

            if (!this.PeekToken(TokenType.Separator, "("))
                return expr;

            return new CallExpression(((NameExpression)expr).Name, this.ParseExpressionList());
        }

        private IExpression ParseSimpleTerm()
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                return null;

            if (token.Type == TokenType.Integer)
                return new ConstantExpression(int.Parse(token.Value));

            if (token.Type == TokenType.String)
                return new ConstantExpression(token.Value);

            if (token.Type == TokenType.Name)
                return new NameExpression(token.Value);

            if (token.Type == TokenType.Separator && token.Value == "(")
            {
                IExpression expr = this.ParseExpression();
                this.ParseToken(TokenType.Separator, ")");
                return expr;
            }

            this.lexer.PushToken(token);

            return null;
        }

        private void ParseName(string name)
        {
            this.ParseToken(TokenType.Name, name);
        }

        private void ParseToken(TokenType type, string value)
        {
            Token token = this.lexer.NextToken();

            if (token == null || token.Type != type || token.Value != value)
                throw new SyntaxError(string.Format("expected '{0}'", value));
        }

        private string ParseName()
        {
            Token token = this.lexer.NextToken();

            if (token == null || token.Type != TokenType.Name)
                throw new SyntaxError("name expected");

            return token.Value;
        }

        private bool TryParseToken(TokenType type, string value)
        {
            Token token = this.lexer.NextToken();

            if (token != null && token.Type == type && token.Value == value)
                return true;

            this.lexer.PushToken(token);

            return false;
        }

        private string TryParseName()
        {
            Token token = this.lexer.NextToken();

            if (token != null && token.Type == TokenType.Name)
                return token.Value;

            this.lexer.PushToken(token);

            return null;
        }

        private bool IsBinaryOperator(int level, Token token)
        {
            return token.Type == TokenType.Operator && binaryoperators[level].Contains(token.Value);
        }

        private bool PeekToken(TokenType type, string value)
        {
            Token token = this.lexer.NextToken();
            this.lexer.PushToken(token);

            if (token != null && token.Type == type && token.Value == value)
                return true;

            return false;
        }
    }
}

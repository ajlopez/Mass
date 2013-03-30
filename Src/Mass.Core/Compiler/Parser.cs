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
        private static string[] endnames = new string[] { "end" };
        private static string[] ifendnames = new string[] { "end", "else" };
        private static ICommand breakcmd = new BreakCommand();
        private static ICommand continuecmd = new ContinueCommand();

        private Lexer lexer;

        public Parser(string text)
        {
            this.lexer = new Lexer(text);
        }

        public IExpression ParseExpression()
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                return null;

            if (token.Type == TokenType.Name && token.Value == "new")
                return this.ParseNewExpression();

            this.lexer.PushToken(token);

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

                if (token.Value == "class")
                    return this.ParseClassCommand();

                if (token.Value == "return")
                    return this.ParseReturnCommand();

                if (token.Value == "for")
                    return this.ParseForCommand();

                if (token.Value == "break")
                {
                    this.ParseEndOfCommand();
                    return breakcmd;
                }

                if (token.Value == "continue")
                {
                    this.ParseEndOfCommand();
                    return continuecmd;
                }
            }

            this.lexer.PushToken(token);

            IExpression expr = this.ParseExpression();

            if (!(expr is NameExpression) && !(expr is DotExpression) && !(expr is IndexedExpression))
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

            if (expr is NameExpression)
                cmd = new AssignCommand(((NameExpression)expr).Name, this.ParseExpression());
            else if (expr is DotExpression)
                cmd = new AssignDotCommand((DotExpression)expr, this.ParseExpression());
            else
                cmd = new AssignIndexedCommand((IndexedExpression)expr, this.ParseExpression());

            this.ParseEndOfCommand();

            return cmd;
        }

        private NewExpression ParseNewExpression()
        {
            string name = this.ParseName();

            while (this.TryParseToken(TokenType.Separator, "."))
                name += "." + this.ParseName();

            if (name.Contains('.'))
                return new NewExpression(new QualifiedNameExpression(name), this.ParseExpressionList());

            return new NewExpression(new NameExpression(name), this.ParseExpressionList());
        }

        private IfCommand ParseIfCommand()
        {
            IExpression condition = this.ParseExpression();
            this.ParseEndOfCommand();
            string endname;
            ICommand thencommand = this.ParseCommandList(ifendnames, out endname);

            if (endname != "else")
                return new IfCommand(condition, thencommand);

            return new IfCommand(condition, thencommand, this.ParseCommandList());
        }

        private ICommand ParseForCommand()
        {
            string name = this.ParseName();
            ICommand command;

            if (this.TryParseToken(TokenType.Operator, "="))
            {
                IExpression fromexpression = this.ParseExpression();
                this.ParseToken(TokenType.Name, "to");
                IExpression toexpression = this.ParseExpression();
                IExpression stepexpression;

                if (this.TryParseToken(TokenType.Name, "step"))
                    stepexpression = this.ParseExpression();
                else
                    stepexpression = new ConstantExpression(1);

                command = this.ParseCommandList();

                return new ForCommand(name, fromexpression, toexpression, stepexpression, command);
            }

            this.ParseName("in");
            IExpression expression = this.ParseExpression();
            this.ParseEndOfCommand();
            command = this.ParseCommandList();
            return new ForEachCommand(name, expression, command);
        }

        private ReturnCommand ParseReturnCommand()
        {
            IExpression expression = this.ParseExpression();
            this.ParseEndOfCommand();
            return new ReturnCommand(expression);
        }

        private WhileCommand ParseWhileCommand()
        {
            IExpression condition = this.ParseExpression();
            this.ParseEndOfCommand();
            ICommand command = this.ParseCommandList();
            return new WhileCommand(condition, command);
        }

        private DefineCommand ParseDefineCommand()
        {
            string name = this.ParseName();
            IList<string> parameters = this.ParseParameterList();
            this.ParseEndOfCommand();
            ICommand body = this.ParseCommandList();
            return new DefineCommand(name, parameters, body);
        }

        private ClassCommand ParseClassCommand()
        {
            string name = this.ParseName();
            this.ParseEndOfCommand();
            ICommand body = this.ParseCommandList();
            return new ClassCommand(name, body);
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
            return this.ParseExpressionList("(", ")");
        }

        private IList<IExpression> ParseExpressionList(string open, string close)
        {
            IList<IExpression> expressions = new List<IExpression>();

            this.ParseToken(TokenType.Separator, open);

            for (IExpression expression = this.ParseExpression(); expression != null; expression = this.ParseExpression())
            {
                expressions.Add(expression);
                if (!this.TryParseToken(TokenType.Separator, ","))
                {
                    this.TryParseToken(TokenType.EndOfLine, "\n");
                    break;
                }
            }

            this.ParseToken(TokenType.Separator, close);

            return expressions;
        }

        private ICommand ParseCommandList()
        {
            string endname;
            return this.ParseCommandList(endnames, out endname);
        }

        private ICommand ParseCommandList(IList<string> endnames, out string endname)
        {
            Token token;
            IList<ICommand> commands = new List<ICommand>();

            for (token = this.lexer.NextToken(); token != null && (token.Type != TokenType.Name || !endnames.Contains(token.Value)); token = this.lexer.NextToken())
            {
                this.lexer.PushToken(token);
                commands.Add(this.ParseCommand());
            }

            this.lexer.PushToken(token);
            endname = this.ParseName();
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
                    expr = new BinaryArithmeticExpression(expr, this.ParseBinaryExpression(level + 1), ArithmeticOperator.Subtract);
                if (token.Value == "*")
                    expr = new BinaryArithmeticExpression(expr, this.ParseBinaryExpression(level + 1), ArithmeticOperator.Multiply);
                if (token.Value == "/")
                    expr = new BinaryArithmeticExpression(expr, this.ParseBinaryExpression(level + 1), ArithmeticOperator.Divide);
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
            IExpression expr = this.ParseSimpleTerm();

            if (expr == null)
                return expr;

            while (true)
            {
                var original = expr;

                if (expr is NameExpression && this.PeekToken(TokenType.Separator, "("))
                    expr = new CallExpression(((NameExpression)expr).Name, this.ParseExpressionList());

                if (expr is DotExpression && this.PeekToken(TokenType.Separator, "("))
                    expr = new CallDotExpression((DotExpression)expr, this.ParseExpressionList());

                while (this.TryParseToken(TokenType.Separator, "."))
                    expr = new DotExpression(expr, this.ParseName());

                while (this.TryParseToken(TokenType.Separator, "["))
                {
                    expr = new IndexedExpression(expr, new IExpression[] { this.ParseExpression() });
                    this.ParseToken(TokenType.Separator, "]");
                }

                if (original == expr)
                    break;
            }

            return expr;
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

            if (token.Type == TokenType.Separator && token.Value == "[")
            {
                this.lexer.PushToken(token);
                return new ArrayExpression(this.ParseExpressionList("[", "]"));
            }

            if (token.Type == TokenType.Separator && token.Value == "{")
            {
                this.lexer.PushToken(token);
                return this.ParseDynamicObjectExpression();
            }

            this.lexer.PushToken(token);

            return null;
        }

        private DynamicObjectExpression ParseDynamicObjectExpression()
        {
            IList<AssignCommand> commands = new List<AssignCommand>();

            this.ParseToken(TokenType.Separator, "{");

            while (true)
            {
                Token token = this.lexer.PeekToken();

                if (token != null && token.Type == TokenType.Separator && token.Value == "}")
                    break;

                string name = this.ParseName();
                this.ParseToken(TokenType.Operator, "=");
                IExpression expression = this.ParseExpression();
                
                commands.Add(new AssignCommand(name, expression));

                if (!this.TryParseToken(TokenType.Separator, ","))
                {
                    this.TryParseToken(TokenType.EndOfLine, "\n");
                    break;
                }
            }

            this.ParseToken(TokenType.Separator, "}");

            return new DynamicObjectExpression(commands);
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

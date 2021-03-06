﻿namespace Mass.Core.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Exceptions;

    public class Lexer
    {
        private const char Quote = '"';
        private const char StartComment = '#';
        private const char EndOfLine = '\n';

        private const string Separators = "(),.[]{}";

        private static string[] operators = new string[] { "+", "-", "*", "/", "=", "<", ">", "!", "==", "<=", ">=", "!=" };

        private string text;
        private int position = 0;
        private Token lasttoken;
        private Stack<Token> tokens = new Stack<Token>();

        public Lexer(string text)
        {
            this.text = text;
        }

        public Token NextToken()
        {
            var token = this.InternalNextToken();

            while (this.lasttoken != null && token != null && token.Type == TokenType.EndOfLine && this.IsContinueToken(this.lasttoken))
                token = this.InternalNextToken();

            this.lasttoken = token;

            return token;
        }

        public void PushLastToken()
        {
            this.PushToken(this.lasttoken);
        }

        public Token PeekToken()
        {
            var token = this.NextToken();
            this.PushToken(token);
            return token;
        }

        public void PushToken(Token token)
        {
            this.tokens.Push(token);
        }

        private Token InternalNextToken()
        {
            if (this.tokens.Count > 0)
                return this.tokens.Pop();

            int ich = this.NextFirstChar();

            if (ich == -1)
                return null;

            char ch = (char)ich;

            if (ch == EndOfLine)
                return new Token(TokenType.EndOfLine, "\n");

            if (ch == Quote)
                return this.NextString();

            if (operators.Contains(ch.ToString()))
            {
                string value = ch.ToString();
                ich = this.NextChar();

                if (ich >= 0)
                {
                    value += (char)ich;
                    if (operators.Contains(value))
                        return new Token(TokenType.Operator, value);

                    this.BackChar();
                }

                return new Token(TokenType.Operator, ch.ToString());
            }

            if (Separators.Contains(ch))
                return new Token(TokenType.Separator, ch.ToString());

            if (char.IsDigit(ch))
                return this.NextInteger(ch);

            if (char.IsLetter(ch) || ch == '_')
                return this.NextName(ch);

            throw new SyntaxError(string.Format("unexpected '{0}'", ch));
        }

        private Token NextName(char ch)
        {
            string value = ch.ToString();
            int ich;

            for (ich = this.NextChar(); ich >= 0 && ((char)ich == '_' || char.IsLetterOrDigit((char)ich)); ich = this.NextChar())
                value += (char)ich;

            if (ich >= 0)
                this.BackChar();

            return new Token(TokenType.Name, value);
        }

        private Token NextString()
        {
            string value = string.Empty;
            int ich;

            for (ich = this.NextChar(); ich >= 0 && ((char)ich) != Quote; ich = this.NextChar())
                value += (char)ich;

            if (ich < 0)
                throw new SyntaxError("unclosed string");

            return new Token(TokenType.String, value);
        }

        private Token NextInteger(char ch)
        {
            string value = ch.ToString();
            int ich;

            for (ich = this.NextChar(); ich >= 0 && char.IsDigit((char)ich); ich = this.NextChar())
                value += (char)ich;

            if (ich == '.')
                return this.NextReal(value + ".");

            if (ich >= 0)
                this.BackChar();

            return new Token(TokenType.Integer, value);
        }

        private Token NextReal(string value)
        {
            int ich;

            for (ich = this.NextChar(); ich >= 0 && char.IsDigit((char)ich); ich = this.NextChar())
                value += (char)ich;

            if (ich >= 0)
                this.BackChar();

            return new Token(TokenType.Real, value);
        }

        private int NextFirstChar()
        {
            while (this.position < this.text.Length && this.text[this.position] != '\n' && char.IsWhiteSpace(this.text[this.position]))
                this.position++;

            return this.NextChar();
        }

        private int NextChar()
        {
            if (this.position >= this.text.Length)
                return -1;

            char ch = this.text[this.position++];

            if (ch == StartComment)
            {
                this.position++;

                while (this.position < this.text.Length && this.text[this.position] != '\n')
                    this.position++;

                if (this.position >= this.text.Length)
                    return -1;

                ch = this.text[this.position++];
            }

            return ch;
        }

        private void BackChar()
        {
            if (this.position <= this.text.Length)
                this.position--;
        }

        private bool IsContinueToken(Token token)
        {
            if (token.Type == TokenType.Operator)
                return true;

            if (token.Type == TokenType.Separator)
                if (token.Value == "," || token.Value == "[" || token.Value == "(" || token.Value == "{")
                    return true;

            return false;
        }
    }
}

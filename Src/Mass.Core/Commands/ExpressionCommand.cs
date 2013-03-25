namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;

    public class ExpressionCommand : ICommand
    {
        private IExpression expression;

        public ExpressionCommand(IExpression expression)
        {
            this.expression = expression;
        }

        public object Execute(Context context)
        {
            return this.expression.Evaluate(context);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ExpressionCommand)
            {
                var expr = (ExpressionCommand)obj;

                return this.expression.Equals(expr.expression);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.expression.GetHashCode();
        }
    }
}

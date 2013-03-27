namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;

    public class ReturnCommand : ICommand
    {
        private static int hashcode = typeof(ReturnCommand).GetHashCode();

        private IExpression expression;

        public ReturnCommand(IExpression expression)
        {
            this.expression = expression;
        }

        public object Execute(Context context)
        {
            object value = this.expression.Evaluate(context);
            context.SetReturn(value);
            return value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ReturnCommand)
            {
                var expr = (ReturnCommand)obj;

                return this.expression.Equals(expr.expression);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.expression.GetHashCode() + hashcode;
        }
    }
}

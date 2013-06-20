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
            if (this.expression == null)
            {
                context.SetReturnValue(null);
                return null;
            }

            object value = this.expression.Evaluate(context);
            context.SetReturnValue(value);
            return value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ReturnCommand)
            {
                var expr = (ReturnCommand)obj;

                if (this.expression == null)
                    return expr.expression == null;

                return this.expression.Equals(expr.expression);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (this.expression == null)
                return hashcode;

            return this.expression.GetHashCode() + hashcode;
        }
    }
}

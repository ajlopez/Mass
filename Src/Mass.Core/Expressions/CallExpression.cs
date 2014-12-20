namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;

    public class CallExpression : IExpression
    {
        private static int hashcode = typeof(CallExpression).GetHashCode();

        private IExpression expression;
        private IList<IExpression> arguments;

        public CallExpression(IExpression expression, IList<IExpression> arguments)
        {
            this.expression = expression;
            this.arguments = arguments;
        }

        public object Evaluate(Context context)
        {
            IFunction function = (IFunction)this.expression.Evaluate(context);

            IList<object> values = new List<object>();

            foreach (var argument in this.arguments)
                values.Add(argument.Evaluate(context));

            return function.Apply(null, values);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is CallExpression)
            {
                var expr = (CallExpression)obj;

                if (!this.expression.Equals(expr.expression))
                    return false;

                if (this.arguments.Count != expr.arguments.Count)
                    return false;

                for (var k = 0; k < this.arguments.Count; k++)
                    if (!this.arguments[k].Equals(expr.arguments[k]))
                        return false;

                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int result = this.expression.GetHashCode() + hashcode;

            foreach (var argument in this.arguments)
                result += argument.GetHashCode();

            return result;
        }
    }
}

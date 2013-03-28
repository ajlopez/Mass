namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;

    public class ArrayExpression : IExpression
    {
        private static int hashcode = typeof(ArrayExpression).GetHashCode();

        private IList<IExpression> expressions;

        public ArrayExpression(IList<IExpression> expressions)
        {
            this.expressions = expressions;
        }

        public object Evaluate(Context context)
        {
            IList<object> values = new List<object>();

            foreach (var argument in this.expressions)
                values.Add(argument.Evaluate(context));

            return values;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ArrayExpression)
            {
                var expr = (ArrayExpression)obj;

                if (this.expressions.Count != expr.expressions.Count)
                    return false;

                for (var k = 0; k < this.expressions.Count; k++)
                    if (!this.expressions[k].Equals(expr.expressions[k]))
                        return false;

                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int result = hashcode;

            foreach (var expression in this.expressions)
                result += expression.GetHashCode();

            return result;
        }
    }
}

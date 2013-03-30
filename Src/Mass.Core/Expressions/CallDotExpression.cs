namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Mass.Core.Utilities;

    public class CallDotExpression : IExpression
    {
        private DotExpression expression;
        private IList<IExpression> arguments;

        public CallDotExpression(DotExpression expression, IList<IExpression> arguments)
        {
            this.expression = expression;
            this.arguments = arguments;
        }

        public object Evaluate(Context context)
        {
            var obj = this.expression.Expression.Evaluate(context);
            IList<object> values = new List<object>();

            foreach (var argument in this.arguments)
                values.Add(argument.Evaluate(context));

            var vals = obj as IValues;

            if (vals != null)
            {
                values.Insert(0, vals);
                var method = (IFunction)vals.Get(this.expression.Name);

                return method.Apply(values);
            }

            return ObjectUtilities.GetValue(obj, this.expression.Name, values);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is CallDotExpression)
            {
                var expr = (CallDotExpression)obj;

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
            int result = this.expression.GetHashCode();

            foreach (var argument in this.arguments)
                result += argument.GetHashCode();

            return result;
        }
    }
}

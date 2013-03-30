namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Mass.Core.Utilities;

    public class NewExpression : IExpression
    {
        private IExpression expression;
        private IList<IExpression> arguments;

        public NewExpression(IExpression expression, IList<IExpression> arguments)
        {
            this.expression = expression;
            this.arguments = arguments;
        }

        public object Evaluate(Context context)
        {
            object result = this.expression.Evaluate(context);

            IList<object> values = new List<object>();

            foreach (var argument in this.arguments)
                values.Add(argument.Evaluate(context));

            if (result is Type)
                return Activator.CreateInstance((Type)result, values.ToArray());

            DefinedClass dclass = (DefinedClass)result;

            var obj = dclass.CreateInstance();

            var initialize = obj.Get("initialize") as IFunction;

            if (initialize != null)
            {
                values.Insert(0, obj);
                initialize.Apply(values);
            }

            return obj;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is NewExpression)
            {
                var expr = (NewExpression)obj;

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

namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Functions;

    public class FunctionExpression : IExpression
    {
        private IList<string> parameters;
        private ICommand body;

        public FunctionExpression(IList<string> parameters, ICommand body)
        {
            this.parameters = parameters;
            this.body = body;
        }

        public object Evaluate(Context context)
        {
            return new DefinedFunction(this.body, this.parameters, context);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is FunctionExpression)
            {
                var expr = (FunctionExpression)obj;

                if (!this.body.Equals(expr.body))
                    return false;

                if (this.parameters.Count != expr.parameters.Count)
                    return false;

                for (var k = 0; k < this.parameters.Count; k++)
                    if (!this.parameters[k].Equals(expr.parameters[k]))
                        return false;

                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int result = this.body.GetHashCode();

            foreach (var argument in this.parameters)
                result += this.parameters.GetHashCode();

            return result;
        }
    }
}

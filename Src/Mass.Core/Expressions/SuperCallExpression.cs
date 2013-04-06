namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;
    using Mass.Core.Language;

    public class SuperCallExpression : IExpression
    {
        private static int hashcode = typeof(SuperCallExpression).GetHashCode();

        private IList<IExpression> arguments;

        public SuperCallExpression(IList<IExpression> arguments)
        {
            this.arguments = arguments;
        }

        public object Evaluate(Context context)
        {
            IList<object> values = new List<object>();

            foreach (var argument in this.arguments)
                values.Add(argument.Evaluate(context));

            DynamicObject dobj = (DynamicObject)context.Get("self");

            if (dobj.Class == null || dobj.Class.Superclass == null)
                return null;

            IFunction initialize = dobj.Class.Superclass.GetInstanceMethod("initialize");

            if (initialize == null)
                return null;

            return initialize.Apply(dobj, values);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is SuperCallExpression)
            {
                var expr = (SuperCallExpression)obj;

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
            int result = hashcode;

            foreach (var argument in this.arguments)
                result += argument.GetHashCode();

            return result;
        }
    }
}

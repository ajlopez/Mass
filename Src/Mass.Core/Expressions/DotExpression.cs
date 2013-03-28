namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Language;
    using Mass.Core.Utilities;

    public class DotExpression : IExpression
    {
        private static int hashcode = typeof(DotExpression).GetHashCode();
        private IExpression expression;
        private string name;

        public DotExpression(IExpression expression, string name)
        {
            this.expression = expression;
            this.name = name;
        }

        public IExpression Expression { get { return this.expression; } }

        public string Name { get { return this.name; } }

        public object Evaluate(Context context)
        {
            var obj = this.expression.Evaluate(context);

            var dobj = obj as DynamicObject;

            if (dobj != null)
                return dobj.GetValue(this.name);

            return ObjectUtilities.GetValue(obj, this.name);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is DotExpression)
            {
                var expr = (DotExpression)obj;

                return this.name.Equals(expr.name) && this.expression.Equals(expr.expression);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int result = this.name.GetHashCode() + this.expression.GetHashCode() + hashcode;

            return result;
        }
    }
}

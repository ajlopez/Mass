namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;
    using Mass.Core.Language;
    using Mass.Core.Utilities;

    public class IndexedExpression : IExpression
    {
        private IExpression expression;
        private IExpression indexexpression;

        public IndexedExpression(IExpression expression, IExpression indexexpression)
        {
            this.expression = expression;
            this.indexexpression = indexexpression;
        }

        public IExpression Expression { get { return this.expression; } }

        public IExpression IndexExpression { get { return this.indexexpression; } }

        public object Evaluate(Context context)
        {
            var obj = this.expression.Evaluate(context);
            object index = this.indexexpression.Evaluate(context);

            if (obj is DynamicObject)
                return ((DynamicObject)obj).Get(index.ToString());

            return ObjectUtilities.GetIndexedValue(obj, index);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is IndexedExpression)
            {
                var expr = (IndexedExpression)obj;

                if (!this.expression.Equals(expr.expression))
                    return false;

                if (!this.indexexpression.Equals(expr.indexexpression))
                    return false;

                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.expression.GetHashCode() + this.indexexpression.GetHashCode();
        }
    }
}

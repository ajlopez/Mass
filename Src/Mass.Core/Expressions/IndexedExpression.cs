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
        private IList<IExpression> indexexpressions;

        public IndexedExpression(IExpression expression, IList<IExpression> indexexpressions)
        {
            this.expression = expression;
            this.indexexpressions = indexexpressions;
        }

        public object Evaluate(Context context)
        {
            var obj = this.expression.Evaluate(context);
            IList<object> indexes = new List<object>();

            foreach (var indexexpression in this.indexexpressions)
                indexes.Add(indexexpression.Evaluate(context));

            if (obj is DynamicObject && indexes.Count == 1)
                return ((DynamicObject)obj).GetValue(indexes[0].ToString());

            return ObjectUtilities.GetIndexedValue(obj, indexes);
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

                if (this.indexexpressions.Count != expr.indexexpressions.Count)
                    return false;

                for (var k = 0; k < this.indexexpressions.Count; k++)
                    if (!this.indexexpressions[k].Equals(expr.indexexpressions[k]))
                        return false;

                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int result = this.expression.GetHashCode();

            foreach (var indexexpression in this.indexexpressions)
                result += indexexpression.GetHashCode();

            return result;
        }
    }
}

namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CompareExpression : BinaryExpression
    {
        private static IDictionary<CompareOperator, Func<object, object, object>> functions = new Dictionary<CompareOperator, Func<object, object, object>>();
        private CompareOperator @operator;

        static CompareExpression()
        {
            functions[CompareOperator.Equal] = (left, right) => Operators.EqualObject(left, right);
            functions[CompareOperator.NotEqual] = (left, right) => Operators.NotEqualObject(left, right);
            functions[CompareOperator.Less] = (left, right) => Operators.LessObject(left, right);
            functions[CompareOperator.Greater] = (left, right) => Operators.GreaterObject(left, right);
            functions[CompareOperator.LessOrEqual] = (left, right) => Operators.LessEqualObject(left, right);
            functions[CompareOperator.GreaterOrEqual] = (left, right) => Operators.GreaterEqualObject(left, right);
        }

        public CompareExpression(IExpression left, IExpression right, CompareOperator @operator)
            : base(left, right)
        {
            this.@operator = @operator;
        }

        public override object Apply(object leftvalue, object rightvalue)
        {
            return functions[this.@operator](leftvalue, rightvalue);
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            return this.@operator == ((CompareExpression)obj).@operator;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + (int)this.@operator;
        }
    }
}

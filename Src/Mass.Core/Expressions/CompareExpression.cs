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
            functions[CompareOperator.Equal] = CompareEqual;
            functions[CompareOperator.NotEqual] = CompareNotEqual;
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

        private static object CompareEqual(object left, object right)
        {
            if (left == null)
                return right == null;

            return left.Equals(right);
        }

        private static object CompareNotEqual(object left, object right)
        {
            if (left == null)
                return right != null;

            return !left.Equals(right);
        }
    }
}

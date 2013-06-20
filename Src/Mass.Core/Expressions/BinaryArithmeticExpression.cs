namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class BinaryArithmeticExpression : BinaryExpression
    {
        private static IDictionary<ArithmeticOperator, Func<object, object, object>> functions = new Dictionary<ArithmeticOperator, Func<object, object, object>>();
        private ArithmeticOperator @operator;
        private Func<object, object, object> function;

        static BinaryArithmeticExpression()
        {
            functions[ArithmeticOperator.Add] = (left, right) => Operators.AddObject(left, right);
            functions[ArithmeticOperator.Subtract] = (left, right) => Operators.SubtractObject(left, right);
            functions[ArithmeticOperator.Multiply] = (left, right) => Operators.MultiplyObject(left, right);
            functions[ArithmeticOperator.Divide] = Divide;
        }

        public BinaryArithmeticExpression(IExpression left, IExpression right, ArithmeticOperator @operator)
            : base(left, right)
        {
            this.@operator = @operator;
            this.function = functions[@operator];
        }

        public override object Apply(object leftvalue, object rightvalue)
        {
            if (leftvalue == null)
                leftvalue = 0;
            if (rightvalue == null)
                rightvalue = 0;

            return this.function(leftvalue, rightvalue);
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            return this.@operator == ((BinaryArithmeticExpression)obj).@operator;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + (int)this.@operator;
        }

        private static object Divide(object left, object right)
        {
            if (left is IConvertible)
                left = ((IConvertible)left).ToDouble(CultureInfo.InvariantCulture);
            if (right is IConvertible)
                right = ((IConvertible)right).ToDouble(CultureInfo.InvariantCulture);

            var result = Operators.DivideObject(left, right);

            if (result is double) 
            {
                double value = (double)result;
                double floor = Math.Floor(value);
                if (floor == value)
                    return (int)value;
            }

            return result;
        }
    }
}

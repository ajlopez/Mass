namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MultiplyExpression : BinaryArithmeticExpression
    {
        public MultiplyExpression(IExpression left, IExpression right)
            : base(left, right, ArithmeticOperator.Multiply)
        {
        }
    }
}

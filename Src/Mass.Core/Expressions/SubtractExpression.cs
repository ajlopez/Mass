namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SubtractExpression : BinaryArithmeticExpression
    {
        public SubtractExpression(IExpression left, IExpression right)
            : base(left, right, ArithmeticOperator.Subtract)
        {
        }
    }
}

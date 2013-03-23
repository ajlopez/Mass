namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DivideExpression : BinaryArithmeticExpression
    {
        public DivideExpression(IExpression left, IExpression right)
            : base(left, right, ArithmeticOperator.Divide)
        {
        }
    }
}

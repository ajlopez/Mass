namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AddExpression : BinaryArithmeticExpression
    {
        public AddExpression(IExpression left, IExpression right)
            : base(left, right, ArithmeticOperator.Add)
        {
        }
    }
}

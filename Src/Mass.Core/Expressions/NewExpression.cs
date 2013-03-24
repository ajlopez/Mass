namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;
    using Mass.Core.Language;

    public class NewExpression : IExpression
    {
        private IExpression expression;
        private IList<IExpression> arguments;

        public NewExpression(IExpression expression, IList<IExpression> arguments)
        {
            this.expression = expression;
            this.arguments = arguments;
        }

        public object Evaluate(Context context)
        {
            DefinedClass dclass = (DefinedClass)this.expression.Evaluate(context);
            return dclass.CreateInstance();
        }
    }
}

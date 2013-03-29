namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;
    using Mass.Core.Language;
    using Mass.Core.Utilities;

    public class AssignIndexedCommand : ICommand
    {
        private static int hashtag = typeof(AssignIndexedCommand).GetHashCode();

        private IndexedExpression leftvalue;
        private IExpression expression;

        public AssignIndexedCommand(IndexedExpression leftvalue, IExpression expression)
        {
            this.leftvalue = leftvalue;
            this.expression = expression;
        }

        public object Execute(Context context)
        {
            var obj = this.leftvalue.Expression.Evaluate(context);
            var index = this.leftvalue.IndexExpressions[0].Evaluate(context);
            object value = this.expression.Evaluate(context);

            if (obj is DynamicObject)
                ((DynamicObject)obj).SetValue(index.ToString(), value);
            else
                ObjectUtilities.SetIndexedValue(obj, new object[] { index }, value);

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is AssignIndexedCommand)
            {
                var cmd = (AssignIndexedCommand)obj;

                return this.leftvalue.Equals(cmd.leftvalue) && this.expression.Equals(cmd.expression);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.leftvalue.GetHashCode() + this.expression.GetHashCode() + hashtag;
        }
    }
}

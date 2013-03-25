namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;
    using Mass.Core.Language;

    public class AssignDotCommand : ICommand
    {
        private static int hashtag = typeof(AssignDotCommand).GetHashCode();

        private DotExpression leftvalue;
        private IExpression expression;

        public AssignDotCommand(DotExpression leftvalue, IExpression expression)
        {
            this.leftvalue = leftvalue;
            this.expression = expression;
        }

        public object Execute(Context context)
        {
            var obj = (DynamicObject)this.leftvalue.Expression.Evaluate(context);
            object value = this.expression.Evaluate(context);
            obj.SetValue(this.leftvalue.Name, value);
            return value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is AssignDotCommand)
            {
                var cmd = (AssignDotCommand)obj;

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

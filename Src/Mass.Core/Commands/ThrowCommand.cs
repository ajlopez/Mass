namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;

    public class ThrowCommand : ICommand
    {
        private static int hashcode = typeof(ThrowCommand).GetHashCode();

        private IExpression expression;

        public ThrowCommand(IExpression expression)
        {
            this.expression = expression;
        }

        public object Execute(Context context)
        {
            var result = this.expression.Evaluate(context);

            if (result is Exception)
                throw (Exception)result;

            throw new Exception(result.ToString());
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ThrowCommand)
            {
                var cmd = (ThrowCommand)obj;

                return this.expression.Equals(cmd.expression);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.expression.GetHashCode() + hashcode;
        }
    }
}

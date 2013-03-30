namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Functions;
    using Mass.Core.Language;

    public class DynamicObjectExpression : IExpression
    {
        private static int hashcode = typeof(DynamicObjectExpression).GetHashCode();

        private IList<AssignCommand> commands;

        public DynamicObjectExpression(IList<AssignCommand> commands)
        {
            this.commands = commands;
        }

        public object Evaluate(Context context)
        {
            DynamicObject obj = new DynamicObject();

            foreach (var command in this.commands)
                obj.Set(command.Name, command.Expression.Evaluate(context));

            return obj;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is DynamicObjectExpression)
            {
                var expr = (DynamicObjectExpression)obj;

                if (this.commands.Count != expr.commands.Count)
                    return false;

                for (var k = 0; k < this.commands.Count; k++)
                    if (!this.commands[k].Equals(expr.commands[k]))
                        return false;

                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int result = hashcode;

            foreach (var expression in this.commands)
                result += expression.GetHashCode();

            return result;
        }
    }
}

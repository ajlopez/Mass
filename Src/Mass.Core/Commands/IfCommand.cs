namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;

    public class IfCommand : ICommand
    {
        private static int hashcode = typeof(IfCommand).GetHashCode();

        private IExpression condition;
        private ICommand thencommand;
        private ICommand elsecommand;

        public IfCommand(IExpression condition, ICommand thencommand)
            : this(condition, thencommand, null)
        {
        }

        public IfCommand(IExpression condition, ICommand thencommand, ICommand elsecommand)
        {
            this.condition = condition;
            this.thencommand = thencommand;
            this.elsecommand = elsecommand;
        }

        public object Execute(Context context)
        {
            object value = this.condition.Evaluate(context);

            if (value == null || false.Equals(value))
            {
                if (this.elsecommand != null)
                    return this.elsecommand.Execute(context);
                return null;
            }

            return this.thencommand.Execute(context);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is IfCommand)
            {
                var cmd = (IfCommand)obj;

                if (this.elsecommand == null)
                {
                    if (cmd.elsecommand != null)
                        return false;
                }
                else if (!this.elsecommand.Equals(cmd.elsecommand))
                    return false;

                return this.condition.Equals(cmd.condition) && this.thencommand.Equals(cmd.thencommand);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int result = this.condition.GetHashCode() + this.thencommand.GetHashCode() + hashcode;

            if (this.elsecommand != null)
                result += this.elsecommand.GetHashCode();

            return result;
        }
    }
}

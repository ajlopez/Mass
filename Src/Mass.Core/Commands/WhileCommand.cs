namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;

    public class WhileCommand : ICompositeCommand
    {
        private static int hashcode = typeof(WhileCommand).GetHashCode();

        private IExpression condition;
        private ICommand command;
        private IList<string> varnames;

        public WhileCommand(IExpression condition, ICommand command)
        {
            this.condition = condition;
            this.command = command;

            if (command is VarCommand)
                this.varnames = new List<string> { ((VarCommand)command).Name };
            else if (command is ICompositeCommand)
                this.varnames = ((ICompositeCommand)command).VarNames;
            else
                this.varnames = new List<string>();
        }

        public IList<string> VarNames { get { return this.varnames; } }

        public object Execute(Context context)
        {
            for (object value = this.condition.Evaluate(context); value != null && !false.Equals(value); value = this.condition.Evaluate(context))
            {
                this.command.Execute(context);
                if (context.HasReturnValue())
                    return context.GetReturnValue();
                if (context.HasContinue())
                    context.ClearContinue();
                if (context.HasBreak())
                {
                    context.ClearBreak();
                    break;
                }
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is WhileCommand)
            {
                var cmd = (WhileCommand)obj;

                return this.condition.Equals(cmd.condition) && this.command.Equals(cmd.command);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.condition.GetHashCode() + this.command.GetHashCode() + hashcode;
        }
    }
}

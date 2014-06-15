namespace Mass.Core.Commands
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;

    public class ForCommand : ICommand
    {
        private static int hashcode = typeof(ForEachCommand).GetHashCode();

        private string name;
        private IExpression fromexpression;
        private IExpression toexpression;
        private IExpression stepexpression;
        private ICommand command;

        public ForCommand(string name, IExpression fromexpression, IExpression toexpression, IExpression stepexpression, ICommand command)
        {
            this.name = name;
            this.fromexpression = fromexpression;
            this.toexpression = toexpression;
            this.stepexpression = stepexpression;
            this.command = command;
        }

        public object Execute(Context context)
        {
            var from = (int)this.fromexpression.Evaluate(context);
            var to = (int)this.toexpression.Evaluate(context);
            var step = (int)this.stepexpression.Evaluate(context);

            for (int k = from; k <= to; k += step)
            {
                context.Set(this.name, k);
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

            if (obj is ForCommand)
            {
                var cmd = (ForCommand)obj;

                return this.name.Equals(cmd.name) && this.fromexpression.Equals(cmd.fromexpression) && this.toexpression.Equals(cmd.toexpression) && this.stepexpression.Equals(cmd.stepexpression) && this.command.Equals(cmd.command);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode() + this.fromexpression.GetHashCode() + this.toexpression.GetHashCode() + this.stepexpression.GetHashCode() + this.command.GetHashCode() + hashcode;
        }
    }
}

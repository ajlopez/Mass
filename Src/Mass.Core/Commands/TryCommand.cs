namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;

    public class TryCommand : ICommand
    {
        private static int hashcode = typeof(TryCommand).GetHashCode();

        private ICommand command;
        private ICommand catchcommand;

        public TryCommand(ICommand command, ICommand catchcommand)
        {
            this.command = command;
            this.catchcommand = catchcommand;
        }

        public object Execute(Context context)
        {
            try
            {
                this.command.Execute(context);

                if (context.HasReturnValue())
                    return context.GetReturnValue();
            }
            catch
            {
                this.catchcommand.Execute(context);
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is TryCommand)
            {
                var cmd = (TryCommand)obj;

                return this.command.Equals(cmd.command) && this.catchcommand.Equals(cmd.catchcommand);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.command.GetHashCode() + this.catchcommand.GetHashCode() + hashcode;
        }
    }
}

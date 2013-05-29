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

        public TryCommand(ICommand command)
        {
            this.command = command;
        }

        public object Execute(Context context)
        {
            this.command.Execute(context);

            if (context.HasReturnValue())
                return context.GetReturnValue();

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is TryCommand)
            {
                var cmd = (TryCommand)obj;

                return this.command.Equals(cmd.command);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.command.GetHashCode() + hashcode;
        }
    }
}

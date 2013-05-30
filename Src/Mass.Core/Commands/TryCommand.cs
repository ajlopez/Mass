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
        private ICommand catchcmd;

        public TryCommand(ICommand command, ICommand catchcmd)
        {
            this.command = command;
            this.catchcmd = catchcmd;
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
                this.catchcmd.Execute(context);
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

                return this.command.Equals(cmd.command) && this.catchcmd.Equals(cmd.catchcmd);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.command.GetHashCode() + this.catchcmd.GetHashCode() + hashcode;
        }
    }
}

namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CompositeCommand : ICommand
    {
        private IList<ICommand> commands = new List<ICommand>();
        private IList<string> varnames = new List<string>();
        private IList<ICommand> defines = new List<ICommand>();

        public CompositeCommand(IList<ICommand> commands)
        {
            if (commands == null)
                return;

            foreach (var command in commands)
            {
                if (command is DefineCommand) 
                {
                    defines.Add(command);
                    continue;
                }

                if (command is VarCommand)
                {
                    varnames.Add(((VarCommand)command).Name);
                    continue;
                }

                this.commands.Add(command);
            }
        }

        public IList<string> VarNames { get { return this.varnames; } }

        public object Execute(Context context)
        {
            object result = null;

            foreach (var varname in this.varnames)
                context.Set(varname, null, true);

            foreach (var define in this.defines)
                result = define.Execute(context);

            foreach (var command in this.commands)
            {
                result = command.Execute(context);
                if (context.HasReturnValue())
                    return context.GetReturnValue();
                if (context.HasBreak() || context.HasContinue())
                    return null;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is CompositeCommand)
            {
                var cmd = (CompositeCommand)obj;

                if (this.commands.Count != cmd.commands.Count)
                    return false;

                for (int k = 0; k < this.commands.Count; k++)
                    if (!this.commands[k].Equals(cmd.commands[k]))
                        return false;

                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int result = 0;

            foreach (var command in this.commands)
            {
                result *= 17;
                result += command.GetHashCode();
            }

            return result;
        }
    }
}

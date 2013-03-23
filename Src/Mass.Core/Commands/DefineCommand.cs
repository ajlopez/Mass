namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;

    public class DefineCommand : ICommand
    {
        private string name;
        private IList<string> parameters;
        private ICommand command;

        public DefineCommand(string name, IList<string> parameters, ICommand command)
        {
            this.name = name;
            this.parameters = parameters;
            this.command = command;
        }

        public object Execute(Context context)
        {
            var result = new DefinedFunction(this.command, this.parameters, context);

            context.SetValue(this.name, result);

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is DefineCommand)
            {
                var cmd = (DefineCommand)obj;

                if (this.parameters.Count != cmd.parameters.Count)
                    return false;

                for (int k = 0; k < this.parameters.Count; k++)
                    if (this.parameters[k] != cmd.parameters[k])
                        return false;

                return this.name == cmd.name && this.command.Equals(cmd.command);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int result = this.name.GetHashCode() + this.command.GetHashCode();

            foreach (var parameter in this.parameters)
                result += parameter.GetHashCode();

            return result;
        }
    }
}

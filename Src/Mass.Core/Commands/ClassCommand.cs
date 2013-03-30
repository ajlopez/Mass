namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;
    using Mass.Core.Language;

    public class ClassCommand : ICommand
    {
        private static int hashcode = typeof(ClassCommand).GetHashCode();
        private string name;
        private ICommand command;

        public ClassCommand(string name, ICommand command)
        {
            this.name = name;
            this.command = command;
        }

        public object Execute(Context context)
        {
            var value = context.Find(this.name);

            if (value == null || !(value is DefinedClass))
            {
                var newclass = new DefinedClass(this.name);
                context.Set(this.name, newclass);
                value = newclass;
            }

            var dclass = (DefinedClass)value;

            Context classcontext = new Context(context, dclass);

            this.command.Execute(classcontext);

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ClassCommand)
            {
                var cmd = (ClassCommand)obj;

                return this.name == cmd.name && this.command.Equals(cmd.command);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode() + this.command.GetHashCode() + hashcode;
        }
    }
}

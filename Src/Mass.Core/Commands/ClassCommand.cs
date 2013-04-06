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
        private string super;
        private ICommand command;

        public ClassCommand(string name, ICommand command)
            : this(name, null, command)
        {
        }

        public ClassCommand(string name, string super, ICommand command)
        {
            this.name = name;
            this.super = super;
            this.command = command;
        }

        public object Execute(Context context)
        {
            var value = context.Get(this.name);

            if (value == null || !(value is DefinedClass))
            {
                DefinedClass superclass = null;

                if (this.super != null)
                    superclass = (DefinedClass)context.Get(this.super);
                
                var newclass = new DefinedClass(this.name, superclass);
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

                if (this.super == null)
                {
                    if (cmd.super != null)
                        return false;
                }
                else if (!this.super.Equals(cmd.super))
                    return false;

                return this.name == cmd.name && this.command.Equals(cmd.command);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var result = this.name.GetHashCode() + this.command.GetHashCode() + hashcode;

            if (this.super != null)
                result += this.super.GetHashCode();

            return result;
        }
    }
}

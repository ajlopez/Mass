namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;

    public class VarCommand : ICommand
    {
        private static int hashtag = typeof(VarCommand).GetHashCode();

        private string name;

        public VarCommand(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public object Execute(Context context)
        {
            if (!context.HasValue(this.name, true))
                context.Set(this.name, null, true);

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is VarCommand)
            {
                var cmd = (VarCommand)obj;

                return this.name.Equals(cmd.name);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode() + hashtag;
        }
    }
}

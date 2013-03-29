namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;

    public class BreakCommand : ICommand
    {
        private static int hashcode = typeof(BreakCommand).GetHashCode();

        public object Execute(Context context)
        {
            context.SetBreak();
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is BreakCommand)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return hashcode;
        }
    }
}

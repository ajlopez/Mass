namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;

    public class ContinueCommand : ICommand
    {
        private static int hashcode = typeof(ContinueCommand).GetHashCode();

        public object Execute(Context context)
        {
            context.SetContinue();
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ContinueCommand)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return hashcode;
        }
    }
}

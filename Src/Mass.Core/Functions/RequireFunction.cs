namespace Mass.Core.Functions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Language;

    public class RequireFunction : IFunction
    {
        private Machine machine;

        public RequireFunction(Machine machine)
        {
            this.machine = machine;
        }

        public object Apply(IList<object> values)
        {
            string name = (string)values[0];

            FileInfo info = new FileInfo(name);

            if (string.IsNullOrEmpty(info.Extension))
                name += ".ms";

            return machine.ExecuteFile(name);
        }
    }
}

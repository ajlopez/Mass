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
        private string path;

        public RequireFunction(Machine machine)
            : this(machine, null)
        {
        }

        public RequireFunction(Machine machine, string path)
        {
            this.machine = machine;
            this.path = path;
        }

        public object Apply(IList<object> values)
        {
            string name = (string)values[0];

            FileInfo info = new FileInfo(name);

            string filename = name;

            if (string.IsNullOrEmpty(info.Extension))
                filename += ".ms";

            if (!string.IsNullOrEmpty(this.path))
                filename = Path.Combine(this.path, filename);

            if (!Path.IsPathRooted(name) && !File.Exists(filename))
            {
                string modules = "modules";

                if (!string.IsNullOrEmpty(this.path))
                    modules = Path.Combine(this.path, modules);

                filename = Path.Combine(modules, name);

                if (Directory.Exists(filename))
                    filename = Path.Combine(filename, "init.ms");                
                else if (string.IsNullOrEmpty(info.Extension))
                    filename += ".ms";
            }

            return machine.ExecuteFile(filename);
        }
    }
}

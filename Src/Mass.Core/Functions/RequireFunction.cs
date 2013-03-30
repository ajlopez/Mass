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

            string filename = this.GetFilename(this.path, name);

            if (filename == null)
                throw new InvalidOperationException(string.Format("cannot find module '{0}'", name));

            return machine.ExecuteFile(filename);
        }

        private string GetFilename(string path, string name)
        {
            string filename = name;
            FileInfo fileinfo = new FileInfo(filename);

            if (string.IsNullOrEmpty(fileinfo.Extension))
                filename += ".ms";

            if (!string.IsNullOrEmpty(path))
                filename = Path.Combine(path, filename);

            if (File.Exists(filename))
                return filename;
            
            if (Path.IsPathRooted(name))
                return null;

            return this.GetFilename(path, "modules", name);
        }

        private string GetFilename(string path, string moduledirectory, string name)
        {
            string directoryname;

            if (string.IsNullOrEmpty(path))
                directoryname = moduledirectory;
            else
                directoryname = Path.Combine(path, moduledirectory);

            string filename = Path.Combine(directoryname, name);
            FileInfo fileinfo = new FileInfo(filename);

            if (Directory.Exists(filename))
                filename = Path.Combine(filename, "init.ms");
            else if (string.IsNullOrEmpty(fileinfo.Extension))
                filename += ".ms";

            if (File.Exists(filename))
                return filename;

            return null;
        }
    }
}

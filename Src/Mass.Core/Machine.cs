namespace Mass.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Compiler;
    using Mass.Core.Functions;

    public class Machine
    {
        private Context rootcontext = new Context();
        private IDictionary<string, object> filecache = new Dictionary<string, object>();

        public Machine()
        {
            this.rootcontext.Set("global", this.rootcontext);
            this.rootcontext.Set("print", new PrintFunction(System.Console.Out));
            this.rootcontext.Set("println", new PrintlnFunction(System.Console.Out));
            this.rootcontext.Set("require", new RequireFunction(this));
        }

        public Context RootContext { get { return this.rootcontext; } }

        public object ExecuteText(string text)
        {
            return this.ExecuteText(text, this.rootcontext);
        }

        public object ExecuteText(string text, Context context)
        {
            Parser parser = new Parser(text);
            object result = null;

            for (var command = parser.ParseCommand(); command != null; command = parser.ParseCommand())
                result = command.Execute(context);

            return result;
        }

        public object ExecuteFile(string filename)
        {
            return this.ExecuteFile(filename, false);
        }

        public object ExecuteFile(string filename, bool usecache)
        {
            FileInfo fileinfo = new FileInfo(filename);
            string path = fileinfo.DirectoryName;
            Context newcontext = new Context(this.rootcontext);
            newcontext.Set("require", new RequireFunction(this, path));
            return this.ExecuteFile(filename, newcontext, usecache);
        }

        public object ExecuteFile(string filename, Context context)
        {
            return this.ExecuteFile(filename, context, false);
        }

        public object ExecuteFile(string filename, Context context, bool usecache)
        {
            string fullname = (new FileInfo(filename)).FullName;

            if (usecache && this.filecache.ContainsKey(fullname))
                return this.filecache[fullname];

            object value = this.ExecuteText(System.IO.File.ReadAllText(filename), context);

            this.filecache[fullname] = value;

            return value;
        }
    }
}

namespace Mass.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Compiler;
    using Mass.Core.Functions;

    public class Machine
    {
        private Context rootcontext = new Context();

        public Machine()
        {
            this.rootcontext.SetValue("print", new PrintFunction(System.Console.Out));
            this.rootcontext.SetValue("println", new PrintlnFunction(System.Console.Out));
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
            return this.ExecuteFile(filename, this.rootcontext);
        }

        public object ExecuteFile(string filename, Context context)
        {
            return this.ExecuteText(System.IO.File.ReadAllText(filename), context);
        }
    }
}

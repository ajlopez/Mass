namespace Mass.Core.Functions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Language;

    public class PrintlnFunction : IFunction
    {
        private TextWriter writer;

        public PrintlnFunction(TextWriter writer)
        {
            this.writer = writer;
        }

        public object Apply(object self, IList<object> values)
        {
            foreach (var value in values)
                this.writer.WriteLine(value);

            return null;
        }
    }
}

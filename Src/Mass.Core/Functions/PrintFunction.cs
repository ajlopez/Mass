namespace Mass.Core.Functions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mass.Core.Language;

    public class PrintFunction : IFunction
    {
        private TextWriter writer;

        public PrintFunction(TextWriter writer)
        {
            this.writer = writer;
        }

        public object Apply(IList<object> values)
        {
            foreach (var value in values)
                this.writer.Write(value);

            return null;
        }

        public object Apply(object self, IList<object> values)
        {
            return this.Apply(values);
        }
    }
}

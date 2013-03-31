namespace Mass.Core.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Commands;
    using Mass.Core.Language;

    public class DefinedFunction : IFunction
    {
        private ICommand body;
        private IList<string> parameters;
        private Context context;

        public DefinedFunction(ICommand body, IList<string> parameters, Context context)
        {
            this.body = body;
            this.context = context;
            this.parameters = parameters;
        }

        public object Apply(IList<object> values)
        {
            Context newcontext = new Context(this.context, true, null);
            return this.DoApply(newcontext, values);
        }

        public object Apply(object self, IList<object> values)
        {
            Context newcontext = new Context(this.context, true, null);
            newcontext.Set("self", self);
            return this.DoApply(newcontext, values);
        }

        private object DoApply(Context newcontext, IList<object> values)
        {
            int k = 0;
            int cv = values.Count;

            foreach (var parameter in this.parameters)
            {
                newcontext.Set(parameter, values[k]);
                k++;
            }

            return this.body.Execute(newcontext);
        }
    }
}

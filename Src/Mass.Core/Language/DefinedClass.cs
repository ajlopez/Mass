namespace Mass.Core.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;

    public class DefinedClass : DynamicObject
    {
        private string name;
        private IDictionary<string, IFunction> methods = new Dictionary<string, IFunction>();

        public DefinedClass(string name)
            : base(null)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public void SetInstanceMethod(string name, IFunction method)
        {
            this.methods[name] = method;
        }

        public IFunction GetInstanceMethod(string name)
        {
            if (this.methods.ContainsKey(name))
                return this.methods[name];

            return null;
        }

        public DynamicObject CreateInstance()
        {
            return new DynamicObject(this);
        }
    }
}

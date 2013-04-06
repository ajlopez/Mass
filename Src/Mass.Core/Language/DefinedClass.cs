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
        private DefinedClass super;
        private IDictionary<string, IFunction> methods = new Dictionary<string, IFunction>();

        public DefinedClass(string name)
            : this(name, null)
        {
        }

        public DefinedClass(string name, DefinedClass super)
            : base(null)
        {
            this.name = name;
            this.super = super;
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

            if (this.super != null)
                return this.super.GetInstanceMethod(name);

            return null;
        }

        public DynamicObject CreateInstance()
        {
            return new DynamicObject(this);
        }
    }
}

namespace Mass.Core.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;

    public class DynamicObject
    {
        private DefinedClass @class;
        private IDictionary<string, object> values = new Dictionary<string, object>();

        public DynamicObject()
            : this(null)
        {
        }

        public DynamicObject(DefinedClass @class)
        {
            this.@class = @class;
        }

        public DefinedClass Class { get { return this.@class; } }

        public void SetValue(string name, object value)
        {
            this.values[name] = value;
        }

        public object GetValue(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            if (this.@class != null)
                return this.@class.GetInstanceMethod(name);

            return null;
        }
    }
}

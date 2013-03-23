namespace Mass.Core.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;

    public class DynamicObject
    {
        private IDictionary<string, object> values = new Dictionary<string, object>();

        public DynamicObject()
        {
        }

        public void SetValue(string name, object value)
        {
            this.values[name] = value;
        }

        public object GetValue(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            return null;
        }
    }
}

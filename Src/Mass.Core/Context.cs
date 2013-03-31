namespace Mass.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Language;

    public class Context : IValues
    {
        private Context parent;
        private DefinedClass @class;
        private IDictionary<string, object> values = new Dictionary<string, object>();
        private object returnvalue;
        private bool hasreturnvalue;
        private bool hasbreak;
        private bool hascontinue;

        public Context()
            : this(null)
        {
        }

        public Context(Context parent)
            : this(parent, null)
        {
        }

        public Context(Context parent, DefinedClass @class)
        {
            this.parent = parent;
            this.@class = @class;
        }

        public DefinedClass Class { get { return this.@class; } }

        public bool HasReturnValue()
        {
            return this.hasreturnvalue;
        }

        public object GetReturnValue()
        {
            return this.returnvalue;
        }

        public void SetReturnValue(object returnvalue)
        {
            this.returnvalue = returnvalue;
            this.hasreturnvalue = true;
        }

        public void Set(string name, object value)
        {
            this.values[name] = value;
        }

        public object Get(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            if (name == "global")
            {
                if (this.parent != null)
                    return this.parent.Get(name);

                return this;
            }

            return null;
        }

        public object Find(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            if (this.parent != null)
                return this.parent.Find(name);

            return null;
        }

        public IList<string> GetNames()
        {
            return this.values.Keys.ToList();
        }

        public bool HasBreak()
        {
            return this.hasbreak;
        }

        public void ClearBreak()
        {
            this.hasbreak = false;
        }

        public void SetBreak()
        {
            this.hasbreak = true;
        }

        public bool HasContinue()
        {
            return this.hascontinue;
        }

        public void ClearContinue()
        {
            this.hascontinue = false;
        }

        public void SetContinue()
        {
            this.hascontinue = true;
        }
    }
}

﻿namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NameExpression : IExpression
    {
        private static int hashcode = typeof(NameExpression).GetHashCode();
        private static IList<object> emptyvalues = new object[] { };
        private string name;

        public NameExpression(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public object Evaluate(Context context)
        {
            return context.Get(this.name);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is NameExpression) 
            {
                var expr = (NameExpression)obj;

                return this.name.Equals(expr.name);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode() + hashcode;
        }
    }
}

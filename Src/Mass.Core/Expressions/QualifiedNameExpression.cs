namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Utilities;

    public class QualifiedNameExpression : IExpression
    {
        private static int hashcode = typeof(QualifiedNameExpression).GetHashCode();
        private static IList<object> emptyvalues = new object[] { };
        private string name;

        public QualifiedNameExpression(string name)
        {
            this.name = name;
        }

        public object Evaluate(Context context)
        {
            return TypeUtilities.GetType(this.name);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is QualifiedNameExpression) 
            {
                var expr = (QualifiedNameExpression)obj;

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

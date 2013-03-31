namespace Mass.Core.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Language;

    public interface IFunction
    {
        object Apply(IList<object> values);

        object Apply(object self, IList<object> values);
    }
}

namespace Mass.Core.Language
{
    using System;

    public interface IValues
    {
        object Get(string name);

        void Set(string name, object value);
    }
}

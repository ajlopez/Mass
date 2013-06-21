namespace Mass.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ICompositeCommand : ICommand
    {
        IList<string> VarNames { get; }
    }
}

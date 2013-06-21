﻿namespace Mass.Core.Commands
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Expressions;

    public class ForEachCommand : ICompositeCommand
    {
        private static int hashcode = typeof(ForEachCommand).GetHashCode();

        private string name;
        private IExpression expression;
        private ICommand command;
        private IList<string> varnames;

        public ForEachCommand(string name, IExpression expression, ICommand command)
        {
            this.name = name;
            this.expression = expression;
            this.command = command;

            if (command is VarCommand)
                this.varnames = new List<string> { ((VarCommand)command).Name };
            else if (command is ICompositeCommand)
                this.varnames = ((ICompositeCommand)command).VarNames;
            else
                this.varnames = new List<string>();
        }

        public IList<string> VarNames { get { return this.varnames; } }

        public object Execute(Context context)
        {
            var values = (IEnumerable)this.expression.Evaluate(context);

            foreach (var value in values)
            {
                context.Set(this.name, value);
                this.command.Execute(context);
                if (context.HasReturnValue())
                    return context.GetReturnValue();
                if (context.HasContinue())
                    context.ClearContinue();
                if (context.HasBreak())
                {
                    context.ClearBreak();
                    break;
                }
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ForEachCommand)
            {
                var cmd = (ForEachCommand)obj;

                return this.name.Equals(cmd.name) && this.expression.Equals(cmd.expression) && this.command.Equals(cmd.command);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode() + this.expression.GetHashCode() + this.command.GetHashCode() + hashcode;
        }
    }
}

namespace Mass.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    public class BinaryArithmeticExpression : BinaryExpression
    {
        private static IDictionary<ArithmeticOperator, IDictionary<TypePair, Func<object, object, object>>> funcs = new Dictionary<ArithmeticOperator, IDictionary<TypePair, Func<object, object, object>>>();
        private ArithmeticOperator @operator;
        private Type tleft;
        private Type tright;
        private Func<object, object, object> function;

        static BinaryArithmeticExpression()
        {
            var tpintint = new TypePair(typeof(int), typeof(int));

            funcs[ArithmeticOperator.Add] = BuildOperations(Expression.Add);
            funcs[ArithmeticOperator.Subtract] = BuildOperations(Expression.Subtract);
            funcs[ArithmeticOperator.Multiply] = BuildOperations(Expression.Multiply);
            funcs[ArithmeticOperator.Divide] = BuildOperations(Expression.Divide);
        }

        public BinaryArithmeticExpression(IExpression left, IExpression right, ArithmeticOperator @operator)
            : base(left, right)
        {
            this.@operator = @operator;
        }

        public override object Apply(object leftvalue, object rightvalue)
        {
            if (leftvalue == null)
                if (this.@operator != ArithmeticOperator.Add || (rightvalue != null && !(rightvalue is string)))
                    leftvalue = 0;
                else
                    return Add(leftvalue, rightvalue);

            if (rightvalue == null)
                if (this.@operator != ArithmeticOperator.Add || (leftvalue != null && !(leftvalue is string)))
                    rightvalue = 0;
                else
                    return Add(leftvalue, rightvalue);

            var tl = leftvalue.GetType();
            var tr = rightvalue.GetType();

            if (tl == this.tleft && tr == this.tright && this.function != null)
                return this.function(leftvalue, rightvalue);

            this.tleft = tl;
            this.tright = tr;

            if (this.@operator == ArithmeticOperator.Add && (tl == typeof(string) || tr == typeof(string)))
                this.function = Add;
            else
                this.function = funcs[this.@operator][new TypePair(tl, tr)];

            return this.function(leftvalue, rightvalue);
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            return this.@operator == ((BinaryArithmeticExpression)obj).@operator;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + (int)this.@operator;
        }

        private static object Add(object left, object right)
        {
            if (left == null && right == null)
                return string.Empty;

            if (left is string)
                if (right == null)
                    return left;
                else
                    return (string)left + right.ToString();

            if (right is string)
                if (left == null)
                    return right;
                else
                    return left.ToString() + (string)right;

            return Operators.AddObject(left, right);
        }

        private static Func<object, object, object> BuildExpression(Type t1, Type t2, Func<Expression, Expression, System.Linq.Expressions.BinaryExpression> oper)
        {
            var ox = Expression.Parameter(typeof(object), "x");
            var oy = Expression.Parameter(typeof(object), "y");
            Expression x = Expression.Unbox(ox, t1);
            Expression y = Expression.Unbox(oy, t1);
            Visit(ref x, ref y);
            var op = oper(x, y);
            var body = Expression.TypeAs(op, typeof(object));
            var lambda = Expression.Lambda<Func<object, object, object>>(body, ox, oy).Compile();
            return lambda;
        }

        private static IDictionary<TypePair, Func<object, object, object>> BuildOperations(Func<Expression, Expression, System.Linq.Expressions.BinaryExpression> oper)
        {
            var dict = new Dictionary<TypePair, Func<object, object, object>>();

            dict[new TypePair(typeof(int), typeof(int))] = BuildExpression(typeof(int), typeof(int), oper);
            dict[new TypePair(typeof(int), typeof(double))] = BuildExpression(typeof(int), typeof(double), oper);
            dict[new TypePair(typeof(double), typeof(int))] = BuildExpression(typeof(double), typeof(int), oper);
            dict[new TypePair(typeof(double), typeof(double))] = BuildExpression(typeof(double), typeof(double), oper);

            return dict;
        }

        private static void Visit(ref Expression left, ref Expression right)
        {
            var leftTypeCode = Type.GetTypeCode(left.Type);
            var rightTypeCode = Type.GetTypeCode(right.Type);

            if (leftTypeCode == rightTypeCode)
                return;

            if (leftTypeCode > rightTypeCode)
                right = Expression.Convert(right, left.Type);
            else
                left = Expression.Convert(left, right.Type);
        }

        private class TypePair
        {
            private Type left;
            private Type right;

            public TypePair(Type left, Type right)
            {
                this.left = left;
                this.right = right;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;

                if (obj is TypePair)
                {
                    TypePair pair = (TypePair)obj;
                    return this.left == pair.left && this.right == pair.right;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return this.left.GetHashCode() + (this.right.GetHashCode() * 17);
            }
        }
    }
}

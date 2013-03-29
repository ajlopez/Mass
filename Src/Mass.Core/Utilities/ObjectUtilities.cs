namespace Mass.Core.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mass.Core.Functions;
    using Mass.Core.Language;

    // Based on AjSharp AjLanguage.ObjectUtilities and PythonSharp.ObjectUtilities
    public class ObjectUtilities
    {
        public static void SetValue(object obj, string name, object value)
        {
            if (obj is DynamicObject)
            {
                ((DynamicObject)obj).SetValue(name, value);

                return;
            }

            Type type = obj.GetType();

            type.InvokeMember(name, System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.SetField | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, obj, new object[] { value });
        }

        public static object GetValue(object obj, string name)
        {
            if (obj is DynamicObject)
                return ((DynamicObject)obj).GetValue(name);

            Type type = obj.GetType();

            try
            {
                return type.InvokeMember(name, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | /* System.Reflection.BindingFlags.InvokeMethod | */ System.Reflection.BindingFlags.Instance, null, obj, null);
            }
            catch
            {
                return type.GetMethod(name);
            }
        }

        public static object GetValue(object obj, string name, IList<object> arguments)
        {
            if (obj is DynamicObject)
            {
                DynamicObject dobj = (DynamicObject)obj;

                if (arguments == null)
                    return dobj.GetValue(name);

                var method = dobj.GetValue(name) as IFunction;

                if (method != null) 
                {
                    var args = new List<object>(arguments);
                    args.Insert(0, dobj);
                    return method.Apply(args);
                }
            }

            if (obj is DynamicObject && (arguments == null || arguments.Count == 0))
                return ((DynamicObject)obj).GetValue(name);

            return GetNativeValue(obj, name, arguments);
        }

        public static IList<string> GetNames(object obj)
        {
            return TypeUtilities.GetNames(obj.GetType());
        }

        public static object GetNativeValue(object obj, string name, IList<object> arguments)
        {
            Type type = obj.GetType();

            return type.InvokeMember(name, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Instance, null, obj, arguments == null ? null : arguments.ToArray());
        }

        public static bool IsNumber(object obj)
        {
            return obj is int ||
                obj is short ||
                obj is long ||
                obj is decimal ||
                obj is double ||
                obj is float ||
                obj is byte;
        }

        // TODO implement a method with only one index
        public static object GetIndexedValue(object obj, IList<object> indexes)
        {
            if (obj is System.Array)
                return GetIndexedValue((System.Array)obj, indexes);

            if (obj is IList)
                return GetIndexedValue((IList)obj, indexes);

            if (obj is IDictionary)
                return GetIndexedValue((IDictionary)obj, indexes);

            if (obj is DynamicObject && indexes != null && indexes.Count == 1)
                return ((DynamicObject)obj).GetValue((string)indexes[0]);

            return GetValue(obj, string.Empty, indexes); 
        }

        // TODO implement a method with only one index
        public static void SetIndexedValue(object obj, IList<object> indexes, object value)
        {
            if (obj is System.Array)
            {
                SetIndexedValue((System.Array)obj, indexes, value);
                return;
            }

            if (obj is IList)
            {
                if (indexes.Count != 1)
                    throw new InvalidOperationException("invalid number of subindices");

                int index = (int)indexes[0];

                IList list = (IList)obj;

                if (list.Count == index)
                    list.Add(value);
                else
                    list[index] = value;

                return;
            }

            if (obj is IDictionary)
            {
                if (indexes.Count != 1)
                    throw new InvalidOperationException("invalid number of subindices");

                ((IDictionary)obj)[indexes[0]] = value;

                return;
            }

            // TODO as in GetIndexedValue, consider Default member
            throw new InvalidOperationException(string.Format("not indexed value of type {0}", obj.GetType().ToString()));
        }

        public static void SetIndexedValue(System.Array array, IList<object> indexes, object value)
        {
            switch (indexes.Count)
            {
                case 1:
                    array.SetValue(value, (int)indexes[0]);
                    return;
                case 2:
                    array.SetValue(value, (int)indexes[0], (int)indexes[1]);
                    return;
                case 3:
                    array.SetValue(value, (int)indexes[0], (int)indexes[1], (int)indexes[2]);
                    return;
            }

            throw new InvalidOperationException("invalid number of subindices");
        }

        private static object GetIndexedValue(System.Array array, IList<object> indexes)
        {
            switch (indexes.Count)
            {
                case 1:
                    return array.GetValue((int)indexes[0]);
                case 2:
                    return array.GetValue((int)indexes[0], (int)indexes[1]);
                case 3:
                    return array.GetValue((int)indexes[0], (int)indexes[1], (int)indexes[2]);
            }

            throw new InvalidOperationException("invalid number of subindices");
        }

        private static object GetIndexedValue(IList list, IList<object> indexes)
        {
            if (indexes.Count != 1)
                throw new InvalidOperationException("invalid number of subindices");

            return list[(int)indexes[0]];
        }

        private static object GetIndexedValue(IDictionary dictionary, IList<object> indexes)
        {
            if (indexes.Count != 1)
                throw new InvalidOperationException("Invalid number of subindices");

            return dictionary[indexes[0]];
        }
    }
}

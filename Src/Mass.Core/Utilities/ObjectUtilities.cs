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
            Type type = obj.GetType();

            type.InvokeMember(name, System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.SetField | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, obj, new object[] { value });
        }

        public static object GetValue(object obj, string name)
        {
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
        public static object GetIndexedValue(object obj, object index)
        {
            if (obj is System.Array)
                return GetIndexedValue((System.Array)obj, index);

            if (obj is IList)
                return GetIndexedValue((IList)obj, index);

            if (obj is IDictionary)
                return GetIndexedValue((IDictionary)obj, index);

            return GetValue(obj, string.Empty, new object[] { index }); 
        }

        // TODO implement a method with only one index
        public static void SetIndexedValue(object obj, object index, object value)
        {
            if (obj is System.Array)
            {
                SetIndexedValue((System.Array)obj, index, value);
                return;
            }

            if (obj is IList)
            {
                int intindex = (int)index;

                IList list = (IList)obj;

                if (list.Count == intindex)
                    list.Add(value);
                else
                    list[intindex] = value;

                return;
            }

            if (obj is IDictionary)
            {
                ((IDictionary)obj)[index] = value;

                return;
            }

            // TODO as in GetIndexedValue, consider Default member
            throw new InvalidOperationException(string.Format("not indexed value of type {0}", obj.GetType().ToString()));
        }

        public static void SetIndexedValue(System.Array array, object index, object value)
        {
            array.SetValue(value, (int)index);
            return;
        }

        private static object GetIndexedValue(System.Array array, object index)
        {
            return array.GetValue((int)index);
        }

        private static object GetIndexedValue(IList list, object index)
        {
            return list[(int)index];
        }

        private static object GetIndexedValue(IDictionary dictionary, object index)
        {
            return dictionary[index];
        }
    }
}

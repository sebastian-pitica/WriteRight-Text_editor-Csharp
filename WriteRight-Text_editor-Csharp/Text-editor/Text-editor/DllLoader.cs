using System;
using System.Reflection;

namespace TextEditor
{
    //Made by: Sebastian
    internal class DllLoader
    {
        internal static object[] LoadDll(string dllName, string className, string methodName)
        {
            Assembly asm;
            Type type;
            MethodInfo mi;
            object obj;
            object[] result = new object[2];

            if (dllName == null || className == null || methodName == null)
            {
                throw new ArgumentNullException("LoadDll(string dllName): arguments: dllName:" + dllName + " className:" + className + " methodName:" + methodName);
            }
            else
            {
                try
                {
                    asm = Assembly.Load(dllName);
                    type = asm.GetType(dllName + "." + className);
                    mi = type.GetMethod(methodName);
                    obj = Activator.CreateInstance(type);
                    result[0] = obj;
                    result[1] = mi;
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

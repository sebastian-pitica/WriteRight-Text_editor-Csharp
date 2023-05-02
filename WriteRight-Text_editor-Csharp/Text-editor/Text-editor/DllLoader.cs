using System;
using System.Reflection;
using static CommonsModule.Utilities;

/**************************************************************************
 *                                                                        *
 *  File:        DllLoader.cs                                             *
 *  Copyright:   (c) 2023, Pitica Sebastian                               *
 *  Description: Fișierul conține funția pentru încărcarea dinamică a     *
 *  funcționalităților din fișierele .dll                                 *
 *                                                                        *
 **************************************************************************/

namespace TextEditor
{
    internal class DllLoader
    {
        /// <summary>
        /// Încărcarcă dinamic o metodă dintr-un fișier .dll
        /// </summary>
        /// <param name="dllName">Numele fișierului .dll</param>
        /// <param name="className">Numele clasei căutate</param>
        /// <param name="methodName">Numele metodei căutate</param>
        /// <returns>Un vector de două elemente ce conține: obiectul prin intermediul căruia va fi apelată metoda și metoda</returns>
        /// <exception cref="ArgumentNullException">Nici unul dintre argumente poate fi null</exception>
        internal static object[] LoadDll(string dllName, string className, string methodName)
        {
            object[] result = new object[2];

            if (dllName == null || className == null || methodName == null)
            {
                throw new ArgumentNullException("LoadDll(string dllName): arguments: dllName:" + dllName + " className:" + className + " methodName:" + methodName);
            }
            try
            {
                var asm = Assembly.Load(dllName);
                var type = asm.GetType(dllName + "." + className);
                var mi = type.GetMethod(methodName);
                var obj = Activator.CreateInstance(type);
                result[0] = obj;
                result[1] = mi;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }
    }
}

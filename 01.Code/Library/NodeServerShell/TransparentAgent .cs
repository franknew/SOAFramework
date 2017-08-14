using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace MicroService.Library
{
    public class TransparentAgent : MarshalByRefObject
    {
        private const BindingFlags bfi = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;

        public TransparentAgent() { }

        /// <summary> Factory method to create an instance of the type whose name is specified,
        /// using the named assembly file and the constructor that best matches the specified parameters. </summary>
        /// <param name="assemblyFile"> The name of a file that contains an assembly where the type named typeName is sought. </param>
        /// <param name="typeName"> The name of the preferred type. </param>
        /// <param name="constructArgs"> An array of arguments that match in number, order,
        　　　　 /// and type the parameters of the constructor to invoke, or null for default constructor. </param>
        /// <returns> The return value is the created object represented as IObjcet. </returns>
        public IObjcet Create(string assemblyFile, string typeName, object[] args)
        {
            return (IObjcet)Activator.CreateInstanceFrom(assemblyFile, typeName, false, bfi, null, args, null, null).Unwrap();
        }

        public T Create<T>(string assemblyPath, string typeName, object[] args)
        {
            string assemblyFile = LoadAssemblyFile(assemblyPath, typeName);
            return (T)Activator.CreateInstanceFrom(assemblyFile, typeName, false, bfi, null, args, null, null).Unwrap();
        }

        public static string LoadAssemblyFile(string assemblyPlugs, string typeName)
        {
            string path = string.Empty;
            DirectoryInfo d = new DirectoryInfo(assemblyPlugs);
            foreach (FileInfo file in d.GetFiles("*.dll"))
            {
                Assembly assembly = Assembly.LoadFile(file.FullName);
                Type type = assembly.GetType(typeName, false);
                if (type != null)
                {
                    path = file.FullName;
                }
            }
            return path;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public class TypeDefinition
    {
        public string TypeName { get; set; }
        public IList<string> GenericArguments { get; set; }

        /// <summary>
        /// 通过类型名称转换成类型定义
        /// </summary>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        public static TypeDefinition FromString(string fullTypeName)
        {
            TypeDefinition t = new TypeDefinition();
            if (fullTypeName.IndexOf("`") > -1)//is generic type
            {
                t.GenericArguments = new List<string>();
                int start = fullTypeName.IndexOf("[");
                t.TypeName = fullTypeName.Substring(0, start);
                var genericArgs = fullTypeName.Substring(start, fullTypeName.Length - start);//sample 2[[System.String], [System.Int32]], remove `2
                genericArgs = genericArgs.Remove(genericArgs.Length - 1, 1).Remove(0, 1);
                //splite by []
                var types = genericArgs.GetVairable('[', ']');
                foreach (var type in types)
                {
                    var typeName = type.Split(',')[0];
                    t.GenericArguments.Add(typeName);
                }
            }
            else t.TypeName = fullTypeName;
            return t;
        }

        /// <summary>
        /// 把类型定义转换成类型实体
        /// </summary>
        /// <returns></returns>
        public TypeModel ToTypeModel()
        {
            var type = TypeName.ToType();
            if (type == null) return null;
            TypeModel main = type.ToTypeModel();
            if (GenericArguments != null)
            {
                foreach (var g in GenericArguments)
                {
                    var t = g.ToType();
                    if (t == null) continue;
                    main.GenericArguments.Add(t.ToTypeModel());
                }
            }
            return main;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Common
{
    public class Reflection
    {
        public static object CallMethod(string DllFilePath, string NameSpace, string TypeName, string MethodName, object[] Args)
        {
            Assembly asmTemp = null;
            Type typTemp = null;
            object objInstance = null;
            MethodInfo miMethod = null;
            if (!string.IsNullOrEmpty(DllFilePath))
            {
                asmTemp = Assembly.LoadFile(DllFilePath);
            }
            if (asmTemp == null && !string.IsNullOrEmpty(NameSpace))
            {
                asmTemp = Assembly.LoadFrom(NameSpace);
            }
            if (asmTemp == null)
            {
                asmTemp = Assembly.GetCallingAssembly();
            }
            if (asmTemp == null)
            {
                throw new Exception("初始化失败,DLL文件路径或者命名空间错误");
            }
            objInstance = asmTemp.CreateInstance(TypeName);
            typTemp = objInstance.GetType();
            miMethod = typTemp.GetMethod(MethodName);
            if (miMethod == null)
            {
                throw new Exception("调用的方法名不存在");
            }
            object objReturn = miMethod.Invoke(objInstance, Args);
            return objReturn;
        }
    }
}

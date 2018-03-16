using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SOAFramework.Library
{
    public class Reflection
    {
        /// <summary>
        /// 通过反射调用方法
        /// </summary>
        /// <param name="DllFilePath">DLL文件路径</param>
        /// <param name="ClassName">类名称</param>
        /// <param name="ClassTypeName">类的类型名称,必须加上命名空间</param>
        /// <param name="MethodName">要调用的方法名</param>
        /// <param name="Args">参数</param>
        /// <returns></returns>
        public static object CallMethod(string MethodName = null, string DllFilePath = null, string AssemblyName = null, string ClassTypeName = null, object[] Args = null)
        {
            Assembly asmTemp = null;
            Type typTemp = null;
            object objInstance = null;
            MethodInfo miMethod = null;
            if (!string.IsNullOrEmpty(DllFilePath))
            {
                try
                {
                    asmTemp = Assembly.LoadFile(DllFilePath);
                }
                catch
                {
                }
            }
            if (asmTemp == null && !string.IsNullOrEmpty(AssemblyName))
            {
                try
                {
                    asmTemp = Assembly.Load(AssemblyName);
                }
                catch
                {
                }
            }
            if (asmTemp == null)
            {
                asmTemp = Assembly.GetEntryAssembly();
            }
            if (asmTemp == null)
            {
                throw new Exception("初始化失败,DLL文件路径或者命名空间错误");
            }
            objInstance = asmTemp.CreateInstance(ClassTypeName, true);
            if (objInstance == null)
            {
                return new Exception("类的类型名称不对，格式为{命名空间}.{类名}");
            }
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

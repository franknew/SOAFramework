using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MicroService.Library
{
    /// <summary>
    /// 定时器任务绑定
    /// </summary>
    public class TimingTasksBind
    {
        static TimingTasksBind()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly result = null;
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            result = ass.FirstOrDefault(t => t.Equals(args.RequestingAssembly));
            return result;
        }

        /// <summary>
        /// 绑定任务
        /// </summary>
        public static void BindTask()
        {
            TimingTasksHandler.ClearAllTask();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var ass in assemblies)
            {
                var types = ass.GetTypes();
                foreach (var type in types)
                {
                    TimingTasksClassAttribute timingTask = type.GetCustomAttributes(typeof(TimingTasksClassAttribute), true)?.FirstOrDefault() as TimingTasksClassAttribute;
                    if (timingTask != null)
                    {
                        BindTask(type);
                    }
                    
                }
            }
        }

        /// <summary>
        /// 绑定任务
        /// </summary>
        /// <param name="type"></param>
        private static void BindTask(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var m in methods)
            {
                TimingTasksAttribute attribute = m.GetCustomAttributes(typeof(TimingTasksAttribute), true)?.FirstOrDefault() as TimingTasksAttribute;
                if (attribute != null)
                {
                    Dictionary<string, Type> paras = new Dictionary<string, Type>();
                    attribute.TaskAddress = string.Format("{0} 【{1}.{2}】", type.Assembly.Location, type.FullName, m.Name);
                    var instance = Activator.CreateInstance(type);
                    var resType = m.ReturnType;
                    paras.Add("returns", resType);

                    if (string.IsNullOrEmpty(attribute.Name))
                    {
                        attribute.Name = string.Format("{0}.{1}", type.FullName, m.Name);
                    }

                    TimingTasksHandler.Register(attribute, Delegate.CreateDelegate(System.Linq.Expressions.Expression.GetDelegateType(paras.Values.ToArray()), instance, m));
                }
            }
        }
    }
}

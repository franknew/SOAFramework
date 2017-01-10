using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroService.Library
{
    /// <summary>
    /// 定时任务属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class TimingTasksAttribute : Attribute
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 时间间隔(毫秒)
        /// </summary>
        public int TimeInterval { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TimingTasksStateEnum State { get; set; }

        /// <summary>
        /// 任务地址
        /// </summary>
        public string TaskAddress { get; set; }
    }

    /// <summary>
    /// 定时任务类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TimingTasksClassAttribute : Attribute
    {

    }
}

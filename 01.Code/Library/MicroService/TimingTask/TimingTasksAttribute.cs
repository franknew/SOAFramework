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
        /// 任务状态改变委托
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        internal delegate void TaskStateChangeDelegate(object sender, TaskStateChangeEventArgs e);

        /// <summary>
        /// 任务状态改变事件
        /// </summary>
        internal event TaskStateChangeDelegate TaskStateChangeEvent;

        private string _guid;

        private TimingTasksStateEnum _state;

        public TimingTasksAttribute()
        {
            _guid = Guid.NewGuid().ToString();
        }

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
        public TimingTasksStateEnum State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value && TaskStateChangeEvent != null)
                {
                    TaskStateChangeEventArgs args = new TaskStateChangeEventArgs(_state, value);
                    _state = value;
                    TaskStateChangeEvent(this, args);
                }
                else
                {
                    _state = value;
                }
            }
        }

        /// <summary>
        /// 任务地址
        /// </summary>
        public string TaskAddress { get; set; }

        public override int GetHashCode()
        {
            return _guid.GetHashCode();//重写哈希,保证值一样
        }
    }

    /// <summary>
    /// 定时任务类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TimingTasksClassAttribute : Attribute
    {

    }

    /// <summary>
    /// 任务状态改变事件参数
    /// </summary>
    public class TaskStateChangeEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oldState">旧任务状态值</param>
        /// <param name="newState">新任务状态值</param>
        public TaskStateChangeEventArgs(TimingTasksStateEnum oldState, TimingTasksStateEnum newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        /// <summary>
        /// 旧任务状态
        /// </summary>
        public TimingTasksStateEnum OldState;

        /// <summary>
        /// 新任务状态
        /// </summary>
        public TimingTasksStateEnum NewState;
    }
}

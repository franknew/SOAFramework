using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroService.Library
{
    /// <summary>
    /// 定时任务信息
    /// </summary>
    public class TimingTaskInfo
    {
        private string _guid;

        private TimingTasksAttribute _task;

       
        /// <summary>
        /// 主键
        /// </summary>
        public string Key
        {
            get { return _guid; }
            set { _guid = value; }
        }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name
        {
            get { return _task.Name; }
        }

        /// <summary>
        /// 任务状态名称
        /// </summary>
        public string StateName
        {
            get { return GetEnumName(_task.State); }
        }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TimingTasksStateEnum State
        {
            get { return _task.State; }
        }

        /// <summary>
        /// 任务地址
        /// </summary>
        public string TaskAddress
        {
            get { return _task.TaskAddress; }
        }

        /// <summary>
        /// 时间间隔(毫秒)
        /// </summary>
        public int TimeInterval
        {
            get { return _task.TimeInterval; }
        }

        /// <summary>
        /// 任务
        /// </summary>
        public TimingTasksAttribute Task
        {
            get { return _task; }
            set
            {
                _task = value;
               
            }
        }

     

        /// <summary>
        /// 任务地址
        /// </summary>
        public string TaskUrl
        {
            get; set;
        }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime? StartExecuteTime { get; set; }

        /// <summary>
        /// 运行时间
        /// </summary>
        public string RunTime
        {
            get
            {
                if (StartExecuteTime == null)
                {
                    return "无";
                }
                TimeSpan timeSpan = DateTime.Now.Subtract((DateTime)StartExecuteTime);
                return string.Format("{0}天{1}小时{2}分{3}秒", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }
        }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int ExecuteCount { get; set; }

        /// <summary>
        /// 获取枚举名称
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string GetEnumName(TimingTasksStateEnum state)
        {
            switch (state)
            {
                case TimingTasksStateEnum.Inactivated:
                    return "待激活";
                case TimingTasksStateEnum.Stop:
                    return "停止";
                case TimingTasksStateEnum.Run:
                    return "运行中";
                case TimingTasksStateEnum.Sleep:
                    return "休眠";
                case TimingTasksStateEnum.Error:
                    return "异常";
                default: return "未知";
            }
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state"></param>
        public void SetState(TimingTasksStateEnum state)
        {
            _task.State = state;
        }
    }
}

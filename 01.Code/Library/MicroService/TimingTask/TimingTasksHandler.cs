using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroService.Library
{
    /// <summary>
    /// 定时任务处理
    /// </summary>
    public class TimingTasksHandler
    {
        private static SimpleLogger _logger = new SimpleLogger();

        /// <summary>
        /// 任务属性集合
        /// </summary>
        public static List<TimingTasksAttribute> Tasks
        {
            get
            {
                return _timingTasks.Keys.ToList();
            }
        }

        /// <summary>
        /// 任务信息集合
        /// </summary>
        public static List<TimingTaskInfo> TaskInfos
        {
            get
            {
                return _timingTaskInfos.Values.ToList();
            }
        }

        /// <summary>
        /// 定时任务集合
        /// </summary>
        private static Dictionary<TimingTasksAttribute, Delegate> _timingTasks = new Dictionary<TimingTasksAttribute, Delegate>();

        /// <summary>
        /// 定时任务信息
        /// </summary>
        private static Dictionary<TimingTasksAttribute, TimingTaskInfo> _timingTaskInfos = new Dictionary<TimingTasksAttribute, TimingTaskInfo>();

        /// <summary>
        /// 锁对象
        /// </summary>
        private static object obj = new object();

        /// <summary>
        /// 注册定时任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handle"></param>
        public static void Register(TimingTasksAttribute key, Delegate handle)
        {
            if (!_timingTasks.ContainsKey(key))
            {
                _timingTasks[key] = handle;
                _timingTaskInfos[key] = new TimingTaskInfo() { Task = key, Key = Guid.NewGuid().ToString() };
            }
        }

        /// <summary>
        /// 卸载定时任务
        /// </summary>
        /// <param name="key"></param>
        public static void UnRegister(TimingTasksAttribute key)
        {
            _timingTasks.Remove(key);
            _timingTaskInfos.Remove(key);
        }


        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="key"></param>
        public static void ExecuteTask(TimingTasksAttribute key)
        {
            if (_timingTasks.ContainsKey(key))
            {
                _timingTasks[key].DynamicInvoke(null);

                //记录任务运行信息
                if (_timingTaskInfos[key].StartExecuteTime == null)
                {
                    _timingTaskInfos[key].StartExecuteTime = DateTime.Now;
                }
                _timingTaskInfos[key].ExecuteCount += 1;
            }
        }

        /// <summary>
        /// 启动定时器(开始执行任务)
        /// </summary>
        public static void StartTiming()
        {
            Task.Factory.StartNew(new Action(() =>
            {
                while (true)
                {
                    lock (obj)
                    {
                        for (int i = 0; i < Tasks.Count; i++)
                        {
                            Task.Factory.StartNew(new Action<object>((t) =>
                            {

                                var task = (TimingTasksAttribute)t;
                                var currentState = task.State;
                                if (currentState != TimingTasksStateEnum.Inactivated)
                                {
                                    return;
                                }
                                Stopwatch stopwatch = new Stopwatch();
                                stopwatch.Start();

                                try
                                {
                                    task.State = TimingTasksStateEnum.Run;
                                    ExecuteTask(task);
                                    stopwatch.Stop();
                                }
                                catch (Exception ex)
                                {
                                    _logger.Write(string.Format("执行{0}任务发生异常:{1},详细信息:{2}", task?.Name, ex.Message, ex.StackTrace));
                                    //记录错误日志
                                    task.State = TimingTasksStateEnum.Error;
                                }
                                finally
                                {
                                    task.State = task.State == TimingTasksStateEnum.Run ? TimingTasksStateEnum.Sleep : task.State;
                                }

                                if (task.TimeInterval > (int)stopwatch.Elapsed.TotalMilliseconds)
                                {
                                    Thread.Sleep(task.TimeInterval - Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds));
                                }

                                task.State = task.State == TimingTasksStateEnum.Sleep ? TimingTasksStateEnum.Inactivated : task.State;

                            }), Tasks[i]);
                        }
                    }
                    Thread.Sleep(50);
                }
            }));
        }

        /// <summary>
        /// 停止指定的任务
        /// </summary>
        /// <param name="key">主键</param>
        public static void StopTask(string key)
        {
            TaskInfos.ForEach(t =>
            {
                if (t.Key == key)
                {
                    t.Task.State = TimingTasksStateEnum.Stop;
                    t.StartExecuteTime = null;
                    return;
                }
            });
        }

        /// <summary>
        /// 停止所有任务
        /// </summary>
        public static void StopAllTask()
        {
            TaskInfos.ForEach(t =>
            {
                t.Task.State = TimingTasksStateEnum.Stop;
                t.StartExecuteTime = null;
            });
        }

        /// <summary>
        /// 启动所有任务
        /// </summary>
        /// <param name="key"></param>
        public static void StartTask(string key)
        {
            TaskInfos.ForEach(t =>
            {
                if (t.Key == key)
                {
                    t.Task.State = TimingTasksStateEnum.Inactivated;
                    t.StartExecuteTime = DateTime.Now;
                    return;
                }
            });
        }

        /// <summary>
        /// 启动所有任务
        /// </summary>
        public static void StartAllTask()
        {
            TaskInfos.ForEach(t =>
            {
                t.Task.State = TimingTasksStateEnum.Inactivated;
                t.StartExecuteTime = DateTime.Now;
            });
        }

        /// <summary>
        /// 清除所有任务
        /// </summary>
        internal static void ClearAllTask()
        {
            _timingTasks.Clear();
            _timingTaskInfos.Clear();
        }
    }
}

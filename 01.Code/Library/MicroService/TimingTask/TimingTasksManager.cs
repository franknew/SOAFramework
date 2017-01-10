using AustinHarris.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroService.Library
{
    /// <summary>
    /// 定时任务管理
    /// </summary>
    [JsonRpcClass]
    public class TimingTasksManager
    {

        /// <summary>
        /// 启动所有任务
        /// </summary>
        [JsonRpcMethod]
        public void StartAllTask()
        {
            TimingTasksHandler.StartAllTask();
        }

        /// <summary>
        /// 启动指定的任务
        /// </summary>
        /// <param name="taskKey">主键</param>
        [JsonRpcMethod]
        public void StartTask(string taskKey)
        {
            TimingTasksHandler.StartTask(taskKey);
        }

        /// <summary>
        /// 停止所有任务
        /// </summary>
        [JsonRpcMethod]
        public void StopAllTask()
        {
            TimingTasksHandler.StopAllTask();
        }

        /// <summary>
        /// 停止指定的任务
        /// </summary>
        /// <param name="taskKey">主键</param>
        [JsonRpcMethod]
        public void StopTask(string taskKey)
        {
            TimingTasksHandler.StopTask(taskKey);
        }

        /// <summary>
        /// 启动定时器
        /// </summary>
        [JsonRpcMethod]
        public void StartTiming()
        {
            TimingTasksHandler.ClearAllTask();
            TimingTasksHandler.StartTiming();
        }

        /// <summary>
        /// 查询定时器任务信息
        /// </summary>
        /// <returns></returns>
        [JsonRpcMethod]
        public List<TimingTaskInfo> Query()
        {
            return TimingTasksHandler.TaskInfos;
        }
    }
}

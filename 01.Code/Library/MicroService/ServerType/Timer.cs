using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AustinHarris.JsonRpc;

namespace MicroService.Library
{
    public class Timer : IServer
    {
        public void Bind(string sessionName, AppDomain domain)
        {
            ServiceBinder.BindService(sessionName, AppDomain.CurrentDomain);
            TimingTasksBind.BindTask(domain);//绑定任务
            TimingTasksBind.GetTimerManager().StartTiming();//启动任务监听
        }
    }
}

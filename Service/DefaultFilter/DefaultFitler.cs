﻿using SOAFramework.Library;
using SOAFramework.Service.Core;
using SOAFramework.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service
{
    public class DefaultFitler : BaseFilter
    {
        public override bool OnActionExecuting(ActionContext context)
        {
            MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "请求方法：" + context.Router.TypeName + "." + context.Router.Action });
            return true;
        }
    }
}
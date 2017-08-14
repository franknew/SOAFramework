using com.alibaba.rocketmq.client.consumer.listener;
using java.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public delegate ConsumeConcurrentlyStatus ConsumeDelegate(List l, ConsumeConcurrentlyContext ccc);
}

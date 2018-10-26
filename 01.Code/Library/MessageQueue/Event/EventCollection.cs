using java.util;
using org.apache.rocketmq.client.consumer.listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public delegate ConsumeConcurrentlyStatus ConsumeDelegate(List l, ConsumeConcurrentlyContext ccc);
}

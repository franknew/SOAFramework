using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using java.util;
using org.apache.rocketmq.client.consumer.listener;

namespace SOAFramework.Library
{
    public class RocketMessageConsumeListener : MessageListenerConcurrently
    {
        public ConsumeDelegate OnConsume { get; set; }

        public ConsumeConcurrentlyStatus consumeMessage(List l, ConsumeConcurrentlyContext ccc)
        {
            if (OnConsume != null) return OnConsume.Invoke(l, ccc);
            else throw new Exception("没有注册事件OnConsume！");
        }
    }
}

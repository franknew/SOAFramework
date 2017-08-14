
using com.alibaba.rocketmq.client.consumer.listener;
using com.alibaba.rocketmq.common.message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WinformTest
{
    public class TestListener : MessageListenerConcurrently
    {
        public ConsumeConcurrentlyStatus consumeMessage(List list, ConsumeConcurrentlyContext ccc)
        {
            for (int i = 0; i < list.size(); i++)
            {
                var msg = list.get(i) as Message;
                byte[] body = msg.getBody();
                var str = Encoding.UTF8.GetString(body);
                if (body.Length == 2 && body[0] == 0 && body[1] == 0)
                {
                    //Info: 鐢熶骇鑰呭仠姝㈢敓鎴愭暟鎹�, 骞朵笉鎰忓懗鐫�椹笂缁撴潫
                    //System.out.println("Got the end signal");
                    continue;
                }

                //PaymentMessage paymentMessage = RaceUtils.readKryoObject(PaymentMessage.class, body);
                //    System.out.println(paymentMessage);
            }
            return ConsumeConcurrentlyStatus.CONSUME_SUCCESS;
        }
    }
}
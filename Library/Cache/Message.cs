using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class CacheMessage
    {
        public string Message { get; set; }

        private DateTime timeStamp = DateTime.Now;

        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public string StackTrace { get; set; }

        private enumMessageType messageType = enumMessageType.Info;

        public enumMessageType MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }
    }

    public enum enumMessageType
    {
        Info,
        Error
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadTesting
{
    public class TestingData
    {
        public string Url { get; set; }

        public int Count { get; set; }

        public string Method { get; set; }

        public long Time { get; set; }

        public long MaxRequestTime { get; set; }

        public string PostData { get; set; }

        public string ContentType { get; set; }

        public int AverageCount
        {
            get
            {
                return (int)(Count * 1000 / Time);
            }
        }

        public string Result { get; set; }
    }
}

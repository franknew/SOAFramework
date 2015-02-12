using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.ElectricWeight
{
    public class EleScaleParams
    {
        public int 端口号 { get; set; }
        public int 波特率 { get; set; }
        public string 请求指令 { get; set; }
        public int 数据格式 { get; set; }
        public int 数据位 { get; set; }
        public string 停止位 { get; set; }
        public int 校验位 { get; set; }
    }
}

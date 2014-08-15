using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Athena.Unitop.Sure.Lib.ElectricWeight
{
    public class GroundWeight
    {
        public int 端口号 { get; set; }
        public int 波特率 { get; set; }
        public int 校验位 { get; set; }
        public int 数据位 { get; set; }
        public int 停止位 { get; set; }
        public string 流控制 { get; set; }
        public bool 十六进制显示 { get; set; }
        public decimal 重量 { get; set; }
        public string 十六进制文本 { get; set; }
    }
}

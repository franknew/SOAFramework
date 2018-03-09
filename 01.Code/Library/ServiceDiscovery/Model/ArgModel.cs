using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class ArgModel : TypeModel
    {
        public ArgModel(): base()
        {

        }

        /// <summary>
        /// 参数索引
        /// </summary>
        public int Index { get; set; }
    }
}

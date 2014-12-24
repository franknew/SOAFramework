using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Model
{
    public class PagingServerResponse : ServerResponse
    {
        /// <summary>
        /// 总数据量
        /// </summary>
        public int TotalRecordCount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.WeiXin
{
    public class User_SimpleListQueryString
    {
        public string department_id { get; set; }
        /// <summary>
        /// 1/0：是否递归获取子部门下面的成员
        /// </summary>
        public int fetch_child { get; set; }
        /// <summary>
        /// 0获取全部成员，1获取已关注成员列表，2获取禁用成员列表，4获取未关注成员列表。status可叠加，未填写则默认为4
        /// </summary>
        public int status { get; set; }
    }
}

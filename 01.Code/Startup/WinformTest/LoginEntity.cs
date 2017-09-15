using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SSO
{
    /// <summary>
    /// 登录实体
    /// </summary>
    public class LoginEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime time { get; set; }
    }
}

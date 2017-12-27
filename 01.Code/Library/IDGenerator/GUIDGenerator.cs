using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    /// <summary>
    /// guid，ms自带guid系统产生
    /// </summary>
    public class GUIDGenerator : IIDGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}

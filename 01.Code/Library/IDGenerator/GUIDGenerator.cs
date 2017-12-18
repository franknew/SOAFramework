using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class GUIDGenerator : IIDGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}

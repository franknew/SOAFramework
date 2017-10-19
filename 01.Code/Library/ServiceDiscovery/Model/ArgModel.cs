using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class ArgModel : TypeModel
    {
        public ArgModel(Type self) : base(self)
        {
        }

        public int Index { get; set; }
    }
}

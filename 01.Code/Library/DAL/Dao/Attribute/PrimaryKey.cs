using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey: Attribute
    {
    }
}

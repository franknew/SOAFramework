using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public interface ILogger
    {
        void Write(string fileName, string log);
        void WriteException(string fileName, Exception ex);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroService.Library
{
    [CategoryInfo("Kernel.SimpleLibrary.PlugPut", "")]
    public interface IObjcet
    {
        void Put();

        string Put(string plus);
    }
}

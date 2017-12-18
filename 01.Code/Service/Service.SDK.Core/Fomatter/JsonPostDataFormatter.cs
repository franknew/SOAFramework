using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.SDK.Core
{
    public class JsonPostDataFormatter : IPostDataFormatter
    {
        public string Format(IDictionary<string, object> o)
        {
            return JsonHelper.Serialize(o);
        }
    }
}

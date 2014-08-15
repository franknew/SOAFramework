using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Interface
{
    public abstract class IRequest<T> where T : IResponse
    {
        public abstract string GetApiName();
    }
}

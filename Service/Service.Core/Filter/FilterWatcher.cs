using SOAFramework.Service.Interface;
using SOAFramework.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core.Filter
{
    [ServiceLayer(Enabled = false)]
    public class FilterWatcher
    {
        private List<IFilter> _list = new List<IFilter>();

        public void AddFilter(IFilter filter)
        {
            _list.Add(filter);
        }

        public void RemoveFilter(IFilter filter)
        {
            _list.Remove(filter);
        }

        public bool OnActionExecuting(ActionContext context)
        {
            foreach (IFilter filter in _list)
            {
                if (!filter.OnActionExecuting(context))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

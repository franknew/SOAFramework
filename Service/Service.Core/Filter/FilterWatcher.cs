using SOAFramework.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core.Filter
{
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

        public bool ExecuteFilter(Dictionary<string, object> args)
        {
            foreach (IFilter filter in _list)
            {
                if (!filter.Execute(args))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

using SOAFramework.Service.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core
{
    public interface IAnalyzer
    {
        void AnalyzeService(IDictionary<string, Model.ServiceModel> inputDic, List<IFilter> filterList);

        List<IFilter> AnalyzeFilter();

    }
}

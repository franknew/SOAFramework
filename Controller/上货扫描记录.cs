using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Controller
{
    public class ScanRecordController
    {
        public List<上货扫描记录> Query()
        {
            List<上货扫描记录> list = new List<上货扫描记录>();
            list = 上货扫描记录.CreateHelper().GetModelList();
            return list;
        }
    }
}

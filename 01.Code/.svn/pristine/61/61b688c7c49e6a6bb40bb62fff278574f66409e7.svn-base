using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using WebServiceCore.Models;

namespace WebServiceCore
{
    public class SubmitModel
    {
        public ComputerInfo ComputerInfo = new ComputerInfo();
        public string Action = null;
        public string Controller = null;
        public string ServiceUrl = null;
        public string AuthKey = null;
        public Args[] Args = null;
        public int CurrentPage = -1;
    }

    public class ReturnModel
    {
        public int TotalRecords = -1;
        public int TotalPages = -1;
        public int CurrentPage = -1;
        public DataTable Data = null;
        public int Status = -1;
        public string Message = null;
        public int IsAuthValid = -1;
        public int ErrorNumber = -1;
    }

}

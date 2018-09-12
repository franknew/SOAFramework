using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class FileUploader
    {
        const string boundery = "--WEBKITFORMBOUNDARY!@#$%^&*()";

        public bool Upload(IDictionary<string, string> fileNames)
        {
            if (null == fileNames || fileNames.Keys.Count == 0) { throw new Exception("没有fileName参数"); }
            StringBuilder post = new StringBuilder();
            post.Append(boundery);
            post.Append(boundery);
            return true;
        }
    }
}

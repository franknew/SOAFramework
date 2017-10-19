using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.Refile
{
    public abstract class Element
    {
        public string id { get; set; }
        public string name { get; set; }
        public string Class { get;set;}
        public string style { get; set; }
        public List<Element> Children { get; set; }
}
}

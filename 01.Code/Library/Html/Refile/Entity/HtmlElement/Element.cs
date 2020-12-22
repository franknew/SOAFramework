using SOAFramework.Library.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.Refile
{
    public abstract class Element
    {
        private string id = "";
        private string _class = "";
        public string ID { get => id; set => id = value; }
        public string name { get; set; }
        public string Class { get => _class; set => _class = value; }
        public string style { get; set; }

        public string text { get; set; }
        public List<Element> Children { get; set; }
        public HtmlNode Node { get; set; }
    }
}

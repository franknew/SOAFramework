namespace SOAFramework.Library.Html
{
    using SOAFramework.Library.Refile;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    [DebuggerDisplay("name:{Name}, value:{Value}")]
    public abstract class HtmlNode
    {
        private readonly IList<HtmlNode> attributes = new List<HtmlNode>();
        private readonly IList<HtmlNode> childNodes = new List<HtmlNode>();

        protected internal HtmlNode(HtmlNodeType nodeType, string name, string value = null)
        {
            NodeType = nodeType;
            Name = name;
            Value = value;
        }

        public HtmlNodeType NodeType { get; private set; }

        public string Name { get; private set; }

        public string Value { get; set; }

        public IList<HtmlNode> ChildNodes
        {
            get { return childNodes; }
        }

        public IList<HtmlNode> Attributes
        {
            get { return attributes; }
        }

        public HtmlNode FirstChild { get; private set; }

        public HtmlNode LastChild { get; private set; }

        public HtmlNode NextSibling { get; private set; }

        public HtmlNode PreviousSibling { get; private set; }

        public HtmlNode ParentNode { get; private set; }

        public void AppendChild(HtmlNode node)
        {
            if (FirstChild == null)
                FirstChild = node;

			node.PreviousSibling = LastChild;

            if (LastChild != null)
                LastChild.NextSibling = node;

			LastChild = node;
            
            node.ParentNode = this;
			
            childNodes.Add(node);
        }

        public void AppendAttribute(HtmlNode attribute)
        {
            attributes.Add(attribute);
        }

        public List<T> GetTags<T>() where T: Element
        {
            List<T> list = new List<T>();
            GetTags_Resc<T>(this, list);
            return list;
        }

        private void GetTags_Resc<T>(HtmlNode node, List<T> list) where T: Element
        {
            Element tag = null;
            if (typeof(T).Name.ToLower().Equals(node.Name))
            {
                switch (node.Name)
                {
                    case "img":
                        tag = new Img();
                        (tag as Img).src = node.Attributes.FirstOrDefault(t => t.Name.Equals("src"))?.Value;
                        break;
                    case "a":
                        tag = new A();
                        (tag as A).Href = node.Attributes.FirstOrDefault(t => t.Name.Equals("href"))?.Value;
                        (tag as A).Title = node.Attributes.FirstOrDefault(t => t.Name.Equals("title"))?.Value;
                        break;
                    case "div":
                        tag = new Div();
                        break;
                    case "section":
                        tag = new Section();
                        break;
                    case "strong":
                        tag = new Strong();
                        break;
                    case "span":
                        tag = new Span();
                        break;
                    case "script":
                        tag = new Script();
                        (tag as Script).Type = node.Attributes.FirstOrDefault(t => t.Name.Equals("type"))?.Value;
                        (tag as Script).Code = node.childNodes.Count > 0 ? node.childNodes[0].Value : null;
                        (tag as Script).Src = node.Attributes.FirstOrDefault(t => t.Name.Equals("Src"))?.Value;
                        break;
                    case "table":
                        tag = new Table();
                        break;
                    case "tr":
                        tag = new Tr();
                        break;
                    case "td":
                        tag = new Td();
                        break;
                }
            }
            if (tag != null)
            {
                tag.name = node.Name;
                tag.Class = node.attributes.FirstOrDefault(p => p.Name.Equals("class")) == null ? "": node.attributes.FirstOrDefault(p => p.Name.Equals("class")).Value;
                tag.ID = node.attributes.FirstOrDefault(p => p.Name.Equals("id")) == null ? "" : node.attributes.FirstOrDefault(p => p.Name.Equals("id")).Value;
                tag.Node = node;
                var t = (T)tag;
                list.Add(t);
            }
            if (node.childNodes != null)
            {
                foreach (var n in node.childNodes)
                {
                    GetTags_Resc(n, list);
                }
            }
        }
    }
}
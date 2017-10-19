namespace SOAFramework.Library.Html
{
    public class HtmlElementNode : HtmlNode
    {
        internal HtmlElementNode(string name) 
            : base(HtmlNodeType.Element, name)
        {
        }
    }
}
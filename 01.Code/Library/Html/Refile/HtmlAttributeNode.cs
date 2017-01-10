namespace SOAFramework.Library.Html
{
    public class HtmlAttributeNode : HtmlNode
    {
        internal HtmlAttributeNode(string name) 
            : base(HtmlNodeType.Attribute, name)
        {
        }
    }
}
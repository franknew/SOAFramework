namespace SOAFramework.Library.Html
{
    public class HtmlTextNode : HtmlNode
    {
        internal HtmlTextNode(string value) 
            : base(HtmlNodeType.Text, "#text", value)
        {
        }
    }
}
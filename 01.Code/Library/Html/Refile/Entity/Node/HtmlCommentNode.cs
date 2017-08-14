namespace SOAFramework.Library.Html
{
    public class HtmlCommentNode : HtmlNode
    {
        internal HtmlCommentNode(string value) 
            : base(HtmlNodeType.Comment, "#comment", value)
        {
        }
    }
}
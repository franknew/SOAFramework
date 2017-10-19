namespace SOAFramework.Library.Html
{
    public class HtmlDocumentNode : HtmlNode
    {
        internal HtmlDocumentNode() 
            : base(HtmlNodeType.Document, "#document")
        {
        }
    }
}
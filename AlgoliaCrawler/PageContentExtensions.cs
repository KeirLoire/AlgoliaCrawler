using Abot2.Poco;
using HtmlAgilityPack;

namespace AlgoliaCrawler
{
    public static class PageContentExtensions
    {
        public static string GetContentByXpath(this PageContent content, string xpath)
        {
            if (string.IsNullOrEmpty(content.Text))
                return string.Empty;

            var doc = new HtmlDocument();
            doc.LoadHtml(content.Text);

            var node = doc.DocumentNode.SelectSingleNode(xpath);

            if (node != null && node.Name == "meta" && node.Attributes["content"] != null)
                return node.Attributes["content"].Value;

            return node?.InnerText ?? string.Empty;
        }
    }
}

using System.Xml.Linq;

namespace AlgoliaCrawler
{
    public sealed class SitemapParser
    {
        public async Task<List<string>> GetSitemapUrlsAsync(string baseUrl)
        {
            var url = baseUrl.EndsWith(".xml") ? new Uri(baseUrl) : new Uri(new Uri(baseUrl), "sitemap.xml");
            var sitemapUrls = new List<string>();

            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(url);
                var doc = XDocument.Parse(content);
                var ns = (XNamespace)"http://www.sitemaps.org/schemas/sitemap/0.9";

                foreach (var element in doc.Descendants())
                {
                    if (element.Name == ns + "sitemap")
                    {
                        var locElement = element.Element(ns + "loc");

                        if (locElement != null)
                        {
                            var nestedSitemapUrls = await GetSitemapUrlsAsync(locElement.Value);
                            sitemapUrls.AddRange(nestedSitemapUrls);
                        }
                    }
                    else if (element.Name == ns + "url")
                    {
                        var locElement = element.Element(ns + "loc");

                        if (locElement != null)
                            sitemapUrls.Add(locElement.Value);
                    }
                }
            }

            return sitemapUrls;
        }
    }
}

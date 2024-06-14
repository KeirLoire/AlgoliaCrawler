using Abot2.Crawler;
using AbotX2.Parallel;
using AbotX2.Poco;
using AlgoliaCrawler.Model.Configs;
using AlgoliaCrawler.Models;
using System.Collections.Concurrent;
using System.Net;

namespace AlgoliaCrawler
{
    public sealed class Crawler
    {
        private readonly AlgoliaConfiguration _algoliaConfiguration;
        private readonly Uploader _uploader;
        private readonly ConcurrentDictionary<string, List<PageIndex>> _pageIndexes = new();

        public Crawler(AlgoliaConfiguration algoliaConfiguration)
        {
            _algoliaConfiguration = algoliaConfiguration;
            _uploader = new Uploader();
        }

        public async Task StartAsync()
        {
            // Add sites to crawl
            var sitesToCrawl = new List<SiteToCrawl>();

            foreach (var application in _algoliaConfiguration.Applications)
            {
                if (!application.Enabled)
                    continue;

                var siteToCrawl = new SiteToCrawl
                {
                    Uri = new Uri(application.Url)
                };

                sitesToCrawl.Add(siteToCrawl);
            }

            var siteToCrawlProvider = new SiteToCrawlProvider();
            siteToCrawlProvider.AddSitesToCrawl(sitesToCrawl);

            // Configuring the crawler
            var crawlConfigurationX = new CrawlConfigurationX
            {
                UserAgentString = _algoliaConfiguration.UserAgentString,
                RobotsDotTextUserAgentString = _algoliaConfiguration.UserAgentString,
                MinCrawlDelayPerDomainMilliSeconds = _algoliaConfiguration.MinCrawlDelayPerDomainMilliSeconds,
                MaxPagesToCrawl = _algoliaConfiguration.MaxPagesToCrawl,
                MaxConcurrentSiteCrawls = _algoliaConfiguration.MaxConcurrentSiteCrawls,
                MaxRetryCount = _algoliaConfiguration.MaxRetryCount
            };

            var crawler = new ParallelCrawlerEngine(crawlConfigurationX,
                new ParallelImplementationOverride(crawlConfigurationX,
                    new ParallelImplementationContainer()
                    {
                        SiteToCrawlProvider = siteToCrawlProvider,
                        WebCrawlerFactory = new WebCrawlerFactory(crawlConfigurationX)
                    })
            );
            crawler.CrawlerInstanceCreated += CrawlerInstanceCreated;
            crawler.SiteCrawlCompleted += SiteCrawlCompleted;
            crawler.AllCrawlsCompleted += AllCrawlsCompleted;
            
            await crawler.StartAsync();
        }

        private async void CrawlerInstanceCreated(object sender, CrawlerInstanceCreatedArgs e)
        {
            e.Crawler.PageCrawlCompleted += PageCrawlCompleted;

            Console.WriteLine($"Starting crawl operation for {e.SiteToCrawl.Uri}");
        }

        private async void SiteCrawlCompleted(object sender, SiteCrawlCompletedArgs e)
        {
            var url = e.CrawledSite.SiteToCrawl.Uri.ToString();

            Console.WriteLine($"Finished crawl operation for {e.CrawledSite.SiteToCrawl.Uri}");

            var application = _algoliaConfiguration.Applications.Where(x => x.Url.Equals(url)).FirstOrDefault();
            var pageIndexes = _pageIndexes[application.Id];

            await _uploader.UploadAsync(application, pageIndexes);
        }

        private async void AllCrawlsCompleted(object sender, AllCrawlsCompletedArgs e)
        {
            Console.WriteLine("All crawl operations completed");
        }

        private async void PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            var pageIndex = new PageIndex
            {
                Title = WebUtility.HtmlDecode(e.CrawledPage.Content.GetContentByXpath("//title")),
                Url = e.CrawledPage.Uri.AbsoluteUri
            };

            if (e.CrawledPage.HttpRequestException != null)
            {
                if (e.CrawledPage.HttpResponseMessage != null)
                {
                    var statusCode = (int)e.CrawledPage.HttpResponseMessage.StatusCode;

                    Console.WriteLine($"[{statusCode}] {pageIndex.Url}");
                }
                else
                    Console.WriteLine($"[TIMED OUT] {pageIndex.Url}");
            }
            else
            {
                var rootUrl = e.CrawlContext.OriginalRootUri.ToString();
                var application = _algoliaConfiguration.Applications.Where(x => x.Url.Equals(rootUrl)).FirstOrDefault();
                var pageIndexes = _pageIndexes.GetOrAdd(application.Id, new List<PageIndex>());

                pageIndexes.Add(pageIndex);
            }
        }
    }
}

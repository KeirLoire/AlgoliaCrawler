using AbotX2.Parallel;
using AbotX2.Poco;
using AlgoliaCrawler.Model.Configs;

namespace AlgoliaCrawler
{
    public sealed class Crawler
    {
        private readonly AlgoliaConfiguration _algoliaConfiguration;

        public Crawler(AlgoliaConfiguration algoliaConfiguration)
        {
            _algoliaConfiguration = algoliaConfiguration;
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
            
            await crawler.StartAsync();
        }
    }
}

using AlgoliaCrawler.Models.Configs;

namespace AlgoliaCrawler.Model.Configs
{
    public sealed class AlgoliaConfiguration
    {
        public string UserAgentString { get; set; }
        public int MinCrawlDelayPerDomainMilliSeconds { get; set; }
        public int MaxPagesToCrawl { get; set; }
        public int MaxConcurrentSiteCrawls { get; set; }
        public int MaxRetryCount { get; set; }
        public List<ApplicationConfiguration> Applications { get; set; }
        public ApplicationConfiguration this[string id] => Applications.FirstOrDefault(a => a.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    }
}

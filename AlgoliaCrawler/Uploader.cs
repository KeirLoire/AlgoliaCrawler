using Algolia.Search.Clients;
using AlgoliaCrawler.Models;
using AlgoliaCrawler.Models.Configs;
using Microsoft.Extensions.Logging;

namespace AlgoliaCrawler
{
    public sealed class Uploader
    {
        private readonly ILogger<Uploader> _logger;

        public Uploader(ILogger<Uploader> logger)
        {
            _logger = logger;
        }

        public async Task UploadAsync(ApplicationConfiguration applicationConfiguration, IEnumerable<PageIndex> pageIndexes)
        {
            var client = new SearchClient(applicationConfiguration.Id, applicationConfiguration.WriteApiKey);
            var index = client.InitIndex(applicationConfiguration.Index);

            await index.ClearObjectsAsync();
            await index.SaveObjectAsync(pageIndexes);

            _logger.LogInformation($"Uploaded {pageIndexes.Count()} pages for {applicationConfiguration.Url}");
        }
    }
}

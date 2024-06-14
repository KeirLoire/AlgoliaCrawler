using Algolia.Search.Clients;
using AlgoliaCrawler.Models;
using AlgoliaCrawler.Models.Configs;

namespace AlgoliaCrawler
{
    public sealed class Uploader
    {
        public async Task UploadAsync(ApplicationConfiguration applicationConfiguration, IEnumerable<PageIndex> pageIndexes)
        {
            var client = new SearchClient(applicationConfiguration.Id, applicationConfiguration.WriteApiKey);
            var index = client.InitIndex(applicationConfiguration.Index);

            await index.ClearObjectsAsync();
            await index.SaveObjectAsync(pageIndexes);

            Console.WriteLine($"Uploaded {pageIndexes.Count()} pages for {applicationConfiguration.Url}");
        }
    }
}

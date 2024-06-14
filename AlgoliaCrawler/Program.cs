using AlgoliaCrawler;
using AlgoliaCrawler.Model.Configs;
using Microsoft.Extensions.Configuration;

// Retrieve Configuration values
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

IConfiguration configuration = builder.Build();

var algoliaConfiguration = new AlgoliaConfiguration();
configuration.GetSection("Algolia").Bind(algoliaConfiguration);

// Start the Crawler
var crawler = new Crawler(algoliaConfiguration);
await crawler.StartAsync();
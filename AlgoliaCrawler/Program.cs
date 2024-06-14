using AlgoliaCrawler;
using AlgoliaCrawler.Model.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

// Retrieve Configuration values
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

IConfiguration configuration = builder.Build();

var algoliaConfiguration = new AlgoliaConfiguration();
configuration.GetSection("Algolia").Bind(algoliaConfiguration);

// Configure Logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

// Start the Crawler
try
{
    var serviceProvider = new ServiceCollection()
        .AddSingleton(algoliaConfiguration)
        .AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        })
        .AddTransient<Crawler>()
        .AddTransient<SitemapParser>()
        .AddTransient<Uploader>()
        .BuildServiceProvider();

    var crawler = serviceProvider.GetService<Crawler>();
    await crawler.StartAsync();
}
catch (Exception e)
{
    Log.Error(e.Message);
}
finally
{
    Log.CloseAndFlush();
}
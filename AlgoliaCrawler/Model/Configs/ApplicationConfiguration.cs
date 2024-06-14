namespace AlgoliaCrawler.Models.Configs
{
    public sealed class ApplicationConfiguration
    {
        public string Id { get; set; }
        public string WriteApiKey { get; set; }
        public string Index { get; set; }
        public string Url { get; set; }
        public bool Enabled { get; set; }
    }
}

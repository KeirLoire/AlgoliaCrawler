using System.Net;

namespace AlgoliaCrawler
{
    public static class HttpClientExtensions
    {
        public static async Task<string> GetStringIgnoreExceptionAsync(this HttpClient client, string url)
        {
            try
            {
                var response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

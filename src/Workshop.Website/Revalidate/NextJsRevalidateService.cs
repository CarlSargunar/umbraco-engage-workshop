using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Workshop.Website.Revalidate
{
    public class NextJsRevalidateService
    {
        private readonly NextJsRevalidateOptions _config;
        private readonly ILogger<NextJsRevalidateService> _logger;

        public NextJsRevalidateService(
            IOptions<NextJsRevalidateOptions> options, ILogger<NextJsRevalidateService> logger)
        {
            _config = options.Value;
            _logger = logger;
        }

        public async Task ForPortal()
        {
            await Send(new
            {
                updatePortal = true
            });
        }

        public async Task ForNavigation()
        {
            await Send(new
            {
                updateNavigation = true
            });
        }

        public async Task ForContent(string path)
        {
            await Send(new
            {
                contentPath = path
            });
        }

        public async Task Send(object payload)
        {
            using (var client = new HttpClient())
            {
                var jsonString = System.Text.Json.JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // Create the HMAC signature
                string signature = CreateSignature(jsonString, _config.WebHookSecret);
                client.DefaultRequestHeaders.Add("x-hub-signature-256", signature);

                if (_config.WebHookUrls != null)
                {
                    var urls = JsonSerializer.Deserialize<string[]>(_config.WebHookUrls);
                    if (urls != null)
                    {
                        foreach (var url in urls)
                        {
                            await SendMessage(client, url, content);
                        }     
                    }
                }
            }
        }

        private async Task SendMessage(HttpClient client, string url, StringContent content)
        {
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Successfully sent NextJs revalidate message to {url}");
            }
            else
            {
                _logger.LogError($"Failed to send NextJs revalidate to {url}. Status code: {response.StatusCode} Status message");
            }
        }

        private string CreateSignature(string payload, string secret)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return "sha256=" + BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}

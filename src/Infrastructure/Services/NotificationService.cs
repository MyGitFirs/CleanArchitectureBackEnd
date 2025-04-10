using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _oneSignalApiKey;
        private readonly string _oneSignalAppId;
        private readonly ILogger<NotificationService> _logger;

        
        public NotificationService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<NotificationService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _oneSignalApiKey = configuration["OneSignal:ApiKey"];
            _oneSignalAppId = configuration["OneSignal:AppId"];
            _logger = logger;
        }

        public async Task SendNotificationAsync(Guid userId, string message, CancellationToken cancellationToken)
        {
            var payload = new
            {
                app_id = _oneSignalAppId,
                contents = new { en = message },
                included_segments = new[] { "Subscribed Users" },//include_external_user_ids = new[] { userId.ToString() },
                channel_for_external_user_ids = "push"
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://onesignal.com/api/v1/notifications")
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Authorization", $"Basic {_oneSignalApiKey}");

            try
            {
                var response = await _httpClient.SendAsync(request, cancellationToken);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("OneSignal error {StatusCode}: {Body}", response.StatusCode, responseBody);
                }
                else
                {
                    _logger.LogInformation("OneSignal notification sent. Response: {Body}", responseBody);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception sending OneSignal notification: {Error}", ex.Message);
            }

        }
    }
}

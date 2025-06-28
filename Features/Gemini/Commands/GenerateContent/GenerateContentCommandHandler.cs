using Microsoft.Extensions.Options;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Settings;
using System.Text.Json;
using System.Text;
using Alwalid.Cms.Api.Features.Gemini.Dtos;

namespace Alwalid.Cms.Api.Features.Gemini.Commands.GenerateContent
{
    public class GenerateContentCommandHandler : ICommandHandler<GenerateContentCommand, string>
    {
        private readonly HttpClient _httpClient;
        private readonly GeminiSettings _geminiSettings;
        private const string Endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        public GenerateContentCommandHandler(HttpClient httpClient, IOptions<GeminiSettings> geminiSettings)
        {
            _httpClient = httpClient;
            _geminiSettings = geminiSettings.Value;
        }

        public async Task<Result<string>> Handle(GenerateContentCommand command, CancellationToken cancellationToken)
        {
            var requestPayload = BuildRequest(command.Prompt);
            var response = await SendRequestAsync(requestPayload);
            var content = await ExtractResponseContentAsync(response);
            var result = ExtractTextFromResponse(content);
            return await Result<string>.SuccessAsync(result, "Get the Ai result", true);
        }

        private static GeminiRequestDto BuildRequest(string prompt) => new()
        {
            contents = new List<Content>
        {
            new Content
            {
                parts = new List<Part> { new Part { text = prompt } }
            }
        }
        };

        private async Task<HttpResponseMessage> SendRequestAsync(GeminiRequestDto request)
        {
            var json = JsonSerializer.Serialize(request);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{Endpoint}?key={_geminiSettings.ApiKey}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Gemini API failed: {response.StatusCode} - {errorMsg}");
            }

            return response;
        }

        private async Task<GeminiResponseDto?> ExtractResponseContentAsync(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<GeminiResponseDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private static string ExtractTextFromResponse(GeminiResponseDto? response)
        {
            return response?.candidates?.FirstOrDefault()?.content?.parts?.FirstOrDefault()?.text
                   ?? "No response from Gemini.";
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using Api.Models;
using Newtonsoft.Json;

namespace Api.Controllers;

[ApiController]
[Route("api/{websiteId}")]
public class UmamiController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public UmamiController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // GET: api/5/stats
    [HttpGet("stats")]
    [ProducesResponseType<WebsiteStats>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStats(string websiteId, string range = "24hours")
    {
        string authHeader = "x-umami-api-key";
        bool authHeaderExists = Request.Headers.TryGetValue(authHeader, out StringValues requestHeaders);
        string? apiKey = requestHeaders.FirstOrDefault();

        if (authHeaderExists && !String.IsNullOrEmpty(apiKey))
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            DateTimeOffset startUtc = utcNow, endUtc = utcNow;

            switch (range.ToLower())
            {
                case "today":
                    startUtc = utcNow.Date;
                    endUtc = startUtc.AddDays(1).AddMilliseconds(-1);
                    break;
                case "24hours":
                    endUtc = utcNow;
                    startUtc = endUtc.AddHours(-24);
                    break;
                case "7days":
                    startUtc = utcNow.Date.AddDays(-6);
                    endUtc = utcNow.Date.AddDays(1).AddMilliseconds(-1);
                    break;
                case "30days":
                    startUtc = utcNow.Date.AddDays(-29);
                    endUtc = utcNow.Date.AddDays(1).AddMilliseconds(-1);
                    break;
                case "60days":
                    startUtc = utcNow.Date.AddDays(-59);
                    endUtc = utcNow.Date.AddDays(1).AddMilliseconds(-1);
                    break;
                case "90days":
                    startUtc = utcNow.Date.AddDays(-90);
                    endUtc = utcNow.Date.AddDays(1).AddMilliseconds(-1);
                    break;
                case "6months":
                    startUtc = utcNow.Date.AddMonths(-6).AddDays(1);
                    endUtc = utcNow.Date.AddDays(1).AddMilliseconds(-1);
                    break;
                case "9months":
                    startUtc = utcNow.Date.AddMonths(-9).AddDays(1);
                    endUtc = utcNow.Date.AddDays(1).AddMilliseconds(-1);
                    break;
                case "1year":
                    startUtc = utcNow.Date.AddYears(-1).AddDays(1);
                    endUtc = utcNow.Date.AddDays(1).AddMilliseconds(-1);
                    break;
                default:
                    endUtc = utcNow;
                    startUtc = endUtc.AddHours(-24);
                    break;
            }

            long startAt = startUtc.ToUnixTimeMilliseconds();
            long endAt = endUtc.ToUnixTimeMilliseconds();

            var uri = $"https://api.umami.is/v1/websites/{websiteId}/stats?startAt={startAt}&endAt={endAt}";
            var apiRequest = new HttpRequestMessage(HttpMethod.Get, uri);
            apiRequest.Headers.Add(authHeader, apiKey);

            var apiResponse = await _httpClient.SendAsync(apiRequest);
            if (!apiResponse.IsSuccessStatusCode)
            {
                return Problem("Error calling Umami API", statusCode: (int)apiResponse.StatusCode);
            }

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            WebsiteStats apiStats = JsonConvert.DeserializeObject<WebsiteStats>(apiContent) ?? new WebsiteStats();

            var stats = new
            {
                StartAt = startAt,
                EndAt = endAt,
                apiStats
            };

            return Ok(stats);
        }
        else
        {
            return Unauthorized("API key is missing.");
        }
    }
}
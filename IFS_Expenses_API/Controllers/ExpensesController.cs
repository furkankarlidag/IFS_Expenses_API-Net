using IFS_Expenses_API.Models;
using IFS_Expenses_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IFS_Expenses_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public ExpensesController(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses(string username, int year, int period)
        {
            var accessToken = await _authService.GetBearerToken();
            var apiUrl = $"https://pame8xi-dev1.build.ifs.cloud/int/ifsapplications/projection/v1/NextFrkService.svc/GetExpenses(PersonId='{username}',Year={year},Period={period})";

            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var apiData = JsonSerializer.Deserialize<object>(responseContent);

            return Ok(apiData);
        }

    }
}

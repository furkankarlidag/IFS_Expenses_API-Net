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
    public class ExpenseCodesController : ControllerBase
    {
        private readonly HttpClient _httpClient;    
        private readonly AuthService _authService;

        public ExpenseCodesController(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenseCodes()
        {
            var accessToken = await _authService.GetBearerToken();
            var apiUrl = "***";

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

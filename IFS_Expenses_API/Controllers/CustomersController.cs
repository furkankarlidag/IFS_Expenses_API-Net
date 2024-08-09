using IFS_Expenses_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
namespace IFS_Expenses_API;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly HttpClient _httpClient;

    public CustomersController(AuthService authService, HttpClient httpClient)
    {
        _authService = authService;
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var accessToken = await _authService.GetBearerToken();
        var apiUrl = "https://pame8xi-dev1.build.ifs.cloud/int/ifsapplications/projection/v1/NextFrkService.svc/Reference_Customers";

        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var apiData = JsonSerializer.Deserialize<object>(responseContent);

        return Ok(apiData);
    }
}

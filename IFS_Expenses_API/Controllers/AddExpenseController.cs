using IFS_Expenses_API.Models;
using IFS_Expenses_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IFS_Expenses_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddExpenseController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public AddExpenseController(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseModel Expense)
        {
            if(Expense == null)
            {
                return BadRequest(new { error = "data is required!" });
            }

            var accessToken = await _authService.GetBearerToken();
            var apiUrl = "https://pame8xi-dev1.build.ifs.cloud/int/ifsapplications/projection/v1/NextFrkService.svc/CreateExpense";
            
            _httpClient.BaseAddress = new Uri(apiUrl);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var settings = new JsonSerializerSettings
            {
                Converters = new[] { new CustomDateFormatConverter("yyyy-MM-dd") } 
            };
            var data = JsonConvert.SerializeObject(Expense,settings);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response  =  await _httpClient.PostAsync(apiUrl, content);

            if(response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AddExpenseReturn>(responseContent);

                return Ok(result);

            }
            else
                return StatusCode((int)response.StatusCode, new { error = "API request failed", status_code = response.StatusCode, response = await response.Content.ReadAsStringAsync() });
        }
    }


}

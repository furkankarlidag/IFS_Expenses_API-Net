using IFS_Expenses_API.Models;
using IFS_Expenses_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace IFS_Expenses_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AccountController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;
        private readonly AuthService _authService;

        public AccountController(HttpClient httpClient, IOptions<JwtSettings> jwtSettings, AuthService authService)
        {
            _httpClient = httpClient;
            _tokenService = new TokenService(jwtSettings.Value);
            _authService = authService;
           
        }
        
        [HttpGet("Login")]
        public async Task<IActionResult> Login (string username, string password)
        {

            var apiUrl = $"(PersonId='{username}',Password='{password}')";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new {error = "username and password are both required!."});
            }

            var bearerToken = await _authService.GetBearerToken();
            
            if(bearerToken == null)
            {
                return BadRequest(new { error = "An error occured while taking token!" });
            }

            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                /// bu kismi bi duzelt gardas
                var dataFromApi = System.Text.Json.JsonSerializer.Deserialize<LoginReturn>(responseData);
                if (dataFromApi.Success)
                {


                    var token = _tokenService.GenerateToken(username);
                    Console.WriteLine("generated token: " + token);
                    if (token != null && dataFromApi.Name != null)
                    {
                        return Ok(new { Token = token });
                    }
                    else
                        return BadRequest(new { Error = "token is broken" });
                }
                else
                    return NotFound(new { Error = dataFromApi.ErrorDescription });
            }

            else
                return StatusCode((int)response.StatusCode, new { error = "API request failed", status_code = response.StatusCode, response = await response.Content.ReadAsStringAsync() });
        }

        
    }

   
}

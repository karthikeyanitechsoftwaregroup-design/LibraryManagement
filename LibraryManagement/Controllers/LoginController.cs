using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace LibraryManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IConfiguration config, ILogger<LoginController> logger)
        {
            _config = config;
            _logger = logger;
        }

        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAuth([FromBody] Models.ValidateUserModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    return Json(new
                    {
                        success = false,
                        statusCode = 400,
                        resultMessage = "Username and Password are required"
                    });
                }

                string apiUrl = _config["ApiUrl"] + "api/ValidateUser/login";
                _logger.LogInformation($"Calling API at: {apiUrl}");

                using (HttpClient client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"API Error: {response.StatusCode}");
                        return Json(new
                        {
                            success = false,
                            statusCode = (int)response.StatusCode,
                            resultMessage = "API Error: " + response.StatusCode
                        });
                    }

                    var result = await response.Content.ReadAsStringAsync();
                    var apiResult = JsonConvert.DeserializeObject<dynamic>(result);

                    int statusCode = (int)apiResult.statusCode;
                    string message = apiResult.resultMessage.ToString();

                    _logger.LogInformation($"API returned - Status: {statusCode}, Message: {message}");

                    
                    if (statusCode == 200)
                    {
                        
                        HttpContext.Session.SetString("Username", model.Username);

                        return Json(new
                        {
                            success = true,
                            statusCode = statusCode,
                            resultMessage = "Login Successful",
                            redirectUrl = Url.Action("Dashboard", "Dashboard")
                        });
                    }
                    else if (statusCode == 404)
                    {
                        return Json(new
                        {
                            success = false,
                            statusCode = statusCode,
                            resultMessage = message
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            statusCode = statusCode,
                            resultMessage = message
                        });
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP Request Exception: {ex.Message}");
                return Json(new
                {
                    success = false,
                    statusCode = 503,
                    resultMessage = "Unable to connect to API. Please ensure the API is running."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected Error: {ex.Message}");
                return Json(new
                {
                    success = false,
                    statusCode = 500,
                    resultMessage = "An unexpected error occurred"
                });
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();

            // Optional: remove session cookie
            Response.Cookies.Delete(".AspNetCore.Session");

            return RedirectToAction("Login", "Login");
        }

    }
}
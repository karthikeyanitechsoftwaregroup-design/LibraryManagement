using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace LibraryManagement.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<BookController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public BookController(IConfiguration config, ILogger<BookController> logger, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        
        public async Task<IActionResult> Book()
        {
            try
            {
                string apiUrl = _config["ApiUrl"] + "api/Book/GetAllBook";
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var books = JsonConvert.DeserializeObject<List<BookModel>>(json);
                    return View(books);
                }

                TempData["Error"] = "Unable to fetch books";
                return View(new List<BookModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching books: {ex.Message}");
                TempData["Error"] = "Error connecting to API";
                return View(new List<BookModel>());
            }
        }

        
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Invalid data" });

                model.CreatedBy = "1";
                model.CreatedDate = DateTime.Now;

                string apiUrl = _config["ApiUrl"] + "api/Book/CreateBook";

                var client = _httpClientFactory.CreateClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                var result = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(result);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(
                        (int)response.StatusCode,
                        new
                        {
                            success = false,
                            message = apiResponse?.Message
                        }
                    );
                }

                return Ok(new
                {
                    success = true,
                    message = apiResponse?.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }




        [HttpPost]
        public async Task<IActionResult> UpdateBook([FromBody] BookModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Invalid data" });

                
                model.CreatedBy = "1";
                model.CreatedDate = DateTime.Now;

                string apiUrl = _config["ApiUrl"] + "api/Book/UpdateBook";

                var client = _httpClientFactory.CreateClient();

                var json = JsonConvert.SerializeObject(model);
                

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(apiUrl, content);
                var result = await response.Content.ReadAsStringAsync();

                

                if (!response.IsSuccessStatusCode)
                    return Json(new { success = false, message = "Failed to update book" });

                return Json(new { success = true, message = "Book updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }





        [HttpPost]
        public async Task<IActionResult> Delete(int bookUid)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var apiUrl = _config["ApiUrl"] + $"api/Book/DeleteBook/{bookUid}";

                var response = await client.DeleteAsync(apiUrl);
                var json = await response.Content.ReadAsStringAsync();

                var apiResult = JsonConvert.DeserializeObject<ApiResponse<object>>(json);

                if (apiResult == null)
                {
                    return BadRequest(new { success = false, message = "Invalid API response" });
                }

                return response.IsSuccessStatusCode
                    ? Ok(new { success = true, message = apiResult.Message ?? "Success" })
                    : BadRequest(new { success = false, message = apiResult.Message ?? "Failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                string apiUrl = _config["ApiUrl"] + $"api/Book/GetBookById/{id}";
                var client = _httpClientFactory.CreateClient();

                var response = await client.GetAsync(apiUrl);
                var json = await response.Content.ReadAsStringAsync();


                if (!response.IsSuccessStatusCode)
                    return Json(new { success = false, message = "Book not found" });

                var book = JsonConvert.DeserializeObject<BookModel>(json);

                if (book == null)
                    return Json(new { success = false, message = "Book not found" });

                return Json(new
                {
                    success = true,
                    data = book
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Details error: {ex.Message}");
                return Json(new { success = false, message = "Error fetching details" });
            }
        }






    }
}

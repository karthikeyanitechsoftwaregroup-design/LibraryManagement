using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace LibraryManagement.Controllers
{
    public class TrackingController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TrackingController> _logger;

        public TrackingController(
            IConfiguration config,
            IHttpClientFactory httpClientFactory,
            ILogger<TrackingController> logger)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        
        public async Task<IActionResult> Tracking()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiUrl = _config["ApiUrl"] + "api/Tracking/GetAllTracking";

                var response = await client.GetAsync(apiUrl);
                var json = await response.Content.ReadAsStringAsync();

                var list = JsonConvert.DeserializeObject<List<TrackingModelById>>(json)
                           ?? new List<TrackingModelById>();

                return View(list); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tracking list error");
                return View(new List<TrackingModelById>());
            }
        }

        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiUrl = _config["ApiUrl"] + $"api/Tracking/GetTrackingById/{id}";

                var response = await client.GetAsync(apiUrl);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Json(new { success = false, message = "Tracking not found" });

                var model = JsonConvert.DeserializeObject<TrackingModelById>(json);

                return Json(new { success = true, data = model });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tracking details error");
                return Json(new { success = false, message = "Error fetching tracking" });
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TrackingModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data" });

            model.CreatedBy = "1";
            model.CreatedDate = DateTime.Now;

            var client = _httpClientFactory.CreateClient();
            string apiUrl = _config["ApiUrl"] + "api/Tracking/CreateTracking";

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            var result = await response.Content.ReadAsStringAsync();

            return Json(new
            {
                success = response.IsSuccessStatusCode,
                message = CleanMessage(result)
            });
        }

        
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] TrackingModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data" });

            var client = _httpClientFactory.CreateClient();
            string apiUrl = _config["ApiUrl"] + "api/Tracking/UpdateTracking";

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(apiUrl, content);
            var result = await response.Content.ReadAsStringAsync();

            return Json(new
            {
                success = response.IsSuccessStatusCode,
                message = CleanMessage(result)
            });
        }

        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            string apiUrl = _config["ApiUrl"] + $"api/Tracking/DeleteTracking?id={id}";

            var response = await client.DeleteAsync(apiUrl);
            var result = await response.Content.ReadAsStringAsync();

            return Json(new
            {
                success = response.IsSuccessStatusCode,
                message = CleanMessage(result)
            });
        }

        private static string CleanMessage(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "Operation completed";

            raw = raw.Replace("\"", "").Trim();

            if (raw.Contains("-"))
                raw = raw.Split('-', 2)[1].Trim();

            return raw.TrimEnd('}');
        }

        [HttpGet]
        public async Task<IActionResult> GetBooksForDropdown()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiUrl = _config["ApiUrl"] + "api/Book/GetAvailableBooks";

                var response = await client.GetAsync(apiUrl);
                var json = await response.Content.ReadAsStringAsync();

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading books");
                return Json(new List<object>());
            }
        }

    }
}

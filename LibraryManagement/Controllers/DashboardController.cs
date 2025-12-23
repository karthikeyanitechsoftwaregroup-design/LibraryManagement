using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndecisionEngine.Controllers
{
    [Authorize("Admin")]
    [RequireHttps]
    public class AdminController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}

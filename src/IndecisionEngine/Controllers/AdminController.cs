using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace IndecisionEngine.Controllers
{
    [Authorize("Admin")]
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

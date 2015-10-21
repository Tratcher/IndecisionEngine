using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndecisionEngine.ViewModels.Story;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IndecisionEngine.Controllers
{
    public class StoryController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Read()
        {
            ViewData["Message"] = "Your story entry goes here.";

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Message"] = "This is where you create an entry.";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            ViewData["Message"] = "Your entry was saved.";
            ViewData["Body"] = model.Body;

            return View();
        }
    }
}

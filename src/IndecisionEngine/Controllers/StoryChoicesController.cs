using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IndecisionEngine.Models;
using Microsoft.AspNetCore.Authorization;

namespace IndecisionEngine.Controllers
{
    [Authorize]
    public class StoryChoicesController : Controller
    {
        private StoryDbContext _context;

        public StoryChoicesController(StoryDbContext context)
        {
            _context = context;    
        }

        // GET: StoryChoices
        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.StoryChoice.ToList());
        }

        // GET: StoryChoices/Details/5
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StoryChoice storyChoice = _context.StoryChoice.Single(m => m.Id == id);
            if (storyChoice == null)
            {
                return NotFound();
            }

            return View(storyChoice);
        }

        // GET: StoryChoices/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: StoryChoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StoryChoice storyChoice)
        {
            if (ModelState.IsValid)
            {
                _context.StoryChoice.Add(storyChoice);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storyChoice);
        }

        // GET: StoryChoices/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StoryChoice storyChoice = _context.StoryChoice.Single(m => m.Id == id);
            if (storyChoice == null)
            {
                return NotFound();
            }
            return View(storyChoice);
        }

        // POST: StoryChoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StoryChoice storyChoice)
        {
            if (ModelState.IsValid)
            {
                _context.Update(storyChoice);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storyChoice);
        }

        // GET: StoryChoices/Delete/5
        [ActionName("Delete")]
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StoryChoice storyChoice = _context.StoryChoice.Single(m => m.Id == id);
            if (storyChoice == null)
            {
                return NotFound();
            }

            return View(storyChoice);
        }

        // POST: StoryChoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            StoryChoice storyChoice = _context.StoryChoice.Single(m => m.Id == id);
            _context.StoryChoice.Remove(storyChoice);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAll()
        {
            _context.Database.ExecuteSqlCommand("delete from StoryChoice");
            return RedirectToAction("Index");
        }
    }
}

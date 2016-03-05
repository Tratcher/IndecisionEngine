using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using IndecisionEngine.Models;

namespace IndecisionEngine.Controllers
{
    public class StoryChoicesController : Controller
    {
        private StoryDbContext _context;

        public StoryChoicesController(StoryDbContext context)
        {
            _context = context;    
        }

        // GET: StoryChoices
        public IActionResult Index()
        {
            return View(_context.StoryChoice.ToList());
        }

        // GET: StoryChoices/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryChoice storyChoice = _context.StoryChoice.Single(m => m.Id == id);
            if (storyChoice == null)
            {
                return HttpNotFound();
            }

            return View(storyChoice);
        }

        // GET: StoryChoices/Create
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
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryChoice storyChoice = _context.StoryChoice.Single(m => m.Id == id);
            if (storyChoice == null)
            {
                return HttpNotFound();
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
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryChoice storyChoice = _context.StoryChoice.Single(m => m.Id == id);
            if (storyChoice == null)
            {
                return HttpNotFound();
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
    }
}

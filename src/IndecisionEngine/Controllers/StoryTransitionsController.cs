using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using IndecisionEngine.Models;

namespace IndecisionEngine.Controllers
{
    public class StoryTransitionsController : Controller
    {
        private StoryDbContext _context;

        public StoryTransitionsController(StoryDbContext context)
        {
            _context = context;    
        }

        // GET: StoryTransitions
        public IActionResult Index()
        {
            ViewData["entries"] = _context.StoryEntry;
            ViewData["choices"] = _context.StoryChoice;
            return View(_context.StoryTransition.ToList());
        }

        // GET: StoryTransitions/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryTransition storyTransition = _context.StoryTransition.Single(m => m.Id == id);
            if (storyTransition == null)
            {
                return HttpNotFound();
            }

            ViewData["entries"] = _context.StoryEntry;
            ViewData["choices"] = _context.StoryChoice;
            return View(storyTransition);
        }

        // GET: StoryTransitions/Create
        public IActionResult Create()
        {
            ViewData["entries"] = _context.StoryEntry;
            ViewData["choices"] = _context.StoryChoice;
            return View();
        }

        // POST: StoryTransitions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StoryTransition storyTransition)
        {
            if (ModelState.IsValid)
            {
                _context.StoryTransition.Add(storyTransition);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storyTransition);
        }

        // GET: StoryTransitions/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryTransition storyTransition = _context.StoryTransition.Single(m => m.Id == id);
            if (storyTransition == null)
            {
                return HttpNotFound();
            }

            ViewData["entries"] = _context.StoryEntry;
            ViewData["choices"] = _context.StoryChoice;
            return View(storyTransition);
        }

        // POST: StoryTransitions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StoryTransition storyTransition)
        {
            if (ModelState.IsValid)
            {
                _context.Update(storyTransition);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storyTransition);
        }

        // GET: StoryTransitions/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryTransition storyTransition = _context.StoryTransition.Single(m => m.Id == id);
            if (storyTransition == null)
            {
                return HttpNotFound();
            }

            ViewData["entries"] = _context.StoryEntry;
            ViewData["choices"] = _context.StoryChoice;
            return View(storyTransition);
        }

        // POST: StoryTransitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            StoryTransition storyTransition = _context.StoryTransition.Single(m => m.Id == id);
            _context.StoryTransition.Remove(storyTransition);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAll()
        {
            _context.Database.ExecuteSqlCommand("delete from StoryTransition");
            return View("Index", _context.StoryTransition.ToList());
        }
    }
}

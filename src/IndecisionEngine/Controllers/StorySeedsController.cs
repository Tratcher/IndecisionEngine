using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using IndecisionEngine.Models;

namespace IndecisionEngine.Controllers
{
    public class StorySeedsController : Controller
    {
        private StoryDbContext _context;

        public StorySeedsController(StoryDbContext context)
        {
            _context = context;    
        }

        // GET: StorySeeds
        public IActionResult Index()
        {
            ViewData["entries"] = _context.StoryEntries;
            return View(_context.StorySeed.ToList());
        }

        // GET: StorySeeds/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StorySeed storySeed = _context.StorySeed.Single(m => m.Id == id);
            if (storySeed == null)
            {
                return HttpNotFound();
            }

            ViewData["entries"] = _context.StoryEntries;
            return View(storySeed);
        }

        // GET: StorySeeds/Create
        public IActionResult Create()
        {
            ViewData["entries"] = _context.StoryEntries;
            return View();
        }

        // POST: StorySeeds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StorySeed storySeed)
        {
            if (ModelState.IsValid)
            {
                _context.StorySeed.Add(storySeed);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storySeed);
        }

        // GET: StorySeeds/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StorySeed storySeed = _context.StorySeed.Single(m => m.Id == id);
            if (storySeed == null)
            {
                return HttpNotFound();
            }

            ViewData["entries"] = _context.StoryEntries;
            return View(storySeed);
        }

        // POST: StorySeeds/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StorySeed storySeed)
        {
            if (ModelState.IsValid)
            {
                _context.Update(storySeed);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storySeed);
        }

        // GET: StorySeeds/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StorySeed storySeed = _context.StorySeed.Single(m => m.Id == id);
            if (storySeed == null)
            {
                return HttpNotFound();
            }

            ViewData["entries"] = _context.StoryEntries;
            return View(storySeed);
        }

        // POST: StorySeeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            StorySeed storySeed = _context.StorySeed.Single(m => m.Id == id);
            _context.StorySeed.Remove(storySeed);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using IndecisionEngine.Models;

namespace IndecisionEngine.Controllers
{
    public class StoryExplorerController : Controller
    {
        private StoryDbContext _context;

        public StoryExplorerController(StoryDbContext context)
        {
            _context = context;    
        }

        // GET: StoryExplorer
        public IActionResult Index(int? id)
        {
            ViewData["transitions"] = _context.StoryTransition.Where(transition => transition.PriorEntryId == id);
            ViewData["choices"] = _context.StoryChoice;
            return View(_context.StoryEntries.FirstOrDefault(entry => entry.Id == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewChoice(int? id, string newChoiceBody)
        {
            if (ModelState.IsValid)
            {
                var choice = new StoryChoice() { Body = newChoiceBody };

                var entity = _context.StoryChoice.Add(choice);
                _context.SaveChanges();

                var transition = new StoryTransition()
                {
                    PriorEntryId = id,
                    ChoiceId = entity.Entity.Id,
                };

                _context.StoryTransition.Add(transition);
                _context.SaveChanges();

                ViewData["entries"] = _context.StoryEntries;
                ViewData["choices"] = _context.StoryChoice;
                return View("NewTransition", transition);
            }

            return new HttpStatusCodeResult(400);
        }
        
        public IActionResult NewTransition(int? id)
        {
            if (ModelState.IsValid)
            {
                var transition = new StoryTransition()
                {
                    PriorEntryId = id,
                };

                _context.StoryTransition.Add(transition);
                _context.SaveChanges();

                ViewData["entries"] = _context.StoryEntries;
                ViewData["choices"] = _context.StoryChoice;
                return View("NewTransition", transition);
            }

            return new HttpStatusCodeResult(400);
        }

        public IActionResult EditTransition(int? id)
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

            ViewData["entries"] = _context.StoryEntries;
            ViewData["choices"] = _context.StoryChoice;
            return View("NewTransition", storyTransition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveTransition(StoryTransition storyTransition)
        {
            if (ModelState.IsValid)
            {
                _context.Update(storyTransition);
                _context.SaveChanges();

                if (storyTransition.NextEntryId.HasValue)
                {
                    return RedirectToAction("Index", new { id = storyTransition.NextEntryId });
                }

                return RedirectToAction("Index", new { id = storyTransition.PriorEntryId });
            }

            return new HttpStatusCodeResult(400);
        }
    }
}

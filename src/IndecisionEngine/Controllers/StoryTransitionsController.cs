using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using IndecisionEngine.Models;
using IndecisionEngine.ViewModels.StoryTransitions;

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
            var viewModel = new StoryTransitionsIndexViewModel()
            {
                Transitions = _context.StoryTransition,
                Entries = _context.StoryEntry,
                Choices = _context.StoryChoice,
            };

            return View(viewModel);
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

            var viewModel = new StoryTransitionViewModel(storyTransition)
            {
                PriorEntry = _context.StoryEntry.FirstOrDefault(entry => entry.Id == storyTransition.PriorEntryId)?.Body,
                Choice = _context.StoryChoice.FirstOrDefault(choice => choice.Id == storyTransition.ChoiceId)?.Body,
                NextEntry = _context.StoryEntry.FirstOrDefault(entry => entry.Id == storyTransition.NextEntryId)?.Body,
            };

            return View(viewModel);
        }

        // GET: StoryTransitions/Create
        public IActionResult Create()
        {
            var viewModel = new StoryTransitionViewModel()
            {
                Choices = _context.StoryChoice,
                Entries = _context.StoryEntry,
            };

            return View(viewModel);
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

            var viewModel = new StoryTransitionViewModel(storyTransition)
            {
                Choices = _context.StoryChoice,
                Entries = _context.StoryEntry,
            };

            return View(viewModel);
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

            var viewModel = new StoryTransitionViewModel(storyTransition)
            {
                PriorEntry = _context.StoryEntry.FirstOrDefault(entry => entry.Id == storyTransition.PriorEntryId)?.Body,
                Choice = _context.StoryChoice.FirstOrDefault(choice => choice.Id == storyTransition.ChoiceId)?.Body,
                NextEntry = _context.StoryEntry.FirstOrDefault(entry => entry.Id == storyTransition.NextEntryId)?.Body,
            };

            return View(viewModel);
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
            return RedirectToAction("Index");
        }
    }
}

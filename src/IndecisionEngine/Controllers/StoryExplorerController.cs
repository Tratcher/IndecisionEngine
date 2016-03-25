using System.Linq;
using System.Threading.Tasks;
using IndecisionEngine.Models;
using IndecisionEngine.ViewModels.StoryExplorer;
using Microsoft.AspNet.Mvc;

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
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "StorySeeds");
            }

            var entry = _context.StoryEntry.FirstOrDefault(e => e.Id == id);

            var viewModel = new StoryExplorerViewModel()
            {
                Id = entry.Id,
                Body = entry.Body,
                Transitions = _context.StoryTransition.Where(transition => transition.PriorEntryId == id),
                Choices = _context.StoryChoice,
            };

            return View(viewModel);
        }

        public IActionResult Graph(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "StorySeeds");
            }

            var graphModel = new GraphViewModel()
            {
                Seed = _context.StorySeed.FirstOrDefault(entry => entry.Id == id),
                Entries = _context.StoryEntry,
                Transitions = _context.StoryTransition,
                Choices = _context.StoryChoice,
            };

            return View(graphModel);
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

                var transitionView = new NewTransitionViewModel()
                {
                    Id = transition.Id,
                    PriorEntryId = transition.PriorEntryId,
                    Entries = _context.StoryEntry,
                    Choices = _context.StoryChoice,
                };

                return View("NewTransition", transitionView);
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

            ViewData["entries"] = _context.StoryEntry;
            ViewData["choices"] = _context.StoryChoice;
            return View("NewTransition", storyTransition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTransition(StoryTransition storyTransition, string newChoice, string newEntry)
        {
            if (ModelState.IsValid)
            {
                if (!storyTransition.ChoiceId.HasValue && !string.IsNullOrEmpty(newChoice))
                {
                    var choice = new StoryChoice() { Body = newChoice };
                    _context.StoryChoice.Add(choice);
                    await _context.SaveChangesAsync();
                    storyTransition.ChoiceId = choice.Id;
                }
                if (!storyTransition.NextEntryId.HasValue && !string.IsNullOrEmpty(newEntry))
                {
                    var entry = new StoryEntry() { Body = newEntry };
                    _context.StoryEntry.Add(entry);
                    await _context.SaveChangesAsync();
                    storyTransition.NextEntryId = entry.Id;
                }

                _context.Update(storyTransition);
                _context.SaveChanges();
                /* This really depends on weither you like depth or bredth first story creation.
                if (storyTransition.NextEntryId.HasValue)
                {
                    return RedirectToAction("Index", new { id = storyTransition.NextEntryId });
                }
                */
                return RedirectToAction("Index", new { id = storyTransition.PriorEntryId });
            }

            return new HttpStatusCodeResult(400);
        }
    }
}

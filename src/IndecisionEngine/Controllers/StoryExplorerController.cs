using System.Linq;
using System.Threading.Tasks;
using IndecisionEngine.Models;
using IndecisionEngine.ViewModels.StoryExplorer;
using IndecisionEngine.ViewModels.StoryTransitions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndecisionEngine.Controllers
{
    [Authorize]
    [RequireHttps]
    public class StoryExplorerController : Controller
    {
        private StoryDbContext _context;

        public StoryExplorerController(StoryDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Start(int id)
        {
            var seed = _context.StorySeed.FirstOrDefault(s => s.Id == id);

            HistoryHelper.Reset(HttpContext, id);

            StateHelper.SetState(HttpContext, seed.InitialState);

            return RedirectToAction("Index", new { id = seed.FirstEntryId });
        }

        // GET: StoryExplorer
        [AllowAnonymous]
        [HttpGet]
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
                State = StateHelper.GetState(HttpContext),
                Transitions = _context.StoryTransition.Where(transition => transition.PriorEntryId == id),
                Choices = _context.StoryChoice,
            };

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Choose(int id)
        {
            if (ModelState.IsValid)
            {
                // Find the matching transition
                var transition = _context.StoryTransition.FirstOrDefault(t => t.Id == id);

                // TODO: Integrity check. Hitting the back button does not revert state or history.

                // Apply any state changes
                // TODO: This should be stored in the user database, but for now we're just going to use session.
                var state = StateHelper.Update(HttpContext, transition.Effects);

                // Update the history
                // TODO: This should be stored in the user database, but for now we're just going to use session.
                HistoryHelper.AppendToHistory(HttpContext, transition, state);

                // Navigate to the next entry
                return RedirectToAction("Index", new { id = transition.NextEntryId });
            }

            return new StatusCodeResult(400);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult History()
        {
            var seedId = HistoryHelper.GetSeedId(HttpContext);
            if (!seedId.HasValue)
            {
                return RedirectToAction("Index", "StorySeeds");
            }
            var seed = _context.StorySeed.FirstOrDefault(s => s.Id == seedId.Value);

            // TODO: Pagination
            var viewModel = new HistoryViewModel()
            {
                Seed = seed,
                History = HistoryHelper.GetHistory(HttpContext),
                Choices = _context.StoryChoice,
                Entries = _context.StoryEntry,
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GoBack()
        {
            var historyEntry = HistoryHelper.GoBack(HttpContext);
            if (historyEntry == null)
            {
                var seed = HistoryHelper.GetSeedId(HttpContext);
                if (seed.HasValue)
                {
                    return RedirectToAction(nameof(Start), new { id = seed.Value });
                }
                return RedirectToAction(nameof(StorySeedsController.Index), nameof(StorySeedsController));
            }

            StateHelper.SetState(HttpContext, historyEntry.EndState);

            return RedirectToAction("Index", new { id = historyEntry.EndEntryId });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GoBackTo(int id)
        {
            var historyEntry = HistoryHelper.GoBackTo(HttpContext, id);

            StateHelper.SetState(HttpContext, historyEntry.EndState);

            return RedirectToAction("Index", new { id = historyEntry.EndEntryId });
        }

        [HttpGet]
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

        [HttpGet]
        public IActionResult NewTransition(int? id)
        {
            if (ModelState.IsValid)
            {
                var viewModel = new StoryTransitionViewModel()
                {
                    PriorEntryId = id,
                    PriorEntry = _context.StoryEntry.FirstOrDefault(entry => entry.Id == id)?.Body,
                    Choices = _context.StoryChoice,
                    Entries = _context.StoryEntry,
                };

                return View("EditTransition", viewModel);
            }

            return new StatusCodeResult(400);
        }

        [HttpGet]
        public IActionResult EditTransition(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StoryTransition storyTransition = _context.StoryTransition.Single(m => m.Id == id);
            if (storyTransition == null)
            {
                return NotFound();
            }

            var viewModel = new StoryTransitionViewModel(storyTransition)
            {
                PriorEntry = _context.StoryEntry.FirstOrDefault(entry => entry.Id == storyTransition.PriorEntryId)?.Body,
                Choices = _context.StoryChoice,
                Entries = _context.StoryEntry,
            };

            return View("EditTransition", viewModel);
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

                if (_context.StoryTransition.Any(t => t.Id == storyTransition.Id))
                {
                    _context.Update(storyTransition);
                }
                else
                {
                    _context.StoryTransition.Add(storyTransition);
                }

                _context.SaveChanges();
                /* This really depends on weither you like depth or bredth first story creation.
                if (storyTransition.NextEntryId.HasValue)
                {
                    return RedirectToAction("Index", new { id = storyTransition.NextEntryId });
                }
                */
                return RedirectToAction("Index", new { id = storyTransition.PriorEntryId });
            }

            return new StatusCodeResult(400);
        }
    }
}

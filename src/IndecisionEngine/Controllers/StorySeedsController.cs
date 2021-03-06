using System.Linq;
using System.Threading.Tasks;
using IndecisionEngine.Models;
using IndecisionEngine.ViewModels.StorySeeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndecisionEngine.Controllers
{
    [Authorize]
    [RequireHttps]
    public class StorySeedsController : Controller
    {
        private StoryDbContext _context;

        public StorySeedsController(StoryDbContext context)
        {
            _context = context;    
        }

        // GET: StorySeeds
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new StorySeedsIndexViewModel()
            {
                Seeds = _context.StorySeed,
                Entries = _context.StoryEntry,
            };

            return View(viewModel);
        }

        // GET: StorySeeds/Details/5
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StorySeed storySeed = _context.StorySeed.Single(m => m.Id == id);
            if (storySeed == null)
            {
                return NotFound();
            }

            var viewModel = new StorySeedViewModel(storySeed)
            {
                FirstEntry = _context.StoryEntry.FirstOrDefault(entry => entry.Id == storySeed.FirstEntryId)?.Body
            };

            return View(viewModel);
        }

        // GET: StorySeeds/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new StorySeedViewModel() { Entries = _context.StoryEntry });
        }

        // POST: StorySeeds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StorySeed storySeed, string newEntry)
        {
            if (ModelState.IsValid)
            {
                if (!storySeed.FirstEntryId.HasValue && !string.IsNullOrEmpty(newEntry))
                {
                    var entry = new StoryEntry() { Body = newEntry };
                    _context.StoryEntry.Add(entry);
                    await _context.SaveChangesAsync();
                    storySeed.FirstEntryId = entry.Id;
                }

                _context.StorySeed.Add(storySeed);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storySeed);
        }

        // GET: StorySeeds/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StorySeed storySeed = _context.StorySeed.Single(m => m.Id == id);
            if (storySeed == null)
            {
                return NotFound();
            }

            var viewModel = new StorySeedViewModel(storySeed)
            {
                Entries = _context.StoryEntry,
            };

            return View(viewModel);
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
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StorySeed storySeed = _context.StorySeed.Single(m => m.Id == id);
            if (storySeed == null)
            {
                return NotFound();
            }

            var viewModel = new StorySeedViewModel(storySeed)
            {
                FirstEntry = _context.StoryEntry.FirstOrDefault(entry => entry.Id == storySeed.FirstEntryId)?.Body,
            };

            return View(viewModel);
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

        [Authorize("Admin")]
        [HttpPost, ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAll()
        {
            _context.Database.ExecuteSqlCommand("delete from StorySeed");
            return RedirectToAction("Index");
        }

        // You can end up here if you tried to DeleteAll without being logged in first.
        // If you do have permissions after login you'll end up on the index page and can try again.
        // Otherwise you'll end up on the /Account/AccessDenied page.
        [Authorize("Admin")]
        [HttpGet, ActionName("DeleteAll")]
        public IActionResult DeleteAllLoginRedirect()
        {
            return RedirectToAction("Index");
        }
    }
}

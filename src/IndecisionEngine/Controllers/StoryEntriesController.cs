using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using IndecisionEngine.Models;

namespace IndecisionEngine.Controllers
{
    public class StoryEntriesController : Controller
    {
        private StoryDbContext _context;

        public StoryEntriesController(StoryDbContext context)
        {
            _context = context;    
        }

        // GET: StoryEntries
        public async Task<IActionResult> Index()
        {
            return View(await _context.StoryEntry.ToListAsync());
        }

        // GET: StoryEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryEntry storyEntry = await _context.StoryEntry.SingleAsync(m => m.Id == id);
            if (storyEntry == null)
            {
                return HttpNotFound();
            }

            return View(storyEntry);
        }

        // GET: StoryEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StoryEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StoryEntry storyEntry)
        {
            if (ModelState.IsValid)
            {
                _context.StoryEntry.Add(storyEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(storyEntry);
        }

        // GET: StoryEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryEntry storyEntry = await _context.StoryEntry.SingleAsync(m => m.Id == id);
            if (storyEntry == null)
            {
                return HttpNotFound();
            }
            return View(storyEntry);
        }

        // POST: StoryEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StoryEntry storyEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Update(storyEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(storyEntry);
        }

        // GET: StoryEntries/Delete/5
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryEntry storyEntry = await _context.StoryEntry.SingleAsync(m => m.Id == id);
            if (storyEntry == null)
            {
                return HttpNotFound();
            }

            return View(storyEntry);
        }

        // POST: StoryEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            StoryEntry storyEntry = await _context.StoryEntry.SingleAsync(m => m.Id == id);
            _context.StoryEntry.Remove(storyEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAll()
        {
            _context.Database.ExecuteSqlCommand("delete from StoryEntry");
            return View("Index", _context.StoryEntry.ToList());
        }
    }
}

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
            return View(await _context.StoryEntries.ToListAsync());
        }

        // GET: StoryEntries/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryEntry storyEntry = await _context.StoryEntries.SingleAsync(m => m.Id == id);
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
                _context.StoryEntries.Add(storyEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(storyEntry);
        }

        // GET: StoryEntries/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryEntry storyEntry = await _context.StoryEntries.SingleAsync(m => m.Id == id);
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
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            StoryEntry storyEntry = await _context.StoryEntries.SingleAsync(m => m.Id == id);
            if (storyEntry == null)
            {
                return HttpNotFound();
            }

            return View(storyEntry);
        }

        // POST: StoryEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            StoryEntry storyEntry = await _context.StoryEntries.SingleAsync(m => m.Id == id);
            _context.StoryEntries.Remove(storyEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

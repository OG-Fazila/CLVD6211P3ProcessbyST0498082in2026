using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using EventEas3.Models;

namespace EventEas3.Controllers
{
    public class EventieController : Controller
    {
        private readonly EventEas3Context _context;

        public EventieController(EventEas3Context context)
        {
            _context = context;
        }

        // GET: Eventie
        public async Task<IActionResult> Index()
        {
            return View(await _context.Eventies.ToListAsync());
        }

        // GET: Eventie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Eventie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventID,eDateTime,eType,eDuration,eLimit")] Eventie eventie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventie);
        }

        // GET: Eventie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var eventie = await _context.Eventies
                .FirstOrDefaultAsync(m => m.EventID == id);

            if (eventie == null) return NotFound();

            // Check if it has active bookings in EventEaser
            bool hasBookings = await _context.EventEars.AnyAsync(ee => ee.EventID == id);
            if (hasBookings)
            {
                ViewBag.ErrorMessage = "This event has active bookings and cannot be deleted.";
            }

            return View(eventie);
        }

        // POST: Eventie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Fail-safe check: Ensure no bookings exist before allowing deletion
            bool hasBookings = await _context.EventEars.AnyAsync(ee => ee.EventID == id);
            if (hasBookings)
            {
                TempData["Error"] = "Cannot delete an event that has active bookings!";
                return RedirectToAction(nameof(Index));
            }

            var eventie = await _context.Eventies.FindAsync(id);
            if (eventie != null)
            {
                _context.Eventies.Remove(eventie);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
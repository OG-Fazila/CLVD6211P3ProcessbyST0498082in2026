
using EventEas3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEas3.Controllers
{
    public class EventEaserController : Controller
    {
        private readonly EventEas3Context _context;

        public EventEaserController(EventEas3Context context)
        {
            _context = context;
        }

        // GET: EventEaser
        public async Task<IActionResult> Index()
        {
            var context = _context.EventEars.Include(e => e.Booking).Include(e => e.Eventie).Include(e => e._Venue);
            return View(await context.ToListAsync());
        }

        // GET: EventEaser/Create
        public IActionResult Create()
        {
            ViewData["BookingID"] = new SelectList(_context.Bookings, "BookingID", "bName");
            ViewData["EventID"] = new SelectList(_context.Eventies, "EventID", "eType");
            ViewData["VenueID"] = new SelectList(_context._Venues, "VenueID", "vName");
            return View();
        }

        // POST: EventEaser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventEaserID,EventID,VenueID,BookingID")] EventEaser eventEaser)
        {
            // 1. Fetch the Event details and the Booking details to check dates/types
            var selectedEvent = await _context.Eventies.FindAsync(eventEaser.EventID);
            var selectedBooking = await _context.Bookings.FindAsync(eventEaser.BookingID);

            if (selectedEvent != null && selectedBooking != null)
            {
                string eventType = selectedEvent.eType.ToLower().Trim();

                // Group classifications
                string[] solitaryTypes = { "doctor", "studio", "therapist", "solitary" };

                // --- CRITICAL LOGIC RULE 1: SOLITARY EVENTS ---
                if (solitaryTypes.Contains(eventType))
                {
                    // Check if there is already ANY booking booked at this exact Date and Time
                    bool timeConflict = await _context.EventEars
                        .Include(ee => ee.Booking)
                        .Include(ee => ee.Eventie)
                        .AnyAsync(ee => ee.Booking.bDate == selectedBooking.bDate &&
                                        ee.Booking.bTime == selectedBooking.bTime);

                    if (timeConflict)
                    {
                        ModelState.AddModelError("", "This time slot is already booked for a solitary service (Doctor/Studio/Therapist).");
                    }
                }
                // --- CRITICAL LOGIC RULE 2: SOCIAL EVENTS ---
                else
                {
                    // Count how many current bookings are tied to this specific Event ID
                    int currentBookingCount = await _context.EventEars
                        .CountAsync(ee => ee.EventID == eventEaser.EventID);

                    if (currentBookingCount >= selectedEvent.eLimit)
                    {
                        ModelState.AddModelError("", $"Booking failed. This event ({selectedEvent.eType}) has reached its limit of {selectedEvent.eLimit} attendees.");
                    }
                }
            }

            // If the checks passed and ModelState is still valid, save it!
            if (ModelState.IsValid)
            {
                _context.Add(eventEaser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we fail, rebuild the dropdown menus and return to view with errors
            ViewData["BookingID"] = new SelectList(_context.Bookings, "BookingID", "bName", eventEaser.BookingID);
            ViewData["EventID"] = new SelectList(_context.Eventies, "EventID", "eType", eventEaser.EventID);
            ViewData["VenueID"] = new SelectList(_context._Venues, "VenueID", "vName", eventEaser.VenueID);
            return View(eventEaser);
        }
    }
}
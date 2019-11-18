using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Controllers
{
    public class GuestBookingsController : Controller
    {
        private readonly EventsDbContext _context;

        public GuestBookingsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: GuestBookings/Index/EVENTID
        public async Task<IActionResult> Index(int? id)
        {
            var eventsDbContext = _context.Guests.Include(g => g.Customer).Include(g => g.Event);
            if (id.HasValue)
            {
                Event ev = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
                if (ev != null)
                {
                    ViewData["Title"] = "Guest Bookings for "+ev.Title;
                    var outList = await eventsDbContext.Where(g => g.EventId == ev.Id).ToListAsync();
                    return View(outList);
                }
            }
            ViewData["Title"] = "All Guest Bookings";
            return View(await eventsDbContext.ToListAsync());
        }

        // GET: GuestBookings/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title");
            return View();
        }

        // POST: GuestBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,EventId,Attended")] GuestBooking guestBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guestBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", guestBooking.CustomerId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", guestBooking.EventId);
            return View(guestBooking);
        }

        // GET: GuestBookings/Attend/5
        public async Task<IActionResult> Attend(int? id, int? customerId)
        {
            if (id == null || customerId == null) 
                return NotFound();

            GuestBooking booking = await _context.Guests.FindAsync(customerId, id);
            if (booking == null)
                return NotFound();

            if (booking.Attended == true) // Already attended
                return RedirectToAction(nameof(Index), new { id });

            booking.Attended = true; // Attend 
            _context.Entry(booking).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id });

        }

        // GET: GuestBookings/Delete/5
        public async Task<IActionResult> Remove(int? id, int? customerId)
        {
            if (id == null || customerId == null)
                return NotFound();

            var guestBooking = await _context.Guests
                .Include(g => g.Customer)
                .Include(g => g.Event)
                .FirstOrDefaultAsync(m => m.CustomerId == customerId && m.EventId == id);

            if (guestBooking == null)
                return NotFound();

            return View(guestBooking);
        }

        // POST: GuestBookings/Delete/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id, int customerId)
        {
            var guestBooking = await _context.Guests.FirstOrDefaultAsync(x => x.CustomerId == customerId && x.EventId == id);
            if (guestBooking == null)
                return BadRequest();

            _context.Guests.Remove(guestBooking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

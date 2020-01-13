using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    /// <summary>
    /// ASP.NET MVC Controller for /Gu
    /// </summary>
    public class GuestBookingsController : Controller
    {
        private readonly EventsDbContext _context;

        public GuestBookingsController(EventsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// HTTP GET endpoint for "/GuestBookings/Index/<paramref name="id"/>". <para/>
        /// Shows a list of existing bookings via a <see cref="System.Collections.Generic.List{GuestBooking}"/>
        /// (T is <see cref="GuestBooking"/>). <para />
        /// If an <paramref name="id"/> is specified; an <see cref="Event.Id"/> filter is applied;
        /// otherwise all bookings are shown.
        /// </summary>
        /// <returns>Directs the user to the <see cref="Index(int?)"/> view.</returns>
        public async Task<IActionResult> Index(int? id)
        {
            var eventsDbContext = _context.Guests
                                    .Include(g => g.Customer)
                                    .Include(g => g.Event)
                                    .Where(g => !g.Customer.Deleted)
                                    .Where(g => !g.Event.Cancelled);
            if (id.HasValue)
            {
                Event ev = await _context.Events.Where(x => !x.Cancelled)
                                .FirstOrDefaultAsync(e => e.Id == id);
                if (ev != null)
                {
                    ViewData["Title"] = "Guest Bookings for "+ev.Title;
                    var outList = await eventsDbContext
                                    .Where(g => g.EventId == ev.Id)
                                    .ToListAsync();
                    return View(outList);
                }
            }
            ViewData["Title"] = "All Guest Bookings";
            return View(await eventsDbContext.ToListAsync());
        }

        /// <summary>
        /// HTTP GET endpoint for "/GuestBookings/Create/". <para/>
        /// Shows the creation options for a new <see cref="GuestBooking"/>.
        /// </summary>
        /// <returns>Directs the user to the <see cref="Create"/> view.</returns>
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title");
            return View();
        }

        /// <summary>
        /// HTTP POST endpoint for "/GuestBookings/Create/". <para/>
        /// Creates a new <see cref="GuestBooking"/> via the <paramref name="guestBooking"/> model given.
        /// </summary>
        /// <param name="guestBooking">A <see cref="GuestBookingCreateViewModel"/> with required
        /// information bound to it.</param>
        /// <returns>Directs the user to the <see cref="Index"/> view on success or <see cref="Create"/> on fail.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,EventId,Attended")] GuestBookingCreateViewModel guestBooking)
        {
            if (ModelState.IsValid)
            {
                GuestBooking existing = await _context.Guests.FindAsync(guestBooking.EventId, guestBooking.CustomerId);
                if (existing != null)
                {
                    // Exists
                    ViewData["CustomerId"] = new SelectList(await _context.Customers.Where(x => !x.Deleted).ToListAsync(), "Id", "FullName", guestBooking.CustomerId);
                    ViewData["EventId"] = new SelectList(await _context.Events.Where(x => !x.Cancelled).ToListAsync(), "Id", "Title", guestBooking.EventId);
                    ModelState.AddModelError(string.Empty, "The chosen guest is already booked into the chosen event.");
                    return View(guestBooking);
                }
                GuestBooking booking = new GuestBooking()
                {
                    CustomerId = guestBooking.CustomerId,
                    EventId = guestBooking.EventId,
                    Attended = guestBooking.Attended
                };
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(await _context.Customers.Where(x => !x.Deleted).ToListAsync(), "Id", "FullName", guestBooking.CustomerId);
            ViewData["EventId"] = new SelectList(await _context.Events.Where(x => !x.Cancelled).ToListAsync(), "Id", "Title", guestBooking.EventId);
            return View(guestBooking);
        }

        /// <summary>
        /// HTTP GET endpoint for "/GuestBookings/Attend/<paramref name="id"/>?customerId=<paramref name="customerId"/>". <para/>
        /// Marks attendance for a <see cref="Customer"/> whose Id is <paramref name="customerId"/> on the <see cref="Event"/>
        /// whose Id is <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The <see cref="Event"/>'s Id.</param>
        /// <param name="customerId">The <see cref="Customer"/>'s Id.</param>
        /// <returns>Directs the user to the <see cref="Index(int?)"/> view with the filter of <paramref name="id"/>.</returns>
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

            if (StringValues.IsNullOrEmpty(Request.Headers["Referer"]))
                return RedirectToAction(nameof(Index));
            return Redirect(Request.Headers["Referer"].ToString());

        }

        /// <summary>
        /// HTTP GET endpoint for "/GuestBookings/Remove/<paramref name="id"/>?customerId=<paramref name="customerId"/>". <para/>
        /// Shows the removal confirmation screen for a <see cref="GuestBooking"/>.
        /// Creates a <see cref="GuestBookingRemoveViewModel"/> to handle the data.
        /// </summary>
        /// <param name="customerId">The <see cref="Customer"/>'s Id whose booking is being removed.</param>
        /// <param name="id">The <see cref="Event"/> of which the <see cref="Customer"/> is being removed from.</param>
        /// <returns>Directs the user to the <see cref="Create"/> view.</returns>
        public async Task<IActionResult> Remove(int? id, int? customerId)
        {
            if (id == null || customerId == null)
                return NotFound();

            var guestBooking = await _context.Guests
                .Include(g => g.Customer)
                .Include(g => g.Event)
                .FirstOrDefaultAsync(m => m.CustomerId == customerId && m.EventId == id);

            if (guestBooking == null || guestBooking.Event.Cancelled || guestBooking.Customer.Deleted)
                return NotFound();

            GuestBookingRemoveViewModel viewModel = new GuestBookingRemoveViewModel()
            {
                Attended = guestBooking.Attended,
                Customer = guestBooking.Customer,
                CustomerId = guestBooking.CustomerId,
                Event = guestBooking.Event,
                EventId = guestBooking.EventId
            };

            return View(viewModel);
        }

        /// <summary>
        /// HTTP POST endpoint for "/GuestBookings/Remove/<paramref name="id"/>". <para/>
        /// Removes a <see cref="GuestBooking"/> via the <paramref name="id"/> and <paramref name="customerId"/>.
        /// </summary>
        /// <param name="customerId">The <see cref="Customer"/>'s Id.</param>
        /// <param name="id">The <see cref="Event"/>'s Id.</param>
        /// <returns>Directs the user to the <see cref="Index(int?)"/> view.</returns>
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id, int customerId)
        {
            var guestBooking = await _context.Guests
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.EventId == id);
            if (guestBooking == null)
                return BadRequest();

            _context.Guests.Remove(guestBooking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

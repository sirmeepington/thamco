using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    /// <summary>
    /// ASP.NET MVC Controller for /Staffs/*
    /// </summary>
    public class StaffsController : Controller
    {
        private readonly EventsDbContext _context;

        public StaffsController(EventsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// HTTP GET endpoint for "/Staff/Index/". <para/>
        /// Shows a list of existing staff via a <see cref="System.Collections.Generic.List{T}"/>
        /// (T is <see cref="StaffIndexViewModel"/>). <para />
        /// </summary>
        /// <returns>Directs the user to the <see cref="Index"/> view.</returns>
        public async Task<IActionResult> Index()
        {
            var staff = await _context.Staff.ToListAsync();
            var indexModel = staff.Select(x => new StaffIndexViewModel() {
                Email = x.Email,
                FirstAider = x.FirstAider, 
                Id = x.Id,
                Name = x.Name 
            }).ToList();

            return View(indexModel);
        }

        /// <summary>
        /// HTTP GET endpoint for "/Staff/Details/<paramref name="id"/>". <para/>
        /// Shows the details of a <see cref="Staff"/> member via the <see cref="StaffDetailsViewModel"/> view model.
        /// This includes a <see cref="System.Collections.Generic.List{T}"/> (T is <see cref="EventDetailsViewModel"/>) for
        /// the events that the staff member in.
        /// </summary>
        /// <returns>Directs the user to the <see cref="Details(int?)"/> view.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            var events = await _context.EventStaff.Include(x => x.Event).Where(x => x.StaffId == staff.Id && !x.Event.Cancelled).ToListAsync();

            StaffDetailsViewModel staffEventViewModel = new StaffDetailsViewModel()
            {
                Id = id.Value,
                Name = staff.Name,
                Events = events.Select(x => new EventDetailsViewModel() {
                    Id = x.Event.Id,
                    Duration = x.Event.Duration,
                    Date = x.Event.Date,
                    Title = x.Event.Title 
                }).ToList(),
                FirstAider = staff.FirstAider,
                Email = staff.Email
            };


            return View(staffEventViewModel);
        }

        /// <summary>
        /// HTTP GET endpoint for "/Staff/Create/". <para/>
        /// Shows the creation page for a new <see cref="Staff"/> member.
        /// </summary>
        /// <returns>Directs the user to the <see cref="Create"/> view.</returns>
        public IActionResult Create() => View();

        /// <summary>
        /// HTTP POST endpoint for "/Staff/Create/". <para/>
        /// Creates a new <see cref="Staff"/> member via the information passed through
        /// the <paramref name="staff"/> parameter.
        /// </summary>
        /// <param name="staff">The bound staff information passed via the form in the view.</param>
        /// <returns>Directs the user to the <see cref="Index"/> view on success; <see cref="Create"/> on fail.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Name")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        /// <summary>
        /// HTTP GET endpoint for "/Staff/Edit/<paramref name="id"/>". <para/>
        /// Shows the editing options for an existing <see cref="Staff"/> member whose id is 
        /// <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The <see cref="Staff"/> Id.</param>
        /// <returns>The <see cref="Edit(int?)"/> view.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        /// <summary>
        /// HTTP POST endpoint for "/Staff/Edit/<paramref name="id"/>". <para/>
        /// Edits the <see cref="Staff"/> record for the Id <paramref name="id"/> with the 
        /// information passed through the <paramref name="staff"/> parameter.
        /// </summary>
        /// <param name="id">The <see cref="Staff"/> Id.</param>
        /// <param name="staff">The <see cref="Staff"/> object with information bound to it.</param>
        /// <returns>The <see cref="Index"/> view on success; <see cref="Edit(int?)"/> on fail.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Email,Name,FirstAider")] Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(staff);

            try
            {
                _context.Update(staff);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(staff.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                } 
            }
            return RedirectToAction(nameof(Index));

        }

        /// <summary>
        /// HTTP GET endpoint for "/Staff/Delete/<paramref name="id"/>". <para/>
        /// Show the deletion confirmation for the <see cref="Staff"/> member whose
        /// Id is <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The <see cref="Staff"/> Id.</param>
        /// <returns>The <see cref="Delete(int?)"/> view.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        /// <summary>
        /// HTTP POST endpoint for "/Staff/Delete/<paramref name="id"/>". <para/>
        /// Deletes the <see cref="Staff"/> member's information (whose Id is <paramref name="id"/>) from the database.
        /// </summary>
        /// <param name="id">The <see cref="Staff"/> Id.</param>
        /// <returns>The <see cref="Index"/> view.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var staff = await _context.Staff.FindAsync(id);
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks whether a <see cref="Staff"/> member whose Id is <paramref name="id"/> exists.
        /// </summary>
        /// <param name="id">The <see cref="Staff"/>'s Id to check.</param>
        /// <returns>True if the staff member exists; false otherwise.</returns>
        private bool StaffExists(int? id) => _context.Staff.Any(e => e.Id == id);

        /// <summary>
        /// HTTP GET endpoint for "/Staff/RemoveFromEvent/<paramref name="id"/>?eventId=<paramref name="eventId"/>". <para/>
        /// Removes the <see cref="Staff"/> member whose Id is<paramref name="id"/> 
        /// from the <see cref="Event"/> whose Id is <paramref name="eventId"/>.
        /// </summary>
        /// <param name="id">The <see cref="Staff"/> Id.</param>
        /// <param name="eventId">The <see cref="Event"/> Id</param>
        /// <returns>The <see cref="Details(int?)"/> view.</returns>
        public async Task<IActionResult> RemoveFromEvent(int? id, int eventId)
        {
            if (!id.HasValue)
                return NotFound();

            var ev = await _context.EventStaff.FindAsync(id, eventId);
            if (ev == null)
                return RedirectToAction(nameof(Details), new { id });

            _context.Remove(ev);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}

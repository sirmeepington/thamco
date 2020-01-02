using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    /// <summary>
    /// ASP.NET MVC Controller for /Customers/*
    /// </summary>
    public class CustomersController : Controller
    {
        private readonly EventsDbContext _context;

        public CustomersController(EventsDbContext context) => _context = context;

        /// <summary>
        /// HTTP GET endpoint for "/Customers/". <para/>
        /// Returns a <see cref="System.Collections.Generic.List{T}"/> (T is <see cref="Customer"/>)
        /// of all customers to the Index view.
        /// </summary>
        /// <returns>Redirects the user to the Index view.</returns>
        public async Task<IActionResult> Index() => View(await _context.Customers.Where(c => !c.Deleted).ToListAsync());

        /// <summary>
        /// HTTP GET endpoint for "/Customers/Details/<paramref name="id"/>". <para/>
        /// Creates a new <see cref="CustomerDetailsViewModel"/> to the Details view. <para/>
        /// The view model created nests a <see cref="GuestBookingDetailsViewModel"/> and 
        /// <see cref="EventDetailsViewModel"/> for the sake of matching the view model to the request type.
        /// </summary>
        /// <param name="id">The <see cref="Customer"/>'s Id.</param>
        /// <returns>Redirects the user to the Details view.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            Customer customer = await _context.Customers
                .Where(c => !c.Deleted)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
                return NotFound();

            CustomerDetailsViewModel model = new CustomerDetailsViewModel()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                Surname = customer.Surname,
                Email = customer.Email,
                Bookings = await _context.Guests
                                .Include(b => b.Customer)
                                .Include(b => b.Event)
                                .Where(i => i.CustomerId == customer.Id)
                                .Select(x => new GuestBookingDetailsViewModel
                                {
                                    Attended = x.Attended,
                                    CustomerId = x.CustomerId,
                                    Event = new EventDetailsViewModel()
                                    {
                                        Id = x.Event.Id,
                                        Duration = x.Event.Duration,
                                        Title = x.Event.Title,
                                        Date = x.Event.Date,
                                        TypeId = x.Event.TypeId,
                                    },
                                    EventId = x.EventId
                                })
                                .ToListAsync()
            };

            return View(model);
        }

        /// <summary>
        /// HTTP GET endpoint for "/Customers/Create". <para/>
        /// Redirects the user to the creation view without any model attached.
        /// </summary>
        /// <returns>Redirects the user to the Create view.</returns>
        public IActionResult Create() => View();

        /// <summary>
        /// HTTP POST endpoint for "/Customers/Create". <para/>
        /// Redirects the user to the creation view without any model attached.
        /// </summary>
        /// <param name="customer">The <see cref="CustomerCreateViewModel"/> to which the data
        /// is bound to.</param>
        /// <returns>Redirects the user to the <see cref="Index"/> view on success or 
        /// <see cref="Create(CustomerCreateViewModel)"/> on fail.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Surname,FirstName,Email")] CustomerCreateViewModel customer)
        {
            if (ModelState.IsValid)
            {
                Customer model = new Customer()
                {
                    Id = customer.Id,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    Surname = customer.Surname
                };
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        /// <summary>
        /// HTTP GET endpoint for "/Customers/Edit/<paramref name="id"/>". <para/>
        /// Redirects the user to the edit view with an attached <see cref="CustomerEditViewModel"/>.
        /// </summary>
        /// <param name="id">The <see cref="Customer"/>'s Id.</param>
        /// <returns>Redirects the user to the <see cref="Edit(int, CustomerEditViewModel)"/> view.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Customer customer = await _context.Customers.FindAsync(id);
            if (customer == null || customer.Deleted)
                return NotFound();

            CustomerEditViewModel model = new CustomerEditViewModel()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                Surname = customer.Surname,
                Email = customer.Email
            };

            return View(model);
        }

        /// <summary>
        /// HTTP POST endpoint for "/Customers/Edit/<paramref name="id"/>". <para/>
        /// Redirects the user to the creation view without any model attached.
        /// </summary>
        /// <param name="id">The <see cref="Customer"/>'s Id.</param>
        /// <param name="customer">The <see cref="CustomerEditViewModel"/> to which the
        /// data is bound to.</param>
        /// <returns>Redirects the user to the <see cref="Index"/> view on success 
        /// or <see cref="Edit(int?)"/> view on fail.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Surname,FirstName,Email")] CustomerEditViewModel customer)
        {
            if (id != customer.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(customer);

            try
            {
                Customer currentCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == customer.Id && !c.Deleted);
                if (currentCustomer == null)
                    return BadRequest();

                currentCustomer.Email = customer.Email;
                currentCustomer.FirstName = customer.FirstName;
                currentCustomer.Surname = customer.Surname;

                _context.Update(currentCustomer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// HTTP GET endpoint for "/Customers/Delete/<paramref name="id"/>". <para/>
        /// Redirects the user to the deletion confirmation view.
        /// </summary>
        /// <param name="id">The <see cref="Customer"/>'s Id.</param>
        /// <returns>Redirects the user to the <see cref="DeleteConfirmed(int)"/> view.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            Customer customer = await _context.Customers.Where(c => !c.Deleted)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        /// <summary>
        /// HTTP GET endpoint for "/Customers/Create/<paramref name="id"/>". <para/>
        /// Anonymises the user's identifiable information and removes the other related
        /// information such as bookings.
        /// </summary>
        /// <param name="id">The <see cref="Customer"/>'s Id.</param>
        /// <returns>Redirects the user to the <see cref="Index"/> view.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Customer customer = await _context.Customers.FindAsync(id);
            if (customer.Deleted)
                return NotFound();
            customer.FirstName = "REDACTED";
            customer.Surname = "REDACTED";
            customer.Email = "REDACTED";
            customer.Deleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks whether or not a <see cref="Customer"/> with the given <paramref name="id"/> exists.
        /// </summary>
        /// <param name="id">The Id of a <see cref="Customer"/>.</param>
        /// <returns>True if a <see cref="Customer"/> with the given <paramref name="id"/> exists.</returns>
        private bool CustomerExists(int id) => _context.Customers.Where(c => !c.Deleted).Any(e => e.Id == id);
    }
}

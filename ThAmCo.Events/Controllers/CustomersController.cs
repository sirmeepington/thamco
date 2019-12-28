using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    public class CustomersController : Controller
    {
        private readonly EventsDbContext _context;

        public CustomersController(EventsDbContext context) => _context = context;

        // GET: Customers
        public async Task<IActionResult> Index() => View(await _context.Customers.Where(c => !c.Deleted).ToListAsync());

        // GET: Customers/Details/5
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
                Surname = customer.FirstName,
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

        // GET: Customers/Create
        public IActionResult Create() => View();

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Customers/Edit/5
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

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Surname,FirstName,Email")] CustomerEditViewModel customer)
        {
            if (id != customer.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
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
            return View(customer);
        }

        // GET: Customers/Delete/5
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

        // POST: Customers/Delete/5
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

        private bool CustomerExists(int id) => _context.Customers.Where(c => !c.Deleted).Any(e => e.Id == id);
    }
}

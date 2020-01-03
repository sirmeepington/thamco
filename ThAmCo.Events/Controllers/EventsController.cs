using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;
using ThAmCo.Venues.Data;
using ThAmCo.Venues.Models;
using ThAmCo.VenuesFacade;
using ThAmCo.VenuesFacade.Availabilities;
using ThAmCo.VenuesFacade.EventTypes;

namespace ThAmCo.Events.Controllers
{
    /// <summary>
    /// ASP.NET MVC Controller for the endpoint /Events/*
    /// </summary>
    public class EventsController : Controller
    {
        /// <summary>
        /// Event database context
        /// </summary>
        private readonly EventsDbContext _context;
        /// <summary>
        /// Interface for checking venue availabilities.
        /// </summary>
        private readonly IVenueAvailabilities _availabilities;
        /// <summary>
        /// Interface for assigning and removing reservations.
        /// </summary>
        private readonly IVenueReservation _reservations;
        /// <summary>
        /// Interface for accessing full names of event types via their ID.
        /// </summary>
        private readonly IEventTypes _eventTypes;

        public EventsController(
            EventsDbContext context, 
            IVenueAvailabilities availabilities, 
            IVenueReservation reservations, 
            IEventTypes eventTypes
            )
        {
            _context = context;
            _availabilities = availabilities;
            _reservations = reservations;
            _eventTypes = eventTypes;
        }

        /// <summary>
        /// HTTP GET endpoint for "/Events/" <para/>
        /// </summary>
        /// <returns>A <see cref="EventDetailsViewModel"/> for every event collated into a <see cref="List{T}"/>.</returns>
        public async Task<IActionResult> Index()
        {
            IEnumerable <Event> e = await _context.Events.Where(x => !x.Cancelled).ToListAsync();
            List<EventDetailsViewModel> model = new List<EventDetailsViewModel>();
            foreach (Event ev in e)
            {
                EventDetailsViewModel temp = new EventDetailsViewModel()
                {
                    Date = ev.Date,
                    Duration = ev.Duration,
                    Id = ev.Id,
                    Title = ev.Title,
                    TypeId = (await _eventTypes.GetEventType(ev.TypeId)).Title,
                    Warnings = await GetWarningTypeFromEvent(ev)
                };
                model.Add(temp);
            }

            return View(model);
        }

        /// <summary>
        /// HTTP GET endpoint for "/Events/Details/<paramref name="id"/>" <para/>
        /// </summary>
        /// <param name="id">The <see cref="Event"/>'s Id</param>
        /// <returns>A <see cref="EventDetailsViewModel"/> the specified event.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            Event @event = await _context.Events.Where(x => !x.Cancelled)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
                return NotFound();

            EventDetailsViewModel viewModel = new EventDetailsViewModel()
            {
                Id = @event.Id,
                Title = @event.Title,
                Date = @event.Date,
                Duration = @event.Duration,
                TypeId = (await _eventTypes.GetEventType(@event.TypeId)).Title,
                Bookings = await _context.Guests
                    .Include(e => e.Customer)
                    .Include(e => e.Event)
                    .Where(e => e.EventId == id)
                    .Select(x => new GuestBookingDetailsViewModel() // Convert to nested view models
                    {
                        Attended = x.Attended,
                        Customer = new CustomerDetailsViewModel()
                        {
                            Email = x.Customer.Email,
                            FirstName = x.Customer.FirstName,
                            FullName = x.Customer.FullName,
                            Id = x.Customer.Id,
                            Surname = x.Customer.Surname
                        },
                        Event = new EventDetailsViewModel()
                        {
                            Id = x.Event.Id,
                            Date = x.Event.Date,
                            Duration = x.Event.Duration,
                            Title = x.Event.Title,
                            TypeId = x.Event.TypeId
                        },
                        CustomerId = x.CustomerId,
                        EventId = x.EventId
                    }).ToListAsync(),
            };
            
            return View(viewModel);
        }

        /// <summary>
        /// A HTTP GET endpoint for "/Events/Venue/<paramref name="id"/>"
        /// </summary>
        /// <param name="id">The Id to be mapped to <see cref="Event.Id"/>.</param>
        /// <returns>A <see cref="EventVenueViewModel"/> for the <see cref="Event"/> returned via the Id <paramref name="id"/></returns>
        [HttpGet]
        public async Task<IActionResult> Venue(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            Event @event = await _context.Events.Where(x => !x.Cancelled)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
                return NotFound();


            if (@event.VenueReservation != null)
            {
                ReservationGetDto reservation = await _reservations.GetReservation(@event.VenueReservation);
                if (reservation.Reference == null)
                    goto NO_RES;

                // HACKY WORKAROUND
                List<AvailabilityApiGetDto> avail = await _availabilities
                    .GetAvailabilities(@event.TypeId, new DateTime(2018, 07, 10), new DateTime(2019, 2, 10));
                Venue venue = avail
                    .Where(x => x.Code == reservation.VenueCode)
                    .Select(a => new Venue() {
                        Code = a.Code,
                        Capacity = a.Capacity,
                        Description = a.Description,
                        Name = a.Name 
                    }).FirstOrDefault();

                if (venue == null)
                    return BadRequest(); // Unfortunately ran out of valid venues for this type due to some scuffed design preventing us doing it properly.

                EventVenueViewModel reservedViewModel = new EventVenueViewModel()
                {
                    Title = @event.Title,
                    Date = @event.Date,
                    Duration = @event.Duration,
                    Id = @event.Id,
                    TypeId = @event.TypeId,
                    TypeTitle = (await _eventTypes.GetEventType(@event.TypeId)).Title,
                    Reservation = new EventReservationViewModel()
                    {
                        Reference = reservation.Reference,
                        //StaffId = reservation.StaffId, // In the case of StaffId, it makes little sense having to include a single staff for a reservation
                                                         // when many staff can be assigned to an event and be added/removed at will.
                        WhenMade = reservation.WhenMade,
                        VenueCode = reservation.VenueCode,
                        EventDate = reservation.EventDate
                    },
                    Venue = venue,
                    ReservationReference = reservation.Reference
                };
                return View(reservedViewModel);
            }

            NO_RES: // Label to redirect execution out of the if statement and to avoid nested / inverted if statements.
                    // (I know a few people who would kill me for using this.)
            List<AvailabilityApiGetDto> apiGetDtoList = await _availabilities
                .GetAvailabilities(@event.TypeId, @event.Date, @event.Date.Add(@event.Duration.Value));
            List<Availability> availabilities = apiGetDtoList
                .Select(x => new Availability
                {
                    CostPerHour = x.CostPerHour,
                    Date = x.Date,
                    VenueCode = x.Code,
                    Venue = new Venue
                    {
                        Code = x.Code,
                        Description = x.Description,
                        Name = x.Name,
                        Capacity = x.Capacity
                    }
                }).ToList();

            // This block may be redundant depending on the availability behaviour and if a reserved availability is not included in the list.
            // (I assume it is not; thus this code block is commented out.)

            //List<Availability> nonReserved = new List<Availability>();
            //foreach (Availability availability in availabilities) 
            //{
            //    ReservationGetDto reservations = await _reservations.GetReservation(availability.VenueCode, @event.Date);
            //    if (reservations.Reference == null)
            //        nonReserved.Add(availability); // If the availability in the list has a reservation assigne then it is reserved.
            //}
            //availabilities.Clear();

            SelectList list = new SelectList(
                availabilities.Select(x => new {
                    x.Venue.Name,
                    x.VenueCode,
                    x.Date,
                    x.CostPerHour}
                ),"VenueCode","Name");
            EventVenueViewModel novenue = new EventVenueViewModel()
            {
                Title = @event.Title,
                Date = @event.Date,
                Duration = @event.Duration,
                Id = @event.Id,
                TypeId = @event.TypeId,
                TypeTitle = (await _eventTypes.GetEventType(@event.TypeId)).Title,
                Availabilities = availabilities,
                AvailabilitiesSelectList = list
            };
            return View(novenue);
        }

        /// <summary>
        /// A HTTP POST endpoint for "/Events/Venue/<paramref name="id"/>"
        /// </summary>
        /// <param name="id">The Id to be mapped to <see cref="Event.Id"/>.</param>
        /// <param name="ev">A <see cref="EventVenueViewModel"/> whose information is filled during the previous GET view.</param>
        /// <returns>A <see cref="EventVenueViewModel"/> for the <see cref="Event"/> returned via the Id <paramref name="id"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Venue(int? id, [Bind("Id,Date,SelectedVenue,TypeId")] EventVenueViewModel ev)
        {
            if (!ModelState.IsValid || !id.HasValue)
                return View(ev);
            
            if (ev.Id != id)
                return NotFound();

            Event @event = await _context.Events.FindAsync(id);
            if (@event == null || @event.Cancelled)
                return NotFound();

            // Create the reservation.
            ReservationGetDto reservationGetDto = await _reservations.CreateReservation(ev.Date, ev.SelectedVenue);

            // Empty view model in case reservation fails.
            EventVenueViewModel model = new EventVenueViewModel()
            {
                Id = ev.Id,
                Date = ev.Date,
                Title = @event.Title,
                Duration = @event.Duration,
                TypeId = ev.TypeId,
                TypeTitle = (await _eventTypes.GetEventType(ev.TypeId)).Title
            };

            if (reservationGetDto.Reference == null)
                return View(model);

            // Assign reservation information.
            model.Reservation = new EventReservationViewModel()
            {
                EventDate = reservationGetDto.EventDate,
                Reference = reservationGetDto.Reference,
                //StaffId = reservationGetDto.StaffId, // Read above for the same reasoning for the removal of this property.
                VenueCode = reservationGetDto.VenueCode,
                WhenMade = reservationGetDto.WhenMade
            };

            // Get venue information from any other availabilities of the same VenueCode (due to the lack of
            // information on the API).
            List<AvailabilityApiGetDto> avail = await _availabilities
                .GetAvailabilities(ev.TypeId, new DateTime(2018, 07, 10), new DateTime(2019, 2, 10));
            Venue venue = avail
                .Where(x => x.Code == reservationGetDto.VenueCode)
                .Select(a => new Venue() {
                    Code = a.Code,
                    Capacity = a.Capacity,
                    Description = a.Description,
                    Name = a.Name })
                .FirstOrDefault();
            model.Venue = venue;

            // Update reservation on the database entity.
            @event.VenueReservation = reservationGetDto.Reference;
            model.ReservationReference = reservationGetDto.Reference;
            await _context.SaveChangesAsync(); // Push database changes.
            
            // Show that view model from before.
            return View(model);
        }

        /// <summary>
        /// A HTTP GET endpoint for "/Events/Create". <para/>
        /// Shows information used for creation of a new <see cref="Event"/>.
        /// </summary>
        /// <returns>A <see cref="EventCreateViewModel"/> that contains a list of
        /// available <see cref="EventType.Id"/>s.</returns>
        public async Task<IActionResult> Create()
        {
            List<EventTypeDto> typeIds = await _eventTypes.GetEventTypes();
            List<EventTypeViewModel> types = null;
            if (typeIds != null)
            {
                types = typeIds.Select(x => new EventTypeViewModel() { Id = x.Id, Title = x.Title }).ToList();
            }
            EventCreateViewModel ev = new EventCreateViewModel() { ValidTypeIds = types };
            return View(ev);
        }

        /// <summary>
        /// A HTTP POST endpoint for "/Events/Create/". <para/>
        /// Uses the given <paramref name="event"/> to create a new <see cref="Event"/> from the bound data.
        /// </summary>
        /// <param name="event">The <see cref="EventCreateViewModel"/> which contains the bound information.</param>
        /// <returns>A redirection to the <see cref="Index"/> view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Date,Duration,TypeId")] EventCreateViewModel @event)
        {
            if (!ModelState.IsValid)
                return View(@event);

            Event e = new Event()
            {
                Id = @event.Id,
                Date = @event.Date,
                Duration = @event.Duration,
                Title = @event.Title,
                TypeId = @event.TypeId
            };
            _context.Add(e);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        /// <summary>
        /// A HTTP GET endpoint for "/Events/Edit/<paramref name="id"/>". <para/>
        /// Shows edit options for the given <see cref="Event"/>'s <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The Id to be mapped to <see cref="Event.Id"/>.</param>
        /// <returns>An <see cref="EventEditViewModel"/> which is sent to the 
        /// Edit view.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Event @event = await _context.Events.FindAsync(id);
            if (@event == null || @event.Cancelled)
                return NotFound();

            EventEditViewModel eventEditViewModel = new EventEditViewModel()
            {
                Duration = @event.Duration,
                Id = @event.Id,
                Title = @event.Title
            };
            return View(eventEditViewModel);
        }

        /// <summary>
        /// A HTTP POST endpoint for "/Events/Edit/<paramref name="id"/>". <para/>
        /// Edits the event's information within the database to the options given via <paramref name="event"/>.
        /// </summary>
        /// <param name="id">The Id to be mapped to <see cref="Event.Id"/>.</param>
        /// <param name="event">The bound information to be edited into the existing <see cref="Event"/>.</param>
        /// <returns>If the information sent via <paramref name="event"/> is valid; the user is redirected to
        /// the <see cref="Index"/> view; otherwise they are redirected to the <see cref="Edit(int?)"/> view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Duration,VenueToReserve")] EventEditViewModel @event)
        {
            if (id != @event.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(@event);

            try
            {

                Event e = await _context.Events
                    .FirstOrDefaultAsync(dbEvent => dbEvent.Id == @event.Id);
                if (e == null)
                    return BadRequest();

                e.Duration = @event.Duration;
                e.Title = @event.Title;
                _context.Update(e);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(@event.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));

        }

        /// <summary>
        /// A HTTP GET endpoint for "/Events/Delete/<paramref name="id"/>. <para/>
        /// Creates an <see cref="EventDeleteViewModel"/> with the information required by the View;
        /// shows confirmation of deletion of the event from the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The Id to be bound to <see cref="Event.Id"/>.</param>
        /// <returns>The Delete view with a respective <see cref="EventDeleteViewModel"/>.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            Event @event = await _context.Events
                .Where(c => !c.Cancelled)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
                return NotFound();

            EventDeleteViewModel vm = new EventDeleteViewModel()
            {
                Title = @event.Title,
                Id = @event.Id,
                Date = @event.Date,
                Duration = @event.Duration,
                TypeId = (await _eventTypes.GetEventType(@event.TypeId)).Title,
                VenueReservation = @event.VenueReservation
            };
            return View(vm);
        }

        /// <summary>
        /// A HTTP POST endpoint for "/Events/Delete/<paramref name="id"/>". <para/>
        /// Deletes the <see cref="Event"/> record within the database; freeing the staff and the venue reservation.
        /// </summary>
        /// <param name="id">The Id to be bound to <see cref="Event.Id"/>.</param>
        /// <returns>The <see cref="Index"/> view if the deletion is successful.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Event @event = await _context.Events.FindAsync(id);
            if (@event.Cancelled)
                return NotFound();
            var staff = await _context.EventStaff
                .Where(x => x.EventId == id).ToListAsync();
            _context.EventStaff.RemoveRange(staff);

            if (!string.IsNullOrEmpty(@event.VenueReservation))
                if (await _reservations.CancelReservation(@event.VenueReservation))
                    @event.VenueReservation = null;
            @event.Cancelled = true;
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// A HTTP POST endpoint for "/Events/VenueCancel/<paramref name="id"/>". <para/>
        /// Cancels the Venue reservation from the <paramref name="reference"/> given.
        /// </summary>
        /// <param name="id">The Id to be mapped to <see cref="Event.Id"/>.</param>
        /// <param name="reference">The <see cref="Venues.Data.Venue"/>'s <see cref="Venues.Data.Reservation"/> reference.</param>
        /// <returns>The <see cref="Venue"/> view if the deletion is successful.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VenueCancel(int? id, string reference)
        {
            if (!id.HasValue || reference == null)
                return NotFound();

            ReservationGetDto reservation = await _reservations.GetReservation(reference);
            if (reservation.Reference == null)
                return NotFound();

            Event @event = await _context.Events.FindAsync(id);
            if (@event == null || @event.Cancelled)
                return NotFound();

            await _reservations.CancelReservation(reference);
            return RedirectToAction(nameof(Venue), new { id });
        }

        /// <summary>
        /// A HTTP GET endpoint for "/Events/Saff/<paramref name="id"/>". <para/>
        /// Views the <see cref="Data.Staff"/> information for the given <see cref="Event"/> via a new 
        /// <see cref="EventStaffViewModel"/>.
        /// </summary>
        /// <param name="id">The Id to be mapped to <see cref="Event.Id"/>.</param>
        /// <returns>The <see cref="Staff"/> with a <see cref="EventStaffViewModel"/>.</returns>
        public async Task<IActionResult> Staff(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var ev = await _context.Events.FindAsync(id);
            if (ev == null || ev.Id != id || ev.Cancelled)
                return NotFound();

            var staff = await _context.Staff.ToListAsync();

            var eventStaff = await _context.EventStaff
                .Where(x => x.EventId == id)
                .Include(x => x.Staff)
                .ToListAsync();

            List<Staff> available = new List<Staff>();
            var occupiedIds = eventStaff.Select(x => x.StaffId).ToList();

            // If not in EventStaff
            foreach (Staff f in staff)
            {
                if (!occupiedIds.Contains(f.Id))
                    available.Add(f);
            }

            EventStaffViewModel vm = new EventStaffViewModel()
            {
                Id = ev.Id,
                Event = new EventDetailsViewModel() {
                    Id = ev.Id,
                    Date = ev.Date,
                    Title = ev.Title,
                    TypeId = (await _eventTypes.GetEventType(ev.TypeId)).Title,
                    Duration = ev.Duration
                },
                AvailableStaff = available
                    .Select(x => new Staff
                    {
                        Id = x.Id,
                        Email = x.Email,
                        FirstAider = x.FirstAider,
                        // Formatting for SelectList
                        Name = $"{(x.FirstAider ? "[First Aider]": "")} {x.Name} ({x.Email})"
                    }).ToList(),
                AssignedStaff = eventStaff
                    .Select(x => new StaffIndexViewModel {
                        Id = x.Staff.Id,
                        Email = x.Staff.Email,
                        Name = x.Staff.Name,
                        FirstAider = x.Staff.FirstAider
                    }).ToList(),
                WarningType = await GetWarningTypeFromEvent(ev)
            };

            return View(vm);
        }

        /// <summary>
        /// Calculates the <see cref="EventWarningType"/> enum value from the given <see cref="Event"/>.
        /// </summary>
        /// <param name="e">The <see cref="Event"/> to check the warnings of.</param>
        /// <returns>The applicable <see cref="EventWarningType"/> for the given <see cref="Event"/>.</returns>
        private async Task<EventWarningType> GetWarningTypeFromEvent(Event e)
        {
            var staff = await _context.EventStaff
                .Include(x => x.Staff)
                .Include(x => x.Event)
                .Where(x => x.EventId == e.Id && !x.Event.Cancelled)
                .Select(x => x.Staff)
                .ToListAsync();

            EventWarningType type = EventWarningType.None;
            if (!staff.Any(x => x.FirstAider))
                type = EventWarningType.NoFirstAider;

            var bookings = await _context.Guests
                .Where(x => x.EventId == e.Id)
                .ToListAsync();
            int count = bookings.Count / 10;

            if (staff.Count <= count)
                type |= EventWarningType.InsufficientStaff;

            return type;
        }

        /// <summary>
        /// HTTP POST endpoint of "/Events/AddStaff/<paramref name="Id"/>". <para/>
        /// Adds a staff member via their <paramref name="StaffId"/> to the <see cref="Event"/> via its <paramref name="Id"/>.
        /// </summary>
        /// <param name="Id">The <see cref="Event"/>'s Id.</param>
        /// <param name="StaffId">The <see cref="Data.Staff"/>'s Id.</param>
        /// <returns>Takes the user to the <see cref="Staff"/> page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStaff(int? Id, int StaffId)
        {
            if (!Id.HasValue)
                return NotFound();

            var existCheck = await _context.EventStaff.FindAsync(StaffId, Id);
            if (existCheck != null)
                return BadRequest();

            EventStaff staff = new EventStaff()
            {
                EventId = Id.Value,
                StaffId = StaffId
            };

            await _context.EventStaff.AddAsync(staff);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Staff), new { id = Id.Value });

        }

        /// <summary>
        /// HTTP GET endpoint of "Events/RemoveStaff/<paramref name="Id"/>?StaffId=<paramref name="StaffId"/>". <para/>
        /// Removes a staff member via their <paramref name="StaffId"/> to the <see cref="Event"/> via its <paramref name="Id"/>.
        /// </summary>
        /// <param name="Id">The <see cref="Event"/>'s Id.</param>
        /// <param name="StaffId">The <see cref="Data.Staff"/>'s Id.</param>
        /// <returns>Takes the user to the <see cref="Staff"/> page.</returns>
        public async Task<IActionResult> RemoveStaff(int? id, int StaffId)
        {
            if (!id.HasValue)
                return NotFound();

            var ev = await _context.EventStaff.FindAsync(StaffId, id);
            if (ev == null)
                return RedirectToAction(nameof(Staff), new { id });

            _context.Remove(ev);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Staff), new { id });
        }

        /// <summary>
        /// Checks whether or not an <see cref="Event"/> with the given <paramref name="id"/> exists.
        /// </summary>
        /// <param name="id">The <see cref="Event"/>'s Id.</param>
        /// <returns>True if the event exists; false otherwise.</returns>
        private bool EventExists(int id) => _context.Events.Where(c => !c.Cancelled).Any(e => e.Id == id);
    }
}

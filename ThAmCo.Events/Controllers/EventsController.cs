﻿using System;
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
    public class EventsController : Controller
    {
        private readonly EventsDbContext _context;
        private readonly IVenueAvailabilities _availabilities;
        private readonly IVenueReservation _reservations;
        private readonly IEventTypes _eventTypes;

        public EventsController(EventsDbContext context, IVenueAvailabilities availabilities, IVenueReservation reservations, IEventTypes eventTypes)
        {
            _context = context;
            _availabilities = availabilities;
            _reservations = reservations;
            _eventTypes = eventTypes;
        }

        // GET: Events
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

        // GET: Events/Details/5
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
                    .Select(x => new GuestBookingDetailsViewModel()
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
                        //StaffId = reservation.StaffId,
                        WhenMade = reservation.WhenMade,
                        VenueCode = reservation.VenueCode,
                        EventDate = reservation.EventDate
                    },
                    Venue = venue,
                    ReservationReference = reservation.Reference
                };
                return View(reservedViewModel);
            }

            NO_RES:
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


            List<Availability> nonReserved = new List<Availability>();
            foreach (Availability availability in availabilities)
            {
                ReservationGetDto reservations = await _reservations.GetReservation(availability.VenueCode, @event.Date);
                if (reservations.Reference == null)
                    nonReserved.Add(availability);
            }
            availabilities.Clear();

            SelectList list = new SelectList(
                nonReserved.Select(x => new {
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
                Availabilities = nonReserved,
                AvailabilitiesSelectList = list
            };
            return View(novenue);
        }

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

            ReservationGetDto reservationGetDto = await _reservations.CreateReservation(ev.Date, ev.SelectedVenue);
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
            {
                return View(model);
            } else
            {
                model.Reservation = new EventReservationViewModel()
                {
                    EventDate = reservationGetDto.EventDate,
                    Reference = reservationGetDto.Reference,
                    //StaffId = reservationGetDto.StaffId,
                    VenueCode = reservationGetDto.VenueCode,
                    WhenMade = reservationGetDto.WhenMade
                };

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

                @event.VenueReservation = reservationGetDto.Reference;
                model.ReservationReference = reservationGetDto.Reference;
                await _context.SaveChangesAsync();
            }

            return View(model);
        }

        // GET: Events/Create
        public async Task<IActionResult> Create()
        {
            List<EventTypeDto> typeIds = await _eventTypes.GetEventTypes();
            List<EventTypeViewModel> types = typeIds.Select(x => new EventTypeViewModel() { Id = x.Id, Title = x.Title }).ToList();
            EventCreateViewModel ev = new EventCreateViewModel() { ValidTypeIds = types };
            return View(ev);
        }

        // POST: Events/Create
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

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Event @event = await _context.Events.FindAsync(id);
            if (@event == null || @event.Cancelled)
                return NotFound();

            List<AvailabilityApiGetDto> apiGetDtos = await _availabilities
                .GetAvailabilities(@event.TypeId, @event.Date, @event.Date.Add(@event.Duration.Value));
            List<Availability> availabilities = new List<Availability>();
            foreach(AvailabilityApiGetDto avail in apiGetDtos)
            {
                availabilities.Add(new Availability()
                {
                    CostPerHour = avail.CostPerHour,
                    Date = avail.Date,
                    VenueCode = avail.Code,
                    Venue = new Venue()
                    {
                        Code = avail.Code,
                        Name = avail.Name,
                        Description = avail.Description,
                        Capacity = avail.Capacity
                    }
                });
            }

            SelectList applicableVenues = new SelectList(availabilities,"Code","Name",availabilities.FirstOrDefault());

            EventEditViewModel eventEditViewModel = new EventEditViewModel()
            {
                Duration = @event.Duration,
                Id = @event.Id,
                Title = @event.Title,
                ApplicableVenues = applicableVenues
            };
            return View(eventEditViewModel);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Duration,VenueToReserve")] EventEditViewModel @event)
        {
            if (id != @event.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {

                    Event e = await _context.Events
                        .FirstOrDefaultAsync(dbEvent => dbEvent.Id == @event.Id);
                    if (e == null)
                        return BadRequest();

                    // Reservation
                    Availability eventDto = @event.VenueToReserve;
                    if (eventDto != null)
                    {
                        ReservationGetDto re = await _reservations.GetReservation(eventDto.VenueCode, e.Date);
                        if (re == null)
                        {
                            // No reservation. Try and make one.
                            re = await _reservations.CreateReservation(e.Date, eventDto.VenueCode);
                        }
                        else
                        {
                            await _reservations.CancelReservation(re.Reference);
                            re = await _reservations.CreateReservation(e.Date, eventDto.VenueCode);
                        }
                    }
                    // End Reservation

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
            return View(@event);
        }

        // GET: Events/Delete/5
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

        // POST: Events/Delete/5
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
                await _reservations.CancelReservation(@event.VenueReservation);
            @event.Cancelled = true;
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

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

        private bool EventExists(int id) => _context.Events.Where(c => !c.Cancelled).Any(e => e.Id == id);

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
    }
}

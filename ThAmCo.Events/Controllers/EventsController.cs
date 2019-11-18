﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsDbContext _context;

        public EventsController(EventsDbContext context) => _context = context;

        // GET: Events
        public async Task<IActionResult> Index() => View(await _context.Events.ToListAsync());

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            Event @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
                return NotFound();

            EventDetailsViewModel viewModel = new EventDetailsViewModel()
            {
                Id = @event.Id,
                Title = @event.Title,
                Date = @event.Date,
                Duration = @event.Duration,
                TypeId = @event.TypeId
            };

            return View(viewModel);
        }

        // GET: Events/Create
        public IActionResult Create() => View();

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Date,Duration,TypeId")] EventCreateViewModel @event)
        {
            if (ModelState.IsValid)
            {
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
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Event @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return NotFound();

            EventEditViewModel eventEditViewModel = new EventEditViewModel()
            {
                Duration = @event.Duration,
                Id = @event.Id,
                Title = @event.Title
            };
            return View(eventEditViewModel);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Duration")] EventEditViewModel @event)
        {
            if (id != @event.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    Event e = await _context.Events.FirstOrDefaultAsync(dbEvent => dbEvent.Id == @event.Id);
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            Event @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
                return NotFound();

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Event @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id) => _context.Events.Any(e => e.Id == id);
    }
}
